#region U S A G E S

using System;
using System.Diagnostics;
using System.Threading;
using TryToExecute.Models;

#endregion

namespace TryExecuteTests.Tests.Models
{
    [TestClass]
    public class TryRetryPolicySyncCancelTests
    {

        [TestMethod]
        public void Execute_WithToken_CancelledBeforeCall_ShouldThrowOperationCanceledException_FuncNotInvoked_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            var invoked = 0;
            var policy = TryRetryPolicy.Fixed(maxAttempts: 3, delay: TimeSpan.FromMilliseconds(300));

            Assert.ThrowsException<OperationCanceledException>(() =>
                policy.Execute<int>(() =>
                {
                    invoked++;

                    return 1;
                }, cts.Token));

            Assert.AreEqual(0, invoked);
        }

        [TestMethod]
        public void Execute_WithToken_CancelledDuringBackoff_ShouldThrowOperationCanceledException_NotAllAttemptsRun_Test()
        {
            const int maxAttempts = 3;
            var baseDelay = TimeSpan.FromMilliseconds(300);
            var fullBackoffSumMs = baseDelay.TotalMilliseconds * (maxAttempts - 1);

            var policy = TryRetryPolicy.Fixed(maxAttempts: maxAttempts, delay: baseDelay, useJitter: false);

            var invoked = 0;
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            var sw = Stopwatch.StartNew();

            Assert.ThrowsException<OperationCanceledException>(() =>
                policy.Execute<int>(() =>
                {
                    invoked++;

                    throw new InvalidOperationException("transient failure");
                }, cts.Token));

            sw.Stop();

            Assert.IsTrue(invoked < maxAttempts);
            Assert.IsTrue(sw.ElapsedMilliseconds < baseDelay.TotalMilliseconds);
        }

        [TestMethod]
        public void Execute_WithNoneToken_AndLegacyOverload_BehaveIdentically_OnSuccess_Test()
        {
            var legacyPolicy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));
            var tokenPolicy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var legacyResult = legacyPolicy.Execute(() => 42);
            var noneTokenResult = tokenPolicy.Execute(() => 42, CancellationToken.None);

            Assert.AreEqual(42, legacyResult);
            Assert.AreEqual(42, noneTokenResult);
        }

        [TestMethod]
        public void Execute_WithNoneToken_AndLegacyOverload_BehaveIdentically_OnExhaustedRetries_Test()
        {
            var legacyPolicy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));
            var tokenPolicy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var legacyThrown = Assert.ThrowsException<InvalidOperationException>(() =>
                legacyPolicy.Execute<int>(() => throw new InvalidOperationException("boom")));

            var noneTokenThrown = Assert.ThrowsException<InvalidOperationException>(() =>
                tokenPolicy.Execute<int>(() => throw new InvalidOperationException("boom"), CancellationToken.None));

            Assert.AreEqual("boom", legacyThrown.Message);
            Assert.AreEqual("boom", noneTokenThrown.Message);
        }

        [TestMethod]
        public void Execute_WithLiveUncancelledToken_CompletesNormally_Test()
        {
            using var cts = new CancellationTokenSource();
            var policy = TryRetryPolicy.Fixed(maxAttempts: 2, delay: TimeSpan.FromMilliseconds(10));

            var result = policy.Execute(() => 7, cts.Token);

            Assert.AreEqual(7, result);
        }

        [TestMethod]
        public void Execute_WithToken_FuncThrowsOperationCanceledException_ShouldPropagateImmediately_NotRetried_Test()
        {
            var invoked = 0;
            var policy = TryRetryPolicy.Fixed(maxAttempts: 3, delay: TimeSpan.FromMilliseconds(10));

            Assert.ThrowsException<OperationCanceledException>(() =>
                policy.Execute<int>(() =>
                {
                    invoked++;

                    throw new OperationCanceledException("cancelled by func, not by the token");
                }, CancellationToken.None));

            Assert.AreEqual(1, invoked);
        }
    }
}
