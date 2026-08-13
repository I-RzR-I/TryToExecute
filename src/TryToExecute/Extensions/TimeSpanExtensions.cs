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
using System.Threading;

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

#if NETSTANDARD1_0
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A TimeSpan extension method that internal sleep, cancellable through the given
        ///     <paramref name="cancellationToken"/>.
        /// </summary>
        /// <exception cref="OperationCanceledException">
        ///     Thrown when cancellation is requested through <paramref name="cancellationToken"/>.
        /// </exception>
        /// <param name="timeSpan">The timeSpan to act on.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// =================================================================================================
        internal static void InternalSleep(this TimeSpan timeSpan, CancellationToken cancellationToken)
        {
            if (timeSpan.TotalMilliseconds <= 0)
                return;

            var sw = Stopwatch.StartNew();
            while (sw.ElapsedMilliseconds < timeSpan.TotalMilliseconds)
            {
                if (cancellationToken.IsCancellationRequested)
                    break;
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
#endif

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