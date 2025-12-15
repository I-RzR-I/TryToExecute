// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 10:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 11:14
// ***********************************************************************
//  <copyright file="ExceptionExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace TryToExecute.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An exception extensions.
    /// </summary>
    /// =================================================================================================
    internal static class ExceptionExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Exception extension method that is type of the given exception.
        /// </summary>
        /// <typeparam name="TException">Type of the exception.</typeparam>
        /// <param name="ex">The ex to act on.</param>
        /// <returns>
        ///     True if type of, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsTypeOf<TException>(this Exception ex) where TException : Exception
            => ex.GetType() == typeof(TException);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An Exception extension method that query if 'ex' is type of operation cancel.
        /// </summary>
        /// <param name="ex">The ex to act on.</param>
        /// <returns>
        ///     True if type of operation cancel, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsTypeOfOperationCancel(this Exception ex)
            => ex is OperationCanceledException;
    }
}