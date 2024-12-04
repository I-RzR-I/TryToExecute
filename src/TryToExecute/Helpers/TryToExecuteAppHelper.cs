// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-03 21:01
// 
//  Last Modified By : RzR
//  Last Modified On : 2024-12-03 21:25
// ***********************************************************************
//  <copyright file="TryToExecuteAppHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

// ReSharper disable InconsistentNaming

#endregion

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A try to execute application helper.
    /// </summary>
    /// =================================================================================================
    internal static class TryToExecuteAppHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Force call Garbage Collection (GC).
        /// </summary>
        /// =================================================================================================
        internal static void ForceCallGC()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}