// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 00:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 16:10
// ***********************************************************************
//  <copyright file="TryBuilderSync.cs" company="RzR SOFT & TECH">
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
using TryToExecute.Extensions;
using TryToExecute.Models;
using TryToExecute.Models.Internal;

#endregion

namespace TryToExecute.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Sync fluent builder for try/catch/finally flows.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:TryToExecute.Builder.TryBuilderBase" />
    /// =================================================================================================
    public class TryBuilderSync<T> : TryBuilderBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the try block with token.
        /// </summary>
        /// =================================================================================================
        private readonly Func<CancellationToken, T> _tryBlockWithToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback synchronize.
        /// </summary>
        /// =================================================================================================
        private Func<T> _fallbackSync;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback synchronize with token.
        /// </summary>
        /// =================================================================================================
        private Func<CancellationToken, T> _fallbackSyncWithToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderSync{T}" /> class.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// =================================================================================================
        public TryBuilderSync(Func<T> tryBlock) : this(_ => tryBlock(), CancellationToken.None) { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderSync{T}" /> class.
        /// </summary>
        /// <param name="tryBlockWithToken">The try block with token.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// =================================================================================================
        public TryBuilderSync(Func<CancellationToken, T> tryBlockWithToken, CancellationToken cancellationToken = default)
        {
            if (tryBlockWithToken.IsNull())
                tryBlockWithToken.ThrowIfArgNull(nameof(tryBlockWithToken));

            _tryBlockWithToken = tryBlockWithToken;
            BaseCancellationToken = cancellationToken;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Catches the given handler (Exception catch handler).
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Catch<TException>(Action<TException> handler)
            where TException : Exception
        {
            if (handler.IsNull())
                handler.ThrowIfArgNull(nameof(handler));

            BaseCatchHandlers.Add(new ExceptionHandler(typeof(TException), ex =>
            {
                handler((TException)ex);

                return Task.FromResult<object>(null);
            }));

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Catches the given handler (Exception catch handler).
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Catch<TException>(Action<TException, CancellationToken> handler)
            where TException : Exception
        {
            if (handler.IsNull())
                handler.ThrowIfArgNull(nameof(handler));

            BaseCatchHandlers.Add(new ExceptionHandler(typeof(TException), ex =>
            {
                handler((TException)ex, BaseCancellationToken);

                return Task.FromResult<object>(null);
            }));

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The final execution function (in the finally block).
        /// </summary>
        /// <param name="finallyBlock">The finally block.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Finally(Action finallyBlock)
        {
            if (finallyBlock.IsNull())
                finallyBlock.ThrowIfArgNull(nameof(finallyBlock));

            BaseFinallyActionWithTokenHandlers.Add(_ => finallyBlock());

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The final execution function (in the finally block).
        /// </summary>
        /// <param name="finallyBlock">The finally block.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Finally(Action<CancellationToken> finallyBlock)
        {
            if (finallyBlock.IsNull())
                finallyBlock.ThrowIfArgNull(nameof(finallyBlock));

            BaseFinallyActionWithTokenHandlers.Add(finallyBlock);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The retry execute function policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Retry(TryRetryPolicy policy)
        {
            if (policy.IsNull())
                policy.ThrowIfArgNull(nameof(policy));

            BaseRetryPolicy = policy;
            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The retry execute function option.
        /// </summary>
        /// <param name="options">Options for controlling the operation.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Retry(TryRetryOptions options)
        {
            if (options.IsNull())
                options.ThrowIfArgNull(nameof(options));

            BaseRetryPolicy = new TryRetryPolicy(options);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback execute function.
        /// </summary>
        /// <param name="fallback">The fallback.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Fallback(Func<T> fallback)
        {
            if (fallback.IsNull())
                fallback.ThrowIfArgNull(nameof(fallback));

            _fallbackSync = fallback;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback execute function with the given token.
        /// </summary>
        /// <param name="fallbackWithToken">The fallback with token.</param>
        /// <returns>
        ///     A TryBuilderSync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderSync<T> Fallback(Func<CancellationToken, T> fallbackWithToken)
        {
            if (fallbackWithToken.IsNull())
                fallbackWithToken.ThrowIfArgNull(nameof(fallbackWithToken));

            _fallbackSyncWithToken = fallbackWithToken;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Execute the given flow.
        /// </summary>
        /// <returns>
        ///     A TryResult&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryResult<T> Execute()
        {
            try
            {
                var resultValue = BaseRetryPolicy.IsNotNull()
                    ? BaseRetryPolicy.Execute(() => _tryBlockWithToken(BaseCancellationToken), BaseCancellationToken)
                    : _tryBlockWithToken(BaseCancellationToken);

                return TryResult<T>.FromValue(resultValue);
            }
            catch (Exception ex) when (ex.IsTypeOfOperationCancel().IsFalse())
            {
                foreach (var handler in BaseCatchHandlers)
                {
                    if (handler.Matches(ex).IsTrue())
                    {
                        handler.Handler(ex);
                        break;
                    }
                }

                try
                {
                    if (_fallbackSyncWithToken.IsNotNull())
                    {
                        var fallback = _fallbackSyncWithToken(BaseCancellationToken);

                        return TryResult<T>.FromValue(fallback);
                    }

                    if (_fallbackSync.IsNotNull())
                    {
                        var fallback = _fallbackSync();

                        return TryResult<T>.FromValue(fallback);
                    }
                }
                catch (Exception fallbackException) when (fallbackException.IsTypeOfOperationCancel().IsFalse())
                {
                    /*
                     *  If fallback throws,
                     *  Treat fallback exception as the result exception!
                     *  (original ex is lost unless you wrap)
                     */
                    return TryResult<T>.FromException(fallbackException);
                }

                return TryResult<T>.FromException(ex);
            }
            finally /*Finally handlers are executed in LIFO order (last added runs first)*/
            {
                if (BaseFinallyActionWithTokenHandlers.Count > 0)
                {
                    for (var i = BaseFinallyActionWithTokenHandlers.Count - 1; i >= 0; i--)
                    {
                        var finallyAction = BaseFinallyActionWithTokenHandlers[i];
                        try
                        {
                            finallyAction(BaseCancellationToken);
                        }
                        catch
                        {
                            /*
                             * ignored
                             * swallow and continue to next finally handler
                             */
                        }
                    }
                }
            }
        }
    }
}