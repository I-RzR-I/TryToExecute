// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-01-13 23:53
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-14 21:49
// ***********************************************************************
//  <copyright file="RefTypeParamHelper.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using TryToExecute.Extensions;

#endregion

namespace TryToExecute.Helpers
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     A reference type parameter helper.
    /// </summary>
    /// =================================================================================================
    internal static class RefTypeParamHelper
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     If is present to default.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <param name="item">The item.</param>
        /// <returns>
        ///     A T1.
        /// </returns>
        /// =================================================================================================
        internal static T1 IfIsPresentToDefault<T1>(T1 item)
        {
            if (item.IsNotNull())
                item = default(T1);

            return item;
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a default value.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// =================================================================================================
        internal static void ToDefaultValue<T1>(ref T1 item1)
        {
            if (item1.IsNotNull())
                item1 = default(T1);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a default value.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// =================================================================================================
        internal static void ToDefaultValue<T1, T2>(ref T1 item1, ref T2 item2)
        {
            ToDefaultValue(ref item1);
            ToDefaultValue(ref item2);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a default value.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// <param name="item3">[in,out] The third item.</param>
        /// =================================================================================================
        internal static void ToDefaultValue<T1, T2, T3>(ref T1 item1, ref T2 item2, ref T3 item3)
        {
            ToDefaultValue(ref item1);
            ToDefaultValue(ref item2);
            ToDefaultValue(ref item3);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a default value.
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
        internal static void ToDefaultValue<T1, T2, T3, T4>(ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4)
        {
            ToDefaultValue(ref item1);
            ToDefaultValue(ref item2);
            ToDefaultValue(ref item3);
            ToDefaultValue(ref item4);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Converts this object to a default value.
        /// </summary>
        /// <typeparam name="T1">Generic type parameter.</typeparam>
        /// <typeparam name="T2">Generic type parameter.</typeparam>
        /// <typeparam name="T3">Generic type parameter.</typeparam>
        /// <typeparam name="T4">Generic type parameter.</typeparam>
        /// <typeparam name="T5">Type of the t 5.</typeparam>
        /// <param name="item1">[in,out] The first item.</param>
        /// <param name="item2">[in,out] The second item.</param>
        /// <param name="item3">[in,out] The third item.</param>
        /// <param name="item4">[in,out] The fourth item.</param>
        /// <param name="item5">[in,out] The fifth item.</param>
        /// =================================================================================================
        internal static void ToDefaultValue<T1, T2, T3, T4, T5>(ref T1 item1, ref T2 item2, ref T3 item3, ref T4 item4, ref T5 item5)
        {
            ToDefaultValue(ref item1);
            ToDefaultValue(ref item2);
            ToDefaultValue(ref item3);
            ToDefaultValue(ref item4);
            ToDefaultValue(ref item5);
        }
    }
}