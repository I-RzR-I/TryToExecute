#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Enums;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TryToExecute.CodeExec;

// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Tests.StaticCopy;

[TestClass]
public class TryCatchExecuteAsyncTests : TryCatchExecuteStaticBase
{
    private ILogger<TryCatchExecuteAsyncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteAsyncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteAsyncTests>();
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_FuncAndTResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync(async () =>
        {
            throw new Exception("Exception");

            return await Task.FromResult(0);
        }, -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_FuncAndTResult_WithFuncExceptionDefault_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync(async () =>
        {
            throw new Exception("Exception");

            return await Task.FromResult(0);
        }, () =>
        {
            _logger.LogError("Internal error!");

            return -999;
        });

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync(async () => await Task.FromResult(1), -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithFuncResult_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () => await Task.FromResult(Result.Success()),
            () => Result.Failure("ResultFailure"));

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithFuncResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () =>
            {
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            () => Result.Failure("ResultFailure"));

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithAsyncErrorFuncResult_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () => await Task.FromResult(Result.Success()),
            async () => await Task.FromResult(Result.Failure("ResultFailure")));

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithAsyncErrorFuncResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () =>
            {
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            async () => await Task.FromResult(Result.Failure("ResultFailure")));

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTResult_FailureTResult_FinallyFuncTResult_ShouldFailWithMessage_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTResult_FailureTResult_FinallyFuncTResult_ShouldPass_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTResult_FailureFuncTResult_ShouldFailWithMessage_Test()
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
            }
            );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTResult_FailureFuncTResult_ShouldPass_Test()
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
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ShouldFailWithMessage_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            }
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ShouldPass_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            }
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ShouldFailWithMessage_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            }
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ShouldPass_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
            }
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }
  
    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }
   
    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResult_ILogger_FinallyFuncTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTResultException_ILogger_FinallyFuncTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ShouldFailWithMessage_Test()
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ShouldPass_Test()
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    } 

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTResult_ShouldFailWithMessage_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_FinallyFuncTResult_ShouldPass_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyFuncTResult_ShouldFailWithMessage_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_FinallyFuncTResult_ShouldPass_Test()
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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
            forceCallGarbageCollector: false
        );

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(string.Empty, exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResult_ILogger_FinallyFuncTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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
    public async Task TryToExecFuncAsync_ExecFuncTaskTResult_FailureFuncTaskTResultException_ILogger_FinallyFuncTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncTests>(
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

                return Result.Success().AddMessage("FinallyMessage", MessageType.Info);
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