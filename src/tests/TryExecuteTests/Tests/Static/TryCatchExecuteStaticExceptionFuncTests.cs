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
using System.Threading.Tasks;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable NotAccessedField.Local
#pragma warning disable CS8618
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.Static;

[TestClass]
public class TryCatchExecuteStaticExceptionFuncTests : TryCatchExecuteStaticBase
{
    private ILogger<TryCatchExecuteStaticExceptionFuncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteStaticExceptionFuncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteStaticExceptionFuncTests>();
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

    [TestMethod]
    public async Task TryToExecAsync_Ex_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = await TryToExecuteAsync(
            async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            async (ex) =>
            {
                Console.WriteLine(ex);
                changedValue++;

                return await Task.FromResult(
                    Result.Failure("FAil")
                    .WithError(ex));
            },
            async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success("Finally"));
            }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public async Task TryToExecAsync_Ex_ShouldPass_WithException_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = await TryToExecuteAsync(
            async () =>
            {
                throw new Exception("Ex1");
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            async (ex) =>
            {
                Console.WriteLine(ex);
                changedValue--;

                return await Task.FromResult(Result.Failure("FAil").WithError(ex));
            },
            async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success("Finally"));
            }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual("FAil", exec.GetFirstMessage());
        Assert.AreEqual("Ex1", exec.Messages.ToArray()[1].Message.Info);
        Assert.AreEqual(-1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }
}