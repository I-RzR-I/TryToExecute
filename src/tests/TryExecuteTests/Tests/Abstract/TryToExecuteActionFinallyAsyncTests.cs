// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-16 17:14
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-16 17:14
// ***********************************************************************
//  <copyright file="TryToExecuteActionFinallyAsyncTests.cs" company="RzR SOFT & TECH">
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
using System.Threading.Tasks;
using System;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.Abstract;

[TestClass]
public class TryToExecuteActionFinallyAsyncTests : TryCatchExecuteBase
{
    private ILogger<TryToExecuteActionFinallyAsyncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryToExecuteActionFinallyAsyncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryToExecuteActionFinallyAsyncTests>();
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: () =>
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: () =>
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
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

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async () =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async () =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async () =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async () =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyAction_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyAction_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteActionFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;

                return await Task.FromResult(Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));
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