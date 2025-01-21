// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-16 10:29
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-16 10:29
// ***********************************************************************
//  <copyright file="TryToExecuteFuncTaskFinallyAsyncTests.cs" company="RzR SOFT & TECH">
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
public class TryToExecuteFuncTaskFinallyAsyncTests : TryCatchExecuteBase
{
    private ILogger<TryToExecuteFuncTaskFinallyAsyncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryToExecuteFuncTaskFinallyAsyncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryToExecuteFuncTaskFinallyAsyncTests>();
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyFuncTask_ShouldFailWithMessage_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyFuncTask_ShouldPass_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyFuncTask_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyFuncTask_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyFuncTask_ShouldFailWithMessage_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyFuncTask_ShouldPass_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyFuncTask_ShouldFailWithMessage_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyFuncTask_ShouldPass_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTask_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTask_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTask_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTask_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTask_ShouldFailWithMessage_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTask_ShouldPass_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyFuncTask_ShouldFailWithMessage_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyFuncTask_ShouldPass_Test()
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTask_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTask_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTask_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTask_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncTaskFinallyAsyncTests>(
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
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Task.CompletedTask;
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