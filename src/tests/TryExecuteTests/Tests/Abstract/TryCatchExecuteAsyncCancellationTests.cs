#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage.Enums;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.CodeExec;

#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Tests.Abstract;

[TestClass]
public class TryCatchExecuteAsyncCancellationTests : TryCatchExecuteBase
{
    private ILogger<TryCatchExecuteAsyncCancellationTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteAsyncCancellationTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteAsyncCancellationTests>();
    }

    #region Token flows into the exec delegate

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_TokenFlowsIntoDelegate_Test()
    {
        using var cts = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;
        var delegateWasInvoked = false;

        var exec = await TryToExecuteAsync(async ct =>
        {
            delegateWasInvoked = true;
            receivedToken = ct;

            return await Task.FromResult(42);
        }, -1, cancellationToken: cts.Token);

        Assert.IsTrue(delegateWasInvoked);
        Assert.AreEqual(cts.Token, receivedToken);
        Assert.IsFalse(receivedToken.IsCancellationRequested);
        Assert.AreEqual(42, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_NoTokenPassed_ReceivesDefaultToken_Test()
    {
        var receivedToken = CancellationToken.None;
        var receivedNonDefaultToken = false;

        var exec = await TryToExecuteAsync(async ct =>
        {
            receivedToken = ct;
            receivedNonDefaultToken = ct != default;

            return await Task.FromResult(7);
        }, -1);

        Assert.AreEqual(default(CancellationToken), receivedToken);
        Assert.IsFalse(receivedNonDefaultToken);
        Assert.AreEqual(7, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_FuncOnFailureResult_TokenFlowsIntoDelegate_Test()
    {
        using var cts = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;

        var exec = await TryToExecuteAsync(async ct =>
        {
            receivedToken = ct;

            return await Task.FromResult(5);
        }, () => -1, cancellationToken: cts.Token);

        Assert.AreEqual(cts.Token, receivedToken);
        Assert.AreEqual(5, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_WithFinally_TokenFlowsIntoDelegate_Test()
    {
        using var cts = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;
        var changedFinallyValue = 10;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async ct =>
            {
                receivedToken = ct;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure"),
            finallyExecFunc: () =>
            {
                changedFinallyValue++;

                return Result.Success();
            },
            cancellationToken: cts.Token);

        Assert.AreEqual(cts.Token, receivedToken);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_ILogger_TokenFlowsIntoDelegate_Test()
    {
        using var cts = new CancellationTokenSource();
        var receivedToken = CancellationToken.None;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncCancellationTests>(
            execFunc: async ct =>
            {
                receivedToken = ct;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure"),
            exceptionLogger: _logger,
            cancellationToken: cts.Token);

        Assert.AreEqual(cts.Token, receivedToken);
        Assert.IsTrue(exec.IsSuccess);
    }

    #endregion

    #region Success with a live (non-canceled) token - fallback not invoked

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_LiveToken_ShouldPass_Test()
    {
        using var cts = new CancellationTokenSource();
        var fallbackInvoked = false;

        var exec = await TryToExecuteAsync(async ct => await Task.FromResult(100), () =>
        {
            fallbackInvoked = true;

            return -1;
        }, cancellationToken: cts.Token);

        Assert.AreEqual(100, exec);
        Assert.IsFalse(fallbackInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndResultIResult_LiveToken_ShouldPass_Test()
    {
        using var cts = new CancellationTokenSource();
        var fallbackInvoked = false;

        var exec = await TryToExecuteAsync<IResult>(async ct => await Task.FromResult(Result.Success()),
            () =>
            {
                fallbackInvoked = true;

                return Result.Failure("ResultFailure");
            },
            cancellationToken: cts.Token);

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.IsFalse(fallbackInvoked);
    }

    #endregion

    #region Default / None token equivalence with the legacy overload behavior

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_DefaultToken_Success_MatchesLegacyBehavior_Test()
    {
        var exec = await TryToExecuteAsync(async (CancellationToken ct) => await Task.FromResult(1), -1);

        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_NoneToken_Success_MatchesLegacyBehavior_Test()
    {
        var exec = await TryToExecuteAsync(async (CancellationToken ct) => await Task.FromResult(1), -1,
            cancellationToken: CancellationToken.None);

        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_DefaultToken_NonCancelException_RoutesToOnFailureResult_Test()
    {
        var exec = await TryToExecuteAsync(async (CancellationToken ct) =>
        {
            throw new InvalidOperationException("Exception");

            return await Task.FromResult(0);
        }, -999);

        Assert.AreEqual(-999, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncAndTResult_NoneToken_NonCancelException_RoutesToOnFailureResult_Test()
    {
        var exec = await TryToExecuteAsync(async (CancellationToken ct) =>
        {
            throw new InvalidOperationException("Exception");

            return await Task.FromResult(0);
        }, -999, cancellationToken: CancellationToken.None);

        Assert.AreEqual(-999, exec);
    }

    #endregion

    #region Representative spread of overload shapes

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_ValueOnFailureResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_FuncOnFailureResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: () =>
            {
                changedExceptionValue++;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_FuncExceptionOnFailureResult_ShouldPass_Test()
    {
        var changedValue = 0;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: (Exception ex) => Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error));

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_FuncExceptionOnFailureResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        Exception capturedException = null;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;
                throw new InvalidOperationException("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: (Exception ex) =>
            {
                capturedException = ex;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.IsNotNull(capturedException);
        Assert.IsInstanceOfType(capturedException, typeof(InvalidOperationException));
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_WithFinally_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
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
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_WithFinally_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async (CancellationToken ct) =>
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
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_ILogger_ShouldPass_Test()
    {
        var changedValue = 0;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncCancellationTests>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.IsTrue(exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_ExecFuncTResult_ILogger_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;

        var exec = await TryToExecuteAsync<IResult, TryCatchExecuteAsyncCancellationTests>(
            execFunc: async (CancellationToken ct) =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error),
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
    }

    #endregion
}
