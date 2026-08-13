// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 00:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 00:05
// ***********************************************************************
//  <copyright file="TryRetryPolicy.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.Enums;
using TryToExecute.Extensions;
using TryToExecute.Helpers;

#endregion

namespace TryToExecute.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A try retry policy.
    /// </summary>
    /// =================================================================================================
    public sealed class TryRetryPolicy
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the retry operation.
        /// </summary>
        /// =================================================================================================
        private readonly TryRetryOptions _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryRetryPolicy"/> class.
        /// </summary>
        /// <param name="options">
        ///     (Optional)
        ///     (Immutable) options for controlling the retry operation.
        /// </param>
        /// =================================================================================================
        public TryRetryPolicy(TryRetryOptions options = null)
            => _options = options ?? new TryRetryOptions();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the operation asynchronous.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="func">The function/action that must be executed.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A T.
        /// </returns>
        /// =================================================================================================
        public async Task<T> ExecuteAsync<T>(Func<CancellationToken, Task<T>> func, CancellationToken cancellationToken = default)
        {
            func.ThrowIfArgNull(nameof(func));

            var attempts = Math.Max(1, _options.MaxAttempts);
            Exception lastEx = null;

            for (var attempt = 1; attempt <= attempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    return await func(cancellationToken);
                }
                catch (Exception ex) when (ex.IsTypeOfOperationCancel().IsFalse())
                {
                    lastEx = ex;

                    // call OnRetry (attempt index is the failed attempt; e.g. if attempt==1, first attempt failed)
                    InvokeOnRetry(attempt, ex);

                    if ((_options.ShouldRetryOn?.Invoke(ex) ?? true).IsFalse() || attempt == attempts)
                        throw;

                    var delay = ComputeRetryDelay(attempt);
                    await Task.Delay(delay, cancellationToken);
                }
            }

            throw lastEx ?? new InvalidOperationException(DefaultMessageHelper.InvalidOperationRetryPolicy);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the given func/operation.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="func">The function/action that must be executed.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        /// =================================================================================================
        public T Execute<T>(Func<T> func)
            => Execute(func, CancellationToken.None);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the given func/operation, cancellable through the given
        ///     <paramref name="cancellationToken"/>.
        /// </summary>
        /// <exception cref="Exception">Thrown when an exception error condition occurs.</exception>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when cancellation is requested through <paramref name="cancellationToken"/>.
        /// </exception>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="func">The function/action that must be executed.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// <returns>
        ///     A T.
        /// </returns>
        /// =================================================================================================
        public T Execute<T>(Func<T> func, CancellationToken cancellationToken)
        {
            func.ThrowIfArgNull(nameof(func));

            var attempts = Math.Max(1, _options.MaxAttempts);
            Exception lastEx = null;

            for (var attempt = 1; attempt <= attempts; attempt++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    return func();
                }
                catch (Exception ex) when (ex.IsTypeOfOperationCancel().IsFalse())
                {
                    lastEx = ex;
                    InvokeOnRetry(attempt, ex);

                    if ((_options.ShouldRetryOn?.Invoke(ex) ?? true).IsFalse() || attempt == attempts)
                        throw;

                    var delay = ComputeRetryDelay(attempt);
                    ThreadSleep(delay, cancellationToken);
                }
            }

            throw lastEx ?? new InvalidOperationException(DefaultMessageHelper.InvalidOperationRetryPolicy);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Convenience factory for exponential policy.
        /// </summary>
        /// <param name="maxAttempts">(Optional) The maximum attempts.</param>
        /// <param name="baseDelay">(Optional) The base delay.</param>
        /// <param name="factor">(Optional) The factor.</param>
        /// <param name="useJitter">(Optional) True to use jitter.</param>
        /// <returns>
        ///     A TryRetryPolicy.
        /// </returns>
        /// =================================================================================================
        public static TryRetryPolicy Exponential(int maxAttempts = 3, TimeSpan? baseDelay = null, double factor = 2.0, bool useJitter = true)
            => new TryRetryPolicy(new TryRetryOptions()
            {
                MaxAttempts = maxAttempts,
                BaseDelay = baseDelay ?? TimeSpan.FromMilliseconds(200),
                Factor = factor,
                UseJitter = useJitter,
                Strategy = TryRetryBackoffStrategy.Exponential
            });

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Convenience factory for fixed policy.
        /// </summary>
        /// <param name="maxAttempts">(Optional) The maximum attempts.</param>
        /// <param name="delay">(Optional) The delay.</param>
        /// <param name="useJitter">(Optional) True to use jitter.</param>
        /// <returns>
        ///     A TryRetryPolicy.
        /// </returns>
        /// =================================================================================================
        public static TryRetryPolicy Fixed(int maxAttempts = 3, TimeSpan? delay = null, bool useJitter = false)
            => new TryRetryPolicy(new TryRetryOptions()
            {
                MaxAttempts = maxAttempts,
                BaseDelay = delay ?? TimeSpan.FromMilliseconds(200),
                UseJitter = useJitter,
                Strategy = TryRetryBackoffStrategy.Fixed
            });

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Calculates the retry delay.
        /// </summary>
        /// <param name="attempt">The attempt.</param>
        /// <returns>
        ///     The calculated retry delay.
        /// </returns>
        /// =================================================================================================
        private TimeSpan ComputeRetryDelay(int attempt)
        {
            // strategy = TryRetryBackoffStrategy.Fixed
            if (_options.Strategy == TryRetryBackoffStrategy.Fixed)
            {
                var baseDelay = _options.BaseDelay;
                if (_options.UseJitter.IsFalse())
                    return baseDelay;

                // jitter between 0 and baseDelay using provider
                var p = _options.RandomProvider?.Invoke() ?? 0.0;
                var jitterMs = (int)(p * baseDelay.TotalMilliseconds);

                return TimeSpan.FromMilliseconds(jitterMs);
            }

            // strategy = TryRetryBackoffStrategy.Exponential
            var factorPow = Math.Pow(_options.Factor, attempt - 1);
            var delayMs = _options.BaseDelay.TotalMilliseconds * factorPow;
            if (_options.UseJitter)
            {
                var p = _options.RandomProvider?.Invoke() ?? 0.0;
                delayMs = p * delayMs; // jitter between 0 .. delayMs
            }

            return TimeSpan.FromMilliseconds(Math.Max(0, delayMs));
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Executes the on retry on a different thread, and waits for the result.
        /// </summary>
        /// <param name="attempt">The attempt.</param>
        /// <param name="ex">The exception.</param>
        /// <remarks>
        ///     On exception, swallow user <see cref="TryRetryOptions.OnRetry"/> exceptions to avoid breaking the retry loop.
        /// </remarks>
        /// =================================================================================================
        private void InvokeOnRetry(int attempt, Exception ex)
        {
            try
            {
                if (_options.OnRetry.IsNull())
                    return;

                _options.OnRetry?.Invoke(attempt, ex);
            }
            catch
            {
                /*ignored*/
                /*avoid breaking retry loop*/
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Thread sleep. Local thread delay, cancellable through the given <paramref name="cancellationToken"/>.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when cancellation is requested through <paramref name="cancellationToken"/>.
        /// </exception>
        /// <param name="timeSpan">The time span.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// =================================================================================================
        private static void ThreadSleep(TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            if (timeSpan.IsLessOrZero().IsTrue())
                return;

#if NETSTANDARD1_0
            timeSpan.InternalSleep(cancellationToken);
#else
            if (cancellationToken.CanBeCanceled)
            {
                if (cancellationToken.WaitHandle.WaitOne(timeSpan))
                    cancellationToken.ThrowIfCancellationRequested();
            }
            else
            {
                Thread.Sleep(timeSpan);
            }
#endif
        }
    }
}