// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 00:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 08:09
// ***********************************************************************
//  <copyright file="TimeSpanExtensions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Diagnostics;

#endregion

namespace TryToExecute.Extensions
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A time span extensions.
    /// </summary>
    /// =================================================================================================
    internal static class TimeSpanExtensions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that internal sleep.
        /// </summary>
        /// <param name="timeSpan">The timeSpan to act on.</param>
        /// =================================================================================================
        internal static void InternalSleep(this TimeSpan timeSpan)
        {
            if (timeSpan.TotalMilliseconds <= 0)
                return;

            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeSpan.TotalMilliseconds)
            {
                // Busy-wait
            }
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that query if 'timeSpan' is less or zero.
        /// </summary>
        /// <param name="timeSpan">The timeSpan to act on.</param>
        /// <returns>
        ///     True if less or zero, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsLessOrZero(this TimeSpan timeSpan) 
            => timeSpan <= TimeSpan.Zero;
    }
}