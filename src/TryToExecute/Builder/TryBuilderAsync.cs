// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 00:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 16:36
// ***********************************************************************
//  <copyright file="TryBuilderAsync.cs" company="RzR SOFT & TECH">
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
    ///     A try builder asynchronous. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// <seealso cref="T:TryToExecute.Builder.TryBuilderBase"/>
    /// =================================================================================================
    public class TryBuilderAsync<T> : TryBuilderBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the try block with token.
        /// </summary>
        /// =================================================================================================
        private readonly Func<CancellationToken, Task<T>> _tryBlockWithToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback asynchronous.
        /// </summary>
        /// =================================================================================================
        private Func<Task<T>> _fallbackAsync;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback asynchronous with token.
        /// </summary>
        /// =================================================================================================
        private Func<CancellationToken, Task<T>> _fallbackAsyncWithToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback synchronize.
        /// </summary>
        /// =================================================================================================
        private Func<T> _fallbackSync;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderAsync{T}"/> class.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// =================================================================================================
        public TryBuilderAsync(Func<Task<T>> tryBlock) : this(_ => tryBlock(), CancellationToken.None) { }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderAsync{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="tryBlockWithToken">(Immutable) the try block with token.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// =================================================================================================
        public TryBuilderAsync(Func<CancellationToken, Task<T>> tryBlockWithToken, CancellationToken cancellationToken = default)
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
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Catch<TException>(Func<TException, Task> handler) 
            where TException : Exception
        {
            if (handler.IsNull())
                handler.ThrowIfArgNull(nameof(handler));

            BaseCatchHandlers.Add(new ExceptionHandler(typeof(TException), ex => handler((TException)ex)));

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Catches the given handler (Exception catch handler).
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Catch<TException>(Func<TException, CancellationToken, Task> handler) 
            where TException : Exception
        {
            if (handler.IsNull())
                handler.ThrowIfArgNull(nameof(handler));

            BaseCatchHandlers.Add(new ExceptionHandler(typeof(TException), ex => handler((TException)ex, BaseCancellationToken)));

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Catches the given handler (Exception catch handler).
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="handler">The handler.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Catch<TException>(Action<TException> handler) 
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
        ///     The final execution function (in the finally block).
        /// </summary>
        /// <param name="finallyBlock">The finally block.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Finally(Func<Task> finallyBlock)
        {
            if (finallyBlock.IsNull())
                finallyBlock.ThrowIfArgNull(nameof(finallyBlock));

            BaseFinallyFunctionWithTokenHandlers.Add(_ => finallyBlock());

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The final execution function (in the finally block).
        /// </summary>
        /// <param name="finallyBlock">The finally block.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Finally(Func<CancellationToken, Task> finallyBlock)
        {
            if (finallyBlock.IsNull())
                finallyBlock.ThrowIfArgNull(nameof(finallyBlock));

            BaseFinallyFunctionWithTokenHandlers.Add(finallyBlock);

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The final execution function (in the finally block).
        /// </summary>
        /// <param name="finallyBlock">The finally block.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Finally(Action finallyBlock)
        {
            if (finallyBlock.IsNull())
                finallyBlock.ThrowIfArgNull(nameof(finallyBlock));

            BaseFinallyFunctionWithTokenHandlers.Add(_ =>
            {
                finallyBlock();

                return Task.FromResult<object>(null);
            });

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The retry execute function policy.
        /// </summary>
        /// <param name="policy">The policy.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Retry(TryRetryPolicy policy)
        {
            if (policy.IsNull())
                policy.ThrowIfArgNull(nameof(policy));

            BaseRetryPolicy = policy;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The retry execute function options.
        /// </summary>
        /// <param name="options">Options for controlling the operation.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Retry(TryRetryOptions options)
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
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Fallback(Func<T> fallback)
        {
            if (fallback.IsNull())
                fallback.ThrowIfArgNull(nameof(fallback));

            _fallbackSync = fallback;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback execute function.
        /// </summary>
        /// <param name="fallbackAsync">The fallback asynchronous.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Fallback(Func<Task<T>> fallbackAsync)
        {
            if (fallbackAsync.IsNull())
                fallbackAsync.ThrowIfArgNull(nameof(fallbackAsync));

            _fallbackAsync = fallbackAsync;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The fallback execute function.
        /// </summary>
        /// <param name="fallbackAsyncWithToken">The fallback asynchronous with token.</param>
        /// <returns>
        ///     A TryBuilderAsync&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public TryBuilderAsync<T> Fallback(Func<CancellationToken, Task<T>> fallbackAsyncWithToken)
        {
            if (fallbackAsyncWithToken.IsNull())
                fallbackAsyncWithToken.ThrowIfArgNull(nameof(fallbackAsyncWithToken));

            _fallbackAsyncWithToken = fallbackAsyncWithToken;

            return this;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Execute the given flow asynchronous.
        /// </summary>
        /// <returns>
        ///     The execution result, task TryResult&lt;T&gt;.
        /// </returns>
        /// =================================================================================================
        public async Task<TryResult<T>> ExecuteAsync()
        {
            try
            {
                var value = BaseRetryPolicy.IsNotNull()
                    ? await BaseRetryPolicy.ExecuteAsync(_tryBlockWithToken, BaseCancellationToken)
                    : await _tryBlockWithToken(BaseCancellationToken);

                return TryResult<T>.FromValue(value);
            }
            catch (Exception ex)
            {
                foreach (var exceptionHandler in BaseCatchHandlers)
                {
                    if (exceptionHandler.Matches(ex).IsTrue())
                    {
                        await exceptionHandler.Handler(ex);
                        break;
                    }
                }

                try
                {
                    if (_fallbackAsyncWithToken.IsNotNull())
                    {
                        var fallback = await _fallbackAsyncWithToken(BaseCancellationToken);

                        return TryResult<T>.FromValue(fallback);
                    }

                    if (_fallbackAsync.IsNotNull())
                    {
                        var fallback = await _fallbackAsync();

                        return TryResult<T>.FromValue(fallback);
                    }

                    if (_fallbackSync.IsNotNull())
                    {
                        var fallback = _fallbackSync();

                        return TryResult<T>.FromValue(fallback);
                    }
                }
                catch (Exception fallbackException)
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
            finally/*Finally handlers are executed in LIFO order (last added runs first)*/
            {
                if (BaseFinallyFunctionWithTokenHandlers.Count > 0)
                {
                    for (var i = BaseFinallyFunctionWithTokenHandlers.Count - 1; i >= 0; i--)
                    {
                        var handler = BaseFinallyFunctionWithTokenHandlers[i];
                        try
                        {
                            await handler(BaseCancellationToken);
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