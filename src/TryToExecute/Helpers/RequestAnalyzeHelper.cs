// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-01-19 20:23
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-21 15:05
// ***********************************************************************
//  <copyright file="RequestAnalyzeHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Reflection;
using System.Threading.Tasks;
using TryToExecute.Extensions;

#endregion

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A request analyze helper.
    /// </summary>
    /// =================================================================================================
    internal static class RequestAnalyzeHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determine if is task type.
        /// </summary>
        /// <typeparam name="TResult">Type of the result.</typeparam>
        /// <typeparam name="TRequest">Type of the request.</typeparam>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        internal static bool CheckIfIsTaskType<TResult, TRequest>()
            => CheckIfIsTaskType<TRequest>() || CheckIfIsTaskType<TResult>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Determine if is task type.
        /// </summary>
        /// <typeparam name="TTaskParam">Type of the task parameter.</typeparam>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        internal static bool CheckIfIsTaskType<TTaskParam>()
        {
            var type = typeof(TTaskParam);

            if (typeof(Task).IsAssignableFromPortable(type))
                return true;

            if (typeof(Delegate).IsAssignableFromPortable(type))
            {
                var invoke = type.GetTypeInfo().GetDeclaredMethod("Invoke");
                if (invoke.IsNotNull() && typeof(Task).IsAssignableFromPortable(invoke.ReturnType))
                    return true;
            }

            return false;
        }
    }
}