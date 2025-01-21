// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-01-20 13:43
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-20 21:55
// ***********************************************************************
//  <copyright file="InternalTryCatchExecHelper.cs" company="RzR SOFT & TECH">
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

#if NETSTANDARD2_0_OR_GREATER
using Microsoft.Extensions.Logging;
#endif

#endregion

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An internal try catch execute helper.
    /// </summary>
    /// =================================================================================================
    internal static class InternalTryCatchExecHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static TExecResult TryIt<TExecResult, TExecRequest, TFailureExecRequest>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                return executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref executor);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution asynchronous.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static async Task<TExecResult> TryItAsync<TExecResult, TExecRequest, TFailureExecRequest>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return RequestAnalyzeHelper.CheckIfIsTaskType<TExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TExecRequest>(execRequest)
                    : executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                return RequestAnalyzeHelper.CheckIfIsTaskType<TFailureExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e)
                    : executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref executor);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TFinallyExecRequest">Type of the finally execute request.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="onFinallyRequest">The on finally request.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static TExecResult TryIt<TExecResult, TExecRequest, TFailureExecRequest, TFinallyExecRequest>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            TFinallyExecRequest onFinallyRequest,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                return executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                executor.InternalExecute<TExecResult, TFinallyExecRequest>(onFinallyRequest);

                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref onFinallyRequest, ref executor);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution asynchronous.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TFinallyExecRequest">Type of the finally execute request.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="onFinallyRequest">The on finally request.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static async Task<TExecResult> TryItAsync<TExecResult, TExecRequest, TFailureExecRequest, TFinallyExecRequest>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            TFinallyExecRequest onFinallyRequest,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return RequestAnalyzeHelper.CheckIfIsTaskType<TExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TExecRequest>(execRequest)
                    : executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
            }
            catch (Exception e)
            {
#if DEBUG
                Debug.WriteLine(e);
#if NETSTANDARD1_3_OR_GREATER
                Console.WriteLine(e);
#endif
#endif
                return RequestAnalyzeHelper.CheckIfIsTaskType<TFailureExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e)
                    : executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                if (RequestAnalyzeHelper.CheckIfIsTaskType<TFinallyExecRequest>())
                    await executor.InternalExecuteAsync<TExecResult, TFinallyExecRequest>(onFinallyRequest);
                else
                    executor.InternalExecute<TExecResult, TFinallyExecRequest>(onFinallyRequest);

                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref onFinallyRequest, ref executor);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

#if NETSTANDARD2_0_OR_GREATER

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TLogger">Type of the application logger.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="exceptionLogger">The on application logger.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static TExecResult TryIt<TExecResult, TExecRequest, TFailureExecRequest, TLogger>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            ILogger<TLogger> exceptionLogger,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
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

                return executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref executor, ref exceptionLogger);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution asynchronous.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TLogger">Type of the application logger.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="exceptionLogger">The on application logger.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static async Task<TExecResult> TryItAsync<TExecResult, TExecRequest, TFailureExecRequest, TLogger>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            ILogger<TLogger> exceptionLogger,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return RequestAnalyzeHelper.CheckIfIsTaskType<TExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TExecRequest>(execRequest)
                    : executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
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

                return RequestAnalyzeHelper.CheckIfIsTaskType<TFailureExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e)
                    : executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref executor, ref exceptionLogger);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TFinallyExecRequest">Type of the finally execute request.</typeparam>
        /// <typeparam name="TLogger">Type of the application logger.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="onFinallyRequest">The on finally request.</param>
        /// <param name="exceptionLogger">The on application logger.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static TExecResult TryIt<TExecResult, TExecRequest, TFailureExecRequest, TFinallyExecRequest, TLogger>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            TFinallyExecRequest onFinallyRequest,
            ILogger<TLogger> exceptionLogger,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
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

                return executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                executor.InternalExecute<TExecResult, TFinallyExecRequest>(onFinallyRequest);

                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref onFinallyRequest, ref executor, ref exceptionLogger);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Try request execution asynchronous.
        /// </summary>
        /// <typeparam name="TExecResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecRequest">Type of the execute request.</typeparam>
        /// <typeparam name="TFailureExecRequest">Type of the failure execute request.</typeparam>
        /// <typeparam name="TFinallyExecRequest">Type of the finally execute request.</typeparam>
        /// <typeparam name="TLogger">Type of the application logger.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="onFailureRequest">The on failure request.</param>
        /// <param name="onFinallyRequest">The on finally request.</param>
        /// <param name="exceptionLogger">The on application logger.</param>
        /// <param name="forceCallGarbageCollector">
        ///     (Optional) True to force call garbage collector.
        /// </param>
        /// <returns>
        ///     A TExecResult.
        /// </returns>
        /// =================================================================================================
        internal static async Task<TExecResult> TryItAsync<TExecResult, TExecRequest, TFailureExecRequest, TFinallyExecRequest, TLogger>(
            TExecRequest execRequest,
            TFailureExecRequest onFailureRequest,
            TFinallyExecRequest onFinallyRequest,
            ILogger<TLogger> exceptionLogger,
            bool forceCallGarbageCollector = false)
        {
            var executor = InternalExecutionHelper.Instance;
            try
            {
                return RequestAnalyzeHelper.CheckIfIsTaskType<TExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TExecRequest>(execRequest)
                    : executor.InternalExecute<TExecResult, TExecRequest>(execRequest);
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

                return RequestAnalyzeHelper.CheckIfIsTaskType<TFailureExecRequest>()
                    ? await executor.InternalExecuteAsync<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e)
                    : executor.InternalExecute<TExecResult, TFailureExecRequest, Exception>(onFailureRequest, e);
            }
            finally
            {
                if (RequestAnalyzeHelper.CheckIfIsTaskType<TFinallyExecRequest>())
                    await executor.InternalExecuteAsync<TExecResult, TFinallyExecRequest>(onFinallyRequest);
                else
                    executor.InternalExecute<TExecResult, TFinallyExecRequest>(onFinallyRequest);

                RefTypeParamHelper.ToDefaultValue(ref execRequest, ref onFailureRequest, ref onFinallyRequest, ref executor, ref exceptionLogger);

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif
    }
}