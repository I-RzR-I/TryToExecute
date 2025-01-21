// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2024-12-08 22:19
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-08 22:19
// ***********************************************************************
//  <copyright file="TryCatchExecuteStaticExceptionFuncTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Extensions.Result;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable NotAccessedField.Local
#pragma warning disable CS8618
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.Abstract;

[TestClass]
public class TryCatchExecuteExceptionFuncTests : TryCatchExecuteBase
{
    private ILogger<TryCatchExecuteExceptionFuncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteExceptionFuncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteExceptionFuncTests>();
    }

    [TestMethod]
    public void TryToExec_Ex_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () => { changedValue++; return Result.Success(); },
            (ex) =>
            {
                Console.WriteLine(ex);
                changedValue++;

                return Result.Failure("FAil")
                    .WithError(ex);
            },
            () => { changedFinallyValue++; return Result.Success("Finally"); }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_Ex_ShouldPass_WithException_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () =>
            {
                throw new Exception("Ex1");
                changedValue++;

                return Result.Success();
            },
            (ex) =>
            {
                Console.WriteLine(ex);
                changedValue--;

                return Result.Failure("FAil")
                    .WithError(ex);
            },
            () => { changedFinallyValue++; return Result.Success("Finally"); }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual("FAil", exec.GetFirstMessage());
        Assert.AreEqual("Ex1", exec.Messages.ToArray()[1].Message.Info);
        Assert.AreEqual(-1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

   
}