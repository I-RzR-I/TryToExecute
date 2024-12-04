// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-03 20:34
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-03 21:29
// ***********************************************************************
//  <copyright file="ObjectExtensions.cs" company="RzR SOFT & TECH">
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
    ///     An object extensions.
    ///     SOURCE: https://github.com/I-RzR-I/DomainCommonExtensions/blob/main/src/DomainCommonExtensions/DataTypeExtensions/ObjectExtensions.cs
    ///             https://github.com/I-RzR-I/DomainCommonExtensions/blob/main/src/DomainCommonExtensions/CommonExtensions/NullExtensions.cs
    /// </summary>
    /// =================================================================================================
    internal static class ObjectExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that query if 'obj' is null.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <returns>
        ///     True if null, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsNull(this object obj) => obj == null;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that query if 'obj' is not null.
        /// </summary>
        /// <param name="obj">The obj to act on.</param>
        /// <returns>
        ///     True if not null, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsNotNull(this object obj) => obj != null;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     An object extension method that throw if argument null.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="source">The source to act on.</param>
        /// <param name="objectName">Name of the object.</param>
        /// =================================================================================================
        internal static void ThrowIfArgNull(this object source, string objectName)
        {
            if (source.IsNull()) throw new ArgumentNullException(objectName);
        }
    }
}