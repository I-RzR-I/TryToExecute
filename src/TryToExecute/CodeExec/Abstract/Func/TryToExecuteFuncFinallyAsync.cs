﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-05 19:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-05 19:05
// ***********************************************************************
//  <copyright file="TryToExecuteFuncFinallyAsync.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using TryToExecute.Extensions;
using TryToExecute.Helpers;

#if NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Logging;
#endif

// ReSharper disable CheckNamespace

#endregion

namespace TryToExecute.CodeExec
{
    /// <inheritdoc cref="CodeExec.TryCatchExecuteBase"/>
    public abstract partial class TryCatchExecuteBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            TResult onFailureResult,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif

                return onFailureResult;
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            TResult onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                exceptionLogger.LogError(e, DefaultMessageHelper.InternalErrorOnTryExecute);

                return onFailureResult;
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<TResult> onFailureResult,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif

                return onFailureResult.Invoke();
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                exceptionLogger.LogError(e, DefaultMessageHelper.InternalErrorOnTryExecute);

                return onFailureResult.Invoke();
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Task<TResult>> onFailureResult,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif

                return await onFailureResult.Invoke();
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

#if NETSTANDARD2_0_OR_GREATER
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected virtual async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<Task<TResult>> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Func<Task<TResult>> finallyExecFunc,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
            exceptionLogger.ThrowIfArgNull(nameof(exceptionLogger));
            finallyExecFunc.ThrowIfArgNull(nameof(finallyExecFunc));

            try
            {
                return await execFunc.Invoke();
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                exceptionLogger.LogError(e, DefaultMessageHelper.InternalErrorOnTryExecute);

                return await onFailureResult.Invoke();
            }
            finally
            {
                await finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif
    }
}