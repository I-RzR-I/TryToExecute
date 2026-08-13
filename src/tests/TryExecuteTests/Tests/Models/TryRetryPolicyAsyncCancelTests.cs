#region U S A G E S

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.Models;

#endregion

namespace TryExecuteTests.Tests.Models
{
    [TestClass]
    public class TryRetryPolicyAsyncCancelTests
    {

        [TestMethod]
        public async Task ExecuteAsync_WithToken_CancelledBeforeCall_ShouldThrowOperationCanceledException_FuncNotInvoked_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var invoked = 0;
            var policy = TryRetryPolicy.Fixed(maxAttempts: 3, delay: TimeSpan.FromMilliseconds(300));

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
                policy.ExecuteAsync<int>(async ct =>
                {
                    invoked++;
                    await Task.CompletedTask;

                    return 1;
                }, cts.Token));

            Assert.AreEqual(0, invoked);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithToken_CancelledDuringBackoff_ShouldThrowOperationCanceledException_NotAllAttemptsRun_Test()
        {
            const int maxAttempts = 3;
            var baseDelay = TimeSpan.FromMilliseconds(300);
            var fullBackoffSumMs = baseDelay.TotalMilliseconds * (maxAttempts - 1);

            var policy = TryRetryPolicy.Fixed(maxAttempts: maxAttempts, delay: baseDelay, useJitter: false);

            var invoked = 0;
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            var sw = Stopwatch.StartNew();

            OperationCanceledException caught = null;
            try
            {
                await policy.ExecuteAsync<int>(async ct =>
                {
                    invoked++;
                    await Task.CompletedTask;

                    throw new InvalidOperationException("transient failure");
                }, cts.Token);
            }
            catch (OperationCanceledException ex)
            {
                caught = ex;
            }

            sw.Stop();

            Assert.IsNotNull(caught);
            Assert.IsTrue(invoked < maxAttempts);
            Assert.IsTrue(sw.ElapsedMilliseconds < baseDelay.TotalMilliseconds);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithNoneToken_OnSuccess_ReturnsValue_Test()
        {
            var policy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var result = await policy.ExecuteAsync<int>(async ct =>
            {
                await Task.CompletedTask;

                return 42;
            }, CancellationToken.None);

            Assert.AreEqual(42, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithNoneToken_OnExhaustedRetries_ThrowsLastException_Test()
        {
            var policy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var thrown = await Assert.ThrowsExceptionAsync<InvalidOperationException>(() =>
                policy.ExecuteAsync<int>(async ct =>
                {
                    await Task.CompletedTask;

                    throw new InvalidOperationException("boom");
                }, CancellationToken.None));

            Assert.AreEqual("boom", thrown.Message);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithLiveUncancelledToken_CompletesNormally_Test()
        {
            using var cts = new CancellationTokenSource();
            var policy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var result = await policy.ExecuteAsync<int>(async ct =>
            {
                await Task.CompletedTask;

                return 7;
            }, cts.Token);

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public async Task ExecuteAsync_WithToken_FuncThrowsOperationCanceledException_ShouldPropagateImmediately_NotRetried_Test()
        {
            var invoked = 0;
            var policy = TryRetryPolicy.Fixed(maxAttempts: 3, delay: TimeSpan.FromMilliseconds(10));

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(() =>
                policy.ExecuteAsync<int>(async ct =>
                {
                    invoked++;
                    await Task.CompletedTask;

                    throw new OperationCanceledException("cancelled by func, not by the token");
                }, CancellationToken.None));

            Assert.AreEqual(1, invoked);
        }
    }
}
