// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2024-12-04 20:49
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-20 15:43
// ***********************************************************************
//  <copyright file="TryCatchExecuteStaticBase.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using TryToExecute.Helpers;

#endregion

namespace TryToExecute.CodeExec
{
    /// -------------------------------------------------------------------------------------------------
    /// <content>
    ///     A try catch execute static base.
    /// </content>
    /// =================================================================================================
    public partial class TryCatchExecuteStaticBase
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets to default if present.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <param name="item">[in,out] The item.</param>
        /// =================================================================================================
        protected static void SetToDefaultIfPresent<T1>(ref T1 item)
            => RefTypeParamHelper.ToDefaultValue(ref item);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets to default if present.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// =================================================================================================
        protected static void SetToDefaultIfPresent<T1, T2>(ref T1 item1, ref T2 item2)
            => RefTypeParamHelper.ToDefaultValue(ref item1, ref item2);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets to default if present.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// <param name="item3">[in,out] The third item.</param>
        /// =================================================================================================
        protected static void SetToDefaultIfPresent<T1, T2, T3>(ref T1 item1, ref T2 item2, ref T3 item3)
            => RefTypeParamHelper.ToDefaultValue(ref item1, ref item2, ref item3);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets to default if present.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <typeparam name="T4">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// <param name="item3">[in,out] The third item.</param>
        /// <param name="item4">[in,out] The fourth item.</param>
        /// =================================================================================================
        protected static void SetToDefaultIfPresent<T1, T2, T3, T4>(ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4)
            => RefTypeParamHelper.ToDefaultValue(ref item1, ref item2, ref item3, ref item4);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Sets to default if present.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <typeparam name="T4">Generic type parameter.</typeparam>
        /// <typeparam name="T5">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// <param name="item3">[in,out] The third item.</param>
        /// <param name="item4">[in,out] The fourth item.</param>
        /// <param name="item5">[in,out] The fourth item.</param>
        /// =================================================================================================
        protected static void SetToDefaultIfPresent<T1, T2, T3, T4, T5>(ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5)
            => RefTypeParamHelper.ToDefaultValue(ref item1, ref item2, ref item3, ref item4, ref item5);
    }
}