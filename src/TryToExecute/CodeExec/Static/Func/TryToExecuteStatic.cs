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
using System.Diagnostics;
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
        {
            try
            {
                return execRequest;
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
                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }

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

            try
            {
                return execRequest;
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
                finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
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

            try
            {
                return execFunc.Invoke();
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
                finallyExecFunc.Invoke();

                if (forceCallGarbageCollector.IsTrue())
                    TryToExecuteAppHelper.ForceCallGC();
            }
        }
#endif
    }
}