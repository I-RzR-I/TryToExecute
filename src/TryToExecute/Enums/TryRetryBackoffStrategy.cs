// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 09:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 09:24
// ***********************************************************************
//  <copyright file="TryRetryBackoffStrategy.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace TryToExecute.Enums
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Values that represent try retry backoff strategies.
    /// </summary>
    /// =================================================================================================
    public enum TryRetryBackoffStrategy
    {
        /// <summary>
        ///     An enum constant representing the fixed option.
        /// </summary>
        Fixed,

        /// <summary>
        ///     An enum constant representing the exponential option.
        /// </summary>
        Exponential
    }
}