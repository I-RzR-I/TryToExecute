// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-15 20:14
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-15 20:14
// ***********************************************************************
//  <copyright file="TryToExecuteActionFinallyTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Enums;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Microsoft.Extensions.Logging;
using System;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.Abstract;

[TestClass]
public class TryToExecuteActionFinallyTests : TryCatchExecuteBase
{
    private ILogger<TryToExecuteActionFinallyTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryToExecuteActionFinallyTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryToExecuteActionFinallyTests>();
    }
    
    [TestMethod]
    public void TryToExecAction_ActionFinally_ShouldPass_Test()
    {
        var changedValue = 0;
        var exec = TryToExecute(1, -1, () => { changedValue = 10; });

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
        Assert.AreEqual(10, changedValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureTResult_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureTResult_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureTResult_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureTResult_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }
 
    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResult_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: () => Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResult_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: () => Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    } 
 
    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResultException_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: (Exception ex) =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResultException_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: (Exception ex) =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    } 
  
    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResult_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: () =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResult_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: () =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }  
 
    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResultException_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return Result.Success();
            },
            onFailureResult: (Exception ex) =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_ExecFuncTResult_FailureFuncTResultException_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = TryToExecute<IResult, TryToExecuteActionFinallyTests>(
            execFunc: () =>
            {
                changedValue++;

                return Result.Success();
            },
            onFailureResult: (Exception ex) =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            },
            exceptionLogger: _logger,
            finallyExecAction: () =>
            {
                changedFinallyValue++;
            },
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }
}