#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.CodeExec;

#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Tests.Abstract;

[TestClass]
public class TryCatchExecuteAsyncCancelEdgeTests : TryCatchExecuteBase
{
    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_CancelledBeforeCall_ShouldThrowOperationCanceledException_FailureNotInvoked_Test()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var execInvoked = 0;
        var failureInvoked = 0;

        var thrown = await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            TryToExecuteAsync<int>(
                execFunc: async ct =>
                {
                    execInvoked++;
                    return await Task.FromResult(1);
                },
                onFailureResult: () =>
                {
                    failureInvoked++;
                    return -1;
                },
                forceCallGarbageCollector: false,
                cancellationToken: cts.Token));

        Assert.IsNotNull(thrown);
        Assert.AreEqual(0, execInvoked);
        Assert.AreEqual(0, failureInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_CancelledDuringExecution_ThrowIfCancellationRequested_ShouldThrowOperationCanceledException_FailureNotInvoked_Test()
    {
        using var cts = new CancellationTokenSource();
        var failureInvoked = 0;

        var thrown = await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            TryToExecuteAsync<int>(
                execFunc: async ct =>
                {
                    await Task.Yield();
                    await cts.CancelAsync();
                    ct.ThrowIfCancellationRequested();

                    return 1;
                },
                onFailureResult: () =>
                {
                    failureInvoked++;
                    return -1;
                },
                forceCallGarbageCollector: false,
                cancellationToken: cts.Token));

        Assert.IsNotNull(thrown);
        Assert.AreEqual(0, failureInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_CancelledDuringExecution_TaskDelayObservesToken_ShouldThrowTaskCanceledException_FailureNotInvoked_Test()
    {
        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromMilliseconds(25));

        var failureInvoked = 0;

        var thrown = await Assert.ThrowsExceptionAsync<TaskCanceledException>(() =>
            TryToExecuteAsync<int>(
                execFunc: async ct =>
                {
                    await Task.Delay(TimeSpan.FromSeconds(5), ct);

                    return 1;
                },
                onFailureResult: () =>
                {
                    failureInvoked++;
                    return -1;
                },
                forceCallGarbageCollector: false,
                cancellationToken: cts.Token));

        Assert.IsNotNull(thrown);
        Assert.IsInstanceOfType(thrown, typeof(OperationCanceledException));
        Assert.AreEqual(0, failureInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_DelegateThrowsTaskCanceledExceptionDirectly_NonCancelledToken_ShouldPropagate_FailureNotInvoked_Test()
    {
        var failureInvoked = 0;

        var thrown = await Assert.ThrowsExceptionAsync<TaskCanceledException>(() =>
            TryToExecuteAsync<int>(
                execFunc: async ct =>
                {
                    await Task.Yield();
                    throw new TaskCanceledException();
                },
                onFailureResult: () =>
                {
                    failureInvoked++;
                    return -1;
                },
                forceCallGarbageCollector: false,
                cancellationToken: CancellationToken.None));

        Assert.IsNotNull(thrown);
        Assert.IsInstanceOfType(thrown, typeof(OperationCanceledException));
        Assert.AreEqual(0, failureInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_FinallyOverload_CancelledBeforeCall_FinallyStillRuns_OperationCanceledExceptionPropagates_FailureNotInvoked_Test()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();

        var failureInvoked = 0;
        var finallyInvoked = 0;

        var thrown = await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
            TryToExecuteAsync<int>(
                execFunc: async ct =>
                {
                    return await Task.FromResult(1);
                },
                onFailureResult: () =>
                {
                    failureInvoked++;
                    return -1;
                },
                finallyExecFunc: () =>
                {
                    finallyInvoked++;
                    return 0;
                },
                forceCallGarbageCollector: false,
                cancellationToken: cts.Token));

        Assert.IsNotNull(thrown);
        Assert.AreEqual(1, finallyInvoked);
        Assert.AreEqual(0, failureInvoked);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_TokenAware_NonCancelledToken_DelegateThrowsNormalException_ShouldRouteToOnFailureResult_Test()
    {
        var failureInvoked = 0;

        var exec = await TryToExecuteAsync<int>(
            execFunc: async ct =>
            {
                throw new InvalidOperationException("Regular failure");

                return await Task.FromResult(1);
            },
            onFailureResult: () =>
            {
                failureInvoked++;
                return -999;
            },
            forceCallGarbageCollector: false,
            cancellationToken: CancellationToken.None);

        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, failureInvoked);
    }
}
