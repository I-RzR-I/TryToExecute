// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-03 20:33
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-03 21:21
// ***********************************************************************
//  <copyright file="TryToExecuteStatic.cs" company="RzR SOFT & TECH">
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
            bool forceCallGarbageCollector = false)
            => InternalTryCatchExecHelper.TryIt<TResult, TResult, TResult>(
                execRequest, onFailureResult, forceCallGarbageCollector);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, TResult, TResult, Func<TResult>>(
                execRequest, onFailureResult, finallyExecFunc, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult>(
                execFunc, onFailureResult, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult, Func<TResult>>(
                execFunc, onFailureResult, finallyExecFunc, forceCallGarbageCollector);
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult, TLogger>(
                execFunc, onFailureResult, exceptionLogger, forceCallGarbageCollector);
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
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, TResult, Func<TResult>, TLogger>(
                execFunc, onFailureResult, finallyExecFunc, exceptionLogger, forceCallGarbageCollector);
        }
#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>>(
                execFunc, onFailureResult, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>>(
                execFunc, onFailureResult, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>, Func<TResult>>(
                execFunc, onFailureResult, finallyExecFunc, forceCallGarbageCollector);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <param name="execFunc">The execute function.</param>
        /// <param name="onFailureResult">The on failure result.</param>
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>, Func<TResult>>(
                execFunc, onFailureResult, finallyExecFunc, forceCallGarbageCollector);
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>, TLogger>(
                execFunc, onFailureResult, exceptionLogger, forceCallGarbageCollector);
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
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>, TLogger>(
                execFunc, onFailureResult, exceptionLogger, forceCallGarbageCollector);
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
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<TResult>, Func<TResult>, TLogger>(
                execFunc, onFailureResult, finallyExecFunc, exceptionLogger, forceCallGarbageCollector);
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
        /// <param name="finallyExecFunc">The finally execute function.</param>
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
            Func<TResult> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            return InternalTryCatchExecHelper.TryIt<TResult, Func<TResult>, Func<Exception, TResult>, Func<TResult>, TLogger>(
                execFunc, onFailureResult, finallyExecFunc, exceptionLogger, forceCallGarbageCollector);
        }
#endif
    }
}