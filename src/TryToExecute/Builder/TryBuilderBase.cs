// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 11:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 12:15
// ***********************************************************************
//  <copyright file="TryBuilderBase.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TryToExecute.CodeExec;
using TryToExecute.Models;
using TryToExecute.Models.Internal;

// ReSharper disable ArrangeObjectCreationWhenTypeEvident

#endregion

namespace TryToExecute.Builder
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A try builder base props and functionalities.
    /// </summary>
    /// <seealso cref="T:TryToExecute.CodeExec.TryCatchExecuteBase"/>
    /// =================================================================================================
    public abstract class TryBuilderBase : TryCatchExecuteBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A token that allows processing to be cancelled.
        /// </summary>
        /// =================================================================================================
        protected CancellationToken BaseCancellationToken;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The base retry policy.
        /// </summary>
        /// =================================================================================================
        protected TryRetryPolicy BaseRetryPolicy;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The base catch handlers.
        /// </summary>
        /// =================================================================================================
        protected readonly List<ExceptionHandler> BaseCatchHandlers = new List<ExceptionHandler>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The base finally action with token handlers.
        /// </summary>
        /// =================================================================================================
        protected readonly List<Action<CancellationToken>> BaseFinallyActionWithTokenHandlers = new List<Action<CancellationToken>>();

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     The base finally function with token handlers.
        /// </summary>
        /// =================================================================================================
        protected readonly List<Func<CancellationToken, Task>> BaseFinallyFunctionWithTokenHandlers = new List<Func<CancellationToken, Task>>();
    }
}