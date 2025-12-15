// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-16 23:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 08:10
// ***********************************************************************
//  <copyright file="TypeExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Reflection;

#endregion

namespace TryToExecute.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A type extensions.
    /// </summary>
    /// =================================================================================================
    internal static class TypeExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A Type extension method that query if 'baseType' is assignable from portable.
        /// </summary>
        /// <param name="baseType">The baseType to act on.</param>
        /// <param name="derivedType">Type of the derived.</param>
        /// <returns>
        ///     True if assignable from portable, false if not.
        /// </returns>
        /// =================================================================================================
        public static bool IsAssignableFromPortable(this Type baseType, Type derivedType)
        {
#if NETSTANDARD1_0_OR_GREATER
            return baseType.GetTypeInfo().IsAssignableFrom(derivedType.GetTypeInfo());
#elif NET
            return baseType.IsAssignableFrom(derivedType);
#endif
        }
    }
}