// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-03 20:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-03 21:29
// ***********************************************************************
//  <copyright file="BoolExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace TryToExecute.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A boolean extensions.
    /// </summary>
    /// =================================================================================================
    internal static class BoolExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A bool extension method that query if 'source' is true.
        /// </summary>
        /// <param name="source">The source to act on.</param>
        /// <remarks>
        ///     SOURCE: https://github.com/I-RzR-I/DomainCommonExtensions/blob/main/src/DomainCommonExtensions/DataTypeExtensions/BoolExtensions.cs
        /// </remarks>
        /// <returns>
        ///     True if true, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsTrue(this bool source) => source.IsNotNull() && source.Equals(true);
    }
}