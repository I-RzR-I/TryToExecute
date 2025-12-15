// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-16 23:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 08:11
// ***********************************************************************
//  <copyright file="ExceptionHandler.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading.Tasks;

#endregion

namespace TryToExecute.Models.Internal
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     An exception handler. This class cannot be inherited.
    /// </summary>
    /// =================================================================================================
    public sealed class ExceptionHandler
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the type of the exception.
        /// </summary>
        /// <value>
        ///     The type of the exception.
        /// </value>
        /// =================================================================================================
        public Type ExceptionType { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets the handler.
        /// </summary>
        /// <value>
        ///     A function delegate that yields a Task.
        /// </value>
        /// =================================================================================================
        public Func<Exception, Task> Handler { get; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExceptionHandler"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="handler">The handler.</param>
        /// =================================================================================================
        public ExceptionHandler(Type type, Func<Exception, Task> handler)
        {
            ExceptionType = type;
            Handler = handler;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Matches the given exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        /// <returns>
        ///     True if it succeeds, false if it fails.
        /// </returns>
        /// =================================================================================================
        public bool Matches(Exception ex)
            => ExceptionType == ex.GetType();
    }
}