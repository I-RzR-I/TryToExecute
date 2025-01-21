// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-03 20:58
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-03 20:58
// ***********************************************************************
//  <copyright file="DefaultMessageHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

// ReSharper disable ClassNeverInstantiated.Global

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A default message helper.
    /// </summary>
    /// =================================================================================================
    internal class DefaultMessageHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) the internal error on try execute.
        /// </summary>
        /// =================================================================================================
        internal const string InternalErrorOnTryExecute = "An internal error occurred while trying to execute generic try/catch!";
    }
}