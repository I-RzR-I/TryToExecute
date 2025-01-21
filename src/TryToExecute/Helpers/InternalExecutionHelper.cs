// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-01-14 23:14
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-20 21:51
// ***********************************************************************
//  <copyright file="InternalExecutionHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading.Tasks;
using TryToExecute.Extensions;

// ReSharper disable MemberCanBeMadeStatic.Global
// ReSharper disable RedundantCast

#endregion

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An internal execution helper.
    /// </summary>
    /// =================================================================================================
    internal class InternalExecutionHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the instance.
        /// </summary>
        /// =================================================================================================
        public static readonly InternalExecutionHelper Instance = new InternalExecutionHelper();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Internal execute.
        /// </summary>
        /// <typeparam name="TExecuteResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecute">Type of the execute.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="exception">(Optional) The exception.</param>
        /// <returns>
        ///     A TExecuteResult.
        /// </returns>
        /// =================================================================================================
        internal TExecuteResult InternalExecute<TExecuteResult, TExecute>(TExecute execRequest, Exception exception = null)
        {
            var defaultTResult = (TExecuteResult)default;
            if (execRequest.IsNotNull())
            {
                if (execRequest.GetType() == typeof(Action))
                {
                    (execRequest as Action)?.Invoke();

                    return defaultTResult;
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<TExecute>))
                {
                    (execRequest as Func<TExecute>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<TExecuteResult>))
                {
                    return (execRequest as Func<TExecuteResult>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<Exception, TExecute>) || typeof(TExecute) == typeof(Func<Exception, TExecuteResult>))
                {
                    return (execRequest as Func<Exception, TExecuteResult>)!.Invoke(exception);
                }

                return CastToResult<TExecuteResult, TExecute>(execRequest);
            }

            return defaultTResult;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Internal execute.
        /// </summary>
        /// <typeparam name="TExecuteResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecute">Type of the execute.</typeparam>
        /// <typeparam name="TParam1">Type of the parameter 1.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="param">The parameter.</param>
        /// <returns>
        ///     A TExecuteResult.
        /// </returns>
        /// =================================================================================================
        internal TExecuteResult InternalExecute<TExecuteResult, TExecute, TParam1>(TExecute execRequest, TParam1 param)
        {
            var defaultTResult = (TExecuteResult)default;
            if (execRequest.IsNotNull())
            {
                if (execRequest.GetType() == typeof(Action))
                {
                    (execRequest as Action)?.Invoke();

                    return defaultTResult;
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<TExecute>))
                {
                    (execRequest as Func<TExecute>)?.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<TExecuteResult>))
                {
                    return (execRequest as Func<TExecuteResult>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<TParam1, TExecute>) || typeof(TExecute) == typeof(Func<TParam1, TExecuteResult>))
                {
                    return (execRequest as Func<TParam1, TExecuteResult>)!.Invoke(param);
                }

                return CastToResult<TExecuteResult, TExecute>(execRequest);
            }

            return defaultTResult;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Internal execute asynchronous.
        /// </summary>
        /// <typeparam name="TExecuteResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecute">Type of the execute.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="exception">(Optional) The exception.</param>
        /// <returns>
        ///     A TExecuteResult.
        /// </returns>
        /// =================================================================================================
        internal async Task<TExecuteResult> InternalExecuteAsync<TExecuteResult, TExecute>(TExecute execRequest, Exception exception = null)
        {
            var defaultTResult = (TExecuteResult)default;
            if (execRequest.IsNotNull())
            {
                if (execRequest.GetType() == typeof(Action))
                {
                    (execRequest as Action)?.Invoke();
                }
                else if (execRequest.GetType() == typeof(Task))
                {
                    await (execRequest as Task)!;
                }
                else if (execRequest.GetType() == typeof(Func<Task>))
                {
                    await (execRequest as Func<Task>)!.Invoke();

                    return await Task<TExecuteResult>.Factory.StartNew(() => defaultTResult);
                }
                else if (typeof(TExecute) == typeof(Func<Task<TExecute>>))
                {
                    await (execRequest as Func<Task<TExecute>>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<Task<TExecuteResult>>))
                {
                    return await (execRequest as Func<Task<TExecuteResult>>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<Exception, Task<TExecute>>) || typeof(TExecute) == typeof(Func<Exception, Task<TExecuteResult>>))
                {
                    return await (execRequest as Func<Exception, Task<TExecuteResult>>)!.Invoke(exception);
                }

                return CastToResult<TExecuteResult, TExecute>(execRequest);
            }

            return defaultTResult;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Internal execute asynchronous.
        /// </summary>
        /// <typeparam name="TExecuteResult">Type of the execute result.</typeparam>
        /// <typeparam name="TExecute">Type of the execute.</typeparam>
        /// <typeparam name="TParam1">Type of the parameter 1.</typeparam>
        /// <param name="execRequest">The execute request.</param>
        /// <param name="param1">The first parameter.</param>
        /// <returns>
        ///     A TExecuteResult.
        /// </returns>
        /// =================================================================================================
        internal async Task<TExecuteResult> InternalExecuteAsync<TExecuteResult, TExecute, TParam1>(TExecute execRequest, TParam1 param1)
        {
            var defaultTResult = (TExecuteResult)default;
            if (execRequest.IsNotNull())
            {
                if (execRequest.GetType() == typeof(Action))
                {
                    (execRequest as Action)?.Invoke();
                }
                else if (execRequest.GetType() == typeof(Task))
                {
                    await (execRequest as Task)!;
                }
                else if (execRequest.GetType() == typeof(Func<Task>))
                {
                    await (execRequest as Func<Task>)!.Invoke();

                    return await Task<TExecuteResult>.Factory.StartNew(() => defaultTResult);
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<Task<TExecute>>))
                {
                    await (execRequest as Func<Task<TExecute>>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<>) || typeof(TExecute) == typeof(Func<Task<TExecuteResult>>))
                {
                    return await (execRequest as Func<Task<TExecuteResult>>)!.Invoke();
                }
                else if (typeof(TExecute) == typeof(Func<TParam1, Task<TExecute>>) || typeof(TExecute) == typeof(Func<TParam1, Task<TExecuteResult>>))
                {
                    return await (execRequest as Func<TParam1, Task<TExecuteResult>>)!.Invoke(param1);
                }

                return CastToResult<TExecuteResult, TExecute>(execRequest);
            }

            return defaultTResult;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts an item to a result.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     A TResult.
        /// </returns>
        /// =================================================================================================
        private static TResult CastToResult<TResult, TRequest>(TRequest item)
        {
            if (item.IsNotNull())
            {
                if (typeof(TRequest) == typeof(TResult))
                    return item is TResult result ? result : default;

                return (TResult)Convert.ChangeType(item, typeof(TResult));
            }

            return (TResult)default;
        }
    }
}