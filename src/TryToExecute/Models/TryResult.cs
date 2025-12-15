// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-16 19:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 08:09
// ***********************************************************************
//  <copyright file="TryResult.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using TryToExecute.Extensions;

// ReSharper disable MemberCanBePrivate.Global

#endregion

namespace TryToExecute.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Encapsulates the result of a try. This class cannot be inherited.
    /// </summary>
    /// <typeparam name="T">Generic type parameter.</typeparam>
    /// =================================================================================================
    public sealed class TryResult<T>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets a value indicating whether the succeeded.
        /// </summary>
        /// <value>
        ///     True if succeeded, false if not.
        /// </value>
        /// =================================================================================================
        public bool Succeeded { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the value.
        /// </summary>
        /// <value>
        ///     The value.
        /// </value>
        /// =================================================================================================
        public T Value { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the exception.
        /// </summary>
        /// <value>
        ///     The exception.
        /// </value>
        /// =================================================================================================
        public Exception Exception { get; private set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryResult{T}"/> class.
        /// </summary>
        /// <param name="value">The value.</param>
        /// =================================================================================================
        internal TryResult(T value)
        {
            Succeeded = true;
            Value = value;
            Exception = null;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryResult{T}"/> class.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when one or more required arguments are null.
        /// </exception>
        /// <param name="ex">The exception.</param>
        /// =================================================================================================
        internal TryResult(Exception ex)
        {
            ex.ThrowIfArgNull(nameof(ex));

            Succeeded = false;
            Value = default;
            Exception = ex;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new object from the given value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>
        ///     A TryResult&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static TryResult<T> FromValue(T value) => new TryResult<T>(value);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Creates a new object from the given exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        ///     A TryResult&lt;T&gt;
        /// </returns>
        /// =================================================================================================
        public static TryResult<T> FromException(Exception ex) => new TryResult<T>(ex);
    }
}