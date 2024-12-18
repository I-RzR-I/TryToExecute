﻿// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-04 17:08
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-04 17:08
// ***********************************************************************
//  <copyright file="TryToExecuteActionFinallyStaticAsync.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Diagnostics;
using System;
using TryToExecute.Extensions;
using TryToExecute.Helpers;
using System.Threading.Tasks;

#if NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Logging;
#endif

// ReSharper disable CheckNamespace
// 
#endregion

namespace TryToExecute.CodeExec
{
    /// <inheritdoc cref="CodeExec.TryCatchExecuteStaticBase"/>
    public partial class TryCatchExecuteStaticBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected static async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            TResult onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));

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
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            TResult onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
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
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<TResult> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

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
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected static async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Exception, TResult> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

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

                return onFailureResult.Invoke(e);
            }
            finally
            {
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
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
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<Exception, TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
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

                return onFailureResult.Invoke(e);
            }
            finally
            {
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Task<TResult>> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

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
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try to execute asynchronous.
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
        protected static async Task<TResult> TryToExecuteAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Exception, Task<TResult>> onFailureResult,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));

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

                return await onFailureResult.Invoke(e);
            }
            finally
            {
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<Task<TResult>> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
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

                return await onFailureResult.Invoke();
            }
            finally
            {
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif

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
        /// <param name="finallyExecAction">The finally execute action.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        protected static async Task<TResult> TryToExecuteAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<Exception, Task<TResult>> onFailureResult,
            ILogger<TLogger> exceptionLogger,
            Action finallyExecAction,
            bool forceCallGarbageCollector = false)
        {
            execFunc.ThrowIfArgNull(nameof(execFunc));
            onFailureResult.ThrowIfArgNull(nameof(onFailureResult));
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

                return await onFailureResult.Invoke(e);
            }
            finally
            {
                if (finallyExecAction.IsNotNull())
                    finallyExecAction?.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif
    }
}