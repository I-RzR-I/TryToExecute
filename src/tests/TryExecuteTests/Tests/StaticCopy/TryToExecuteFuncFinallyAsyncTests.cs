// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-16 01:12
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-16 01:12
// ***********************************************************************
//  <copyright file="TryToExecuteFuncFinallyAsyncTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Enums;
using AggregatedGenericResultMessage.Extensions.Result;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using System.Linq;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.StaticCopy;

[TestClass]
public class TryToExecuteFuncFinallyAsyncTests : TryCatchExecuteStaticBase
{
    private ILogger<TryToExecuteFuncFinallyAsyncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryToExecuteFuncFinallyAsyncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryToExecuteFuncFinallyAsyncTests>();
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_FinallyFuncTaskTResult_ShouldPass_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureTResult_ILogger_FinallyFuncTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_FinallyFuncTaskTResult_ShouldPass_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_FinallyFuncTaskTResult_ShouldPass_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTaskTResult_ShouldPass_Test()
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryToExecuteFuncFinallyAsyncTests>(
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
            finallyExecFunc: async () =>
            {
                changedFinallyValue++;

                return await Task.FromResult(Result.Success().AddMessage("FinallyMessage", MessageType.Info));
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