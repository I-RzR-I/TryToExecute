// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-04 15:06
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-04 15:06
// ***********************************************************************
//  <copyright file="ActionTryToExecuteStatic.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using TryToExecute.Extensions;
using TryToExecute.Helpers;

#if NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Logging;
#endif

// ReSharper disable CheckNamespace

#endregion

namespace TryToExecute.CodeExec
{
    /// <inheritdoc cref="CodeExec.TryCatchExecuteStaticBase"/>
    public partial class TryCatchExecuteStaticBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult>(
            TResult execRequest,
            TResult onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
            => InternalTryCatchExecHelper.TryIt<TResult, TResult, TResult, Action>(
                execRequest, onFailureResult, finallyExecAction, forceCallGarbageCollector);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult>(
            Func<TResult> execFunc,
            TResult onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult, Action>(
                execFunc, onFailureResult, finallyExecAction, forceCallGarbageCollector);
        }

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TLogger">Type of the logger.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="exceptionLogger">The exception logger.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult, TLogger>(
            Func<TResult> execFunc,
            TResult onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult, Action, TLogger>(
                execFunc, onFailureResult, finallyExecAction, exceptionLogger, forceCallGarbageCollector);
        }
#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult>(
            Func<TResult> execFunc,
            Func<TResult> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>, Action>(
                execFunc, onFailureResult, finallyExecAction, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult>(
            Func<TResult> execFunc,
            Func<Exception, TResult> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            
            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>, Action>(
                execFunc, onFailureResult, finallyExecAction, forceCallGarbageCollector);
        }

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TLogger">Type of the logger.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="exceptionLogger">The exception logger.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult, TLogger>(
            Func<TResult> execFunc,
            Func<TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>, Action, TLogger>(
                execFunc, onFailureResult, finallyExecAction, exceptionLogger, forceCallGarbageCollector);
        }
#endif

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TLogger">Type of the logger.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="exceptionLogger">The exception logger.</param>
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static TResult TryToExecute<TResult, TLogger>(
            Func<TResult> execFunc,
            Func<Exception, TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>, Action, TLogger>(
                execFunc, onFailureResult, finallyExecAction, exceptionLogger, forceCallGarbageCollector);
        }
#endif
    }
}