using System;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.Builder;
using TryToExecute.Enums;
using TryToExecute.Models;

namespace TryExecuteTests.Tests.Fluent
{
    [TestClass]
    public class TryExecuteBuilderRegressionTests
    {

        [TestMethod]
        public void DoAction_Throw_InvalidOperationEx_CaughtByBaseExceptionHandler_Test()
        {
            int exceptionResult = 0;

            var build = TryBuilder.Do(() =>
                {
                    throw new InvalidOperationException("Boom");

                    return 10;
                })
                .Catch<Exception>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(InvalidOperationException), build.Exception.GetType());
            Assert.AreEqual(1, exceptionResult);
        }

        [TestMethod]
        public async Task TryBuilder_Async_Throw_InvalidOperationEx_CaughtByBaseExceptionHandler_Test()
        {
            int exceptionResult = 0;

            var build = await TryBuilder.Do(async () =>
                {
                    await Task.CompletedTask;

                    throw new InvalidOperationException("Boom");
                })
                .Catch<Exception>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(InvalidOperationException), build.Exception.GetType());
            Assert.AreEqual(1, exceptionResult);
        }

        [TestMethod]
        public void DoAction_WithToken_Finally_ReceivesSameToken_Test()
        {
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            CancellationToken receivedInAction = default;
            CancellationToken receivedInFinally = default;

            var build = TryBuilder.Do((CancellationToken ct) =>
                {
                    receivedInAction = ct;
                }, token)
                .Finally(ct => receivedInFinally = ct)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.AreEqual(token, receivedInAction);
            Assert.AreEqual(token, receivedInFinally);
        }

        [TestMethod]
        public void Do_WithToken_Fallback_ReceivesSameToken_Test()
        {
            using var cts = new CancellationTokenSource();
            var token = cts.Token;

            CancellationToken receivedInFallback = default;

            var build = TryBuilder.Do<int>(ct =>
                {
                    throw new InvalidOperationException("Boom");

                    return 10;
                }, token)
                .Catch<InvalidOperationException>(exception => { Console.Write(exception.ToString()); })
                .Fallback(ct =>
                {
                    receivedInFallback = ct;

                    return 42;
                })
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.AreEqual(42, build.Value);
            Assert.AreEqual(token, receivedInFallback);
        }

        [TestMethod]
        public void Execute_PreCanceledToken_PropagatesOperationCanceledException_FallbackNotRun_FinallyRuns_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            int fallbackRan = 0;
            int finallyRan = 0;

            Assert.ThrowsException<OperationCanceledException>(() =>
            {
                TryBuilder.Do<int>(ct =>
                    {
                        ct.ThrowIfCancellationRequested();

                        return 10;
                    }, cts.Token)
                    .Fallback(() => fallbackRan = 1)
                    .Finally(() => finallyRan = 1)
                    .Execute();
            });

            Assert.AreEqual(0, fallbackRan);
            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public void Execute_MidFlightCancellation_PropagatesOperationCanceledException_FallbackNotRun_FinallyRuns_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            int fallbackRan = 0;
            int finallyRan = 0;

            Assert.ThrowsException<OperationCanceledException>(() =>
            {
                TryBuilder.Do<int>(ct =>
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            ct.ThrowIfCancellationRequested();
                            Thread.Sleep(5);
                        }

                        return 10;
                    }, cts.Token)
                    .Fallback(() => fallbackRan = 1)
                    .Finally(() => finallyRan = 1)
                    .Execute();
            });

            Assert.AreEqual(0, fallbackRan);
            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public async Task ExecuteAsync_PreCanceledToken_PropagatesOperationCanceledException_FallbackNotRun_FinallyRuns_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.Cancel();

            int fallbackRan = 0;
            int finallyRan = 0;

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await TryBuilder.Do<int>(async ct =>
                    {
                        ct.ThrowIfCancellationRequested();
                        await Task.CompletedTask;

                        return 10;
                    }, cts.Token)
                    .Fallback(() => fallbackRan = 1)
                    .Finally(() => finallyRan = 1)
                    .ExecuteAsync();
            });

            Assert.AreEqual(0, fallbackRan);
            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public async Task ExecuteAsync_MidFlightCancellation_PropagatesOperationCanceledException_FallbackNotRun_FinallyRuns_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            int fallbackRan = 0;
            int finallyRan = 0;

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await TryBuilder.Do<int>(async ct =>
                    {
                        for (var i = 0; i < 100; i++)
                        {
                            ct.ThrowIfCancellationRequested();

                            await Task.Delay(5);
                        }

                        return 10;
                    }, cts.Token)
                    .Fallback(() => fallbackRan = 1)
                    .Finally(() => finallyRan = 1)
                    .ExecuteAsync();
            });

            Assert.AreEqual(0, fallbackRan);
            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public void Execute_RetryPolicy_CancelDuringBackoff_StopsRetryingPromptly_Test()
        {
            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromMilliseconds(50));

            int attemptCount = 0;
            int fallbackRan = 0;
            int finallyRan = 0;

            Assert.ThrowsException<OperationCanceledException>(() =>
            {
                TryBuilder.Do((Func<CancellationToken, int>)(ct =>
                    {
                        attemptCount++;
                        throw new InvalidOperationException("Boom");
                    }), cts.Token)
                    .Fallback(() => fallbackRan = 1)
                    .Finally(() => finallyRan = 1)
                    .Retry(new TryRetryPolicy(new TryRetryOptions
                    {
                        Strategy = TryRetryBackoffStrategy.Fixed,
                        MaxAttempts = 3,
                        BaseDelay = TimeSpan.FromMilliseconds(250),
                        UseJitter = false
                    }))
                    .Execute();
            });

            Assert.IsTrue(attemptCount < 3);
            Assert.AreEqual(0, fallbackRan);
            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public void DoAction_Catch_BaseRegisteredFirst_DerivedThrown_FirstMatchWins_BaseHandlerRuns_Test()
        {
            var baseRan = false;
            var derivedRan = false;

            var build = TryBuilder.Do((Action)(() => throw new ArgumentNullException("Ex")))
                .Catch<ArgumentException>(_ => baseRan = true)
                .Catch<ArgumentNullException>(_ => derivedRan = true)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), build.Exception.GetType());
            Assert.IsTrue(baseRan);
            Assert.IsFalse(derivedRan);
        }

        [TestMethod]
        public async Task TryBuilder_Async_Catch_BaseRegisteredFirst_DerivedThrown_FirstMatchWins_BaseHandlerRuns_Test()
        {
            var baseRan = false;
            var derivedRan = false;

            var build = await TryBuilder.Do(async () =>
                {
                    await Task.CompletedTask;

                    throw new ArgumentNullException("Ex");
                })
                .Catch<ArgumentException>(async exception =>
                {
                    await Task.CompletedTask;
                    Console.Write(exception.ToString());
                    baseRan = true;
                })
                .Catch<ArgumentNullException>(async exception =>
                {
                    await Task.CompletedTask;
                    Console.Write(exception.ToString());
                    derivedRan = true;
                })
                .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), build.Exception.GetType());
            Assert.IsTrue(baseRan);
            Assert.IsFalse(derivedRan);
        }

        [TestMethod]
        public void Execute_FallbackThrowsOperationCanceledException_Propagates_FinallyStillRuns_Test()
        {
            var finallyRan = 0;

            Assert.ThrowsException<OperationCanceledException>(() =>
            {
                TryBuilder.Do((Action)(() => throw new InvalidOperationException("Boom")))
                    .Catch<InvalidOperationException>(exception => Console.Write(exception.ToString()))
                    .Fallback(() => throw new OperationCanceledException("fallback cancelled"))
                    .Finally(() => finallyRan = 1)
                    .Execute();
            });

            Assert.AreEqual(1, finallyRan);
        }

        [TestMethod]
        public async Task ExecuteAsync_FallbackThrowsOperationCanceledException_Propagates_FinallyStillRuns_Test()
        {
            var finallyRan = 0;

            await Assert.ThrowsExceptionAsync<OperationCanceledException>(async () =>
            {
                await TryBuilder.Do(async () =>
                    {
                        await Task.CompletedTask;

                        throw new InvalidOperationException("Boom");
                    })
                    .Catch<InvalidOperationException>(exception => Console.Write(exception.ToString()))
                    .Fallback((Func<object>)(() => throw new OperationCanceledException("fallback cancelled")))
                    .Finally(() => finallyRan = 1)
                    .ExecuteAsync();
            });

            Assert.AreEqual(1, finallyRan);
        }
    }
}
