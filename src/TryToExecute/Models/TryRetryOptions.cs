// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-17 00:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-17 09:28
// ***********************************************************************
//  <copyright file="TryRetryOptions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using TryToExecute.Enums;
using TryToExecute.Extensions;

#endregion

namespace TryToExecute.Models
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A try retry options. This class cannot be inherited.
    /// </summary>
    /// =================================================================================================
    public sealed class TryRetryOptions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the maximum attempts.
        /// </summary>
        /// <value>
        ///     The maximum attempts.
        ///     Total attempts (including first).
        /// </value>
        /// =================================================================================================
        public int MaxAttempts { get; set; } = 3;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the base delay.
        /// </summary>
        /// <value>
        ///     The base delay. Default value is 200 milliseconds.
        /// </value>
        /// =================================================================================================
        public TimeSpan BaseDelay { get; set; } = TimeSpan.FromMilliseconds(200);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the factor.
        /// </summary>
        /// <value>
        ///     The factor.
        ///     Exponential factor (<see cref="TryRetryBackoffStrategy.Exponential"/>).
        /// </value>
        /// =================================================================================================
        public double Factor { get; set; } = 2.0;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets a value indicating whether this object use jitter.
        /// </summary>
        /// <value>
        ///     True if it uses jitter, false if not.
        /// </value>
        /// =================================================================================================
        public bool UseJitter { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Gets or sets the strategy.
        /// </summary>
        /// <value>
        ///     The strategy.
        ///     Default value <see cref="TryRetryBackoffStrategy.Exponential"/>.
        /// </value>
        /// =================================================================================================
        public TryRetryBackoffStrategy Strategy { get; set; } = TryRetryBackoffStrategy.Exponential;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Predicate that decides whether an exception is transient and should be retried. 
        ///     Default: retry on everything except OperationCanceledException.
        /// </summary>
        /// <value>
        ///     The func should retry on.
        /// </value>
        /// =================================================================================================
        public Func<Exception, bool> ShouldRetryOn { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional callback invoked on each retry attempt (attempt index, exception). 
        ///     Attempt is 1 - based (1 = first attempt that failed, i.e. before second try).
        /// </summary>
        /// <value>
        ///     The on retry.
        /// </value>
        /// =================================================================================================
        public Action<int, Exception> OnRetry { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     A provider that returns a random double in [0,1). 
        ///     Provided so tests can inject deterministic values. Default uses System.Random.
        /// </summary>
        /// <value>
        ///     The random provider.
        /// </value>
        /// =================================================================================================
        public Func<double> RandomProvider { get; set; }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryRetryOptions"/> class.
        /// </summary>
        /// =================================================================================================
        public TryRetryOptions()
        {
            ShouldRetryOn = ex => ex.IsTypeOfOperationCancel().IsFalse();
            RandomProvider = () => new Random().NextDouble();
        }
    }
}