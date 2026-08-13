// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryToExecute
//  Author           : RzR
//  Created On       : 2025-11-16 23:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-12-15 19:19
// ***********************************************************************
//  <copyright file="TryBuilder.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace TryToExecute.Builder
{
    /// <summary>
    ///     Entry point for TryBuilder fluent API.
    ///     Usage:
    ///     <code>
    ///     <![CDATA[
    ///      TryBuilder.Do(async () => await FooAsync())
    ///          .Catch<IOException>(ex => ...)
    ///          .Finally(() => ...)
    ///          .ExecuteAsync();
    ///     ]]>
    /// </code>
    /// </summary>
    public static class TryBuilder
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="tryBlock">The try block.</param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderAsync<T> Do<T>(Func<Task<T>> tryBlock)
            => new TryBuilderAsync<T>(tryBlock);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderAsync<object> Do(Func<Task> tryBlock)
            => new TryBuilderAsync<object>(async () =>
            {
                await tryBlock();

                return null;
            });

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="tryBlock">The try block.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderAsync<T> Do<T>(Func<CancellationToken, Task<T>> tryBlock,
            CancellationToken cancellationToken = default)
            => new TryBuilderAsync<T>(tryBlock, cancellationToken);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderAsync<object> Do(Func<CancellationToken, Task> tryBlock,
            CancellationToken cancellationToken = default)
            => new TryBuilderAsync<object>(async token =>
            {
                await tryBlock(token);

                return null;
            }, cancellationToken);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="tryBlock">The try block.</param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderSync<T> Do<T>(Func<T> tryBlock)
            => new TryBuilderSync<T>(tryBlock);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderSync Do(Action tryBlock)
            => new TryBuilderSync(tryBlock);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <typeparam name="T">Generic type parameter.</typeparam>
        /// <param name="tryBlock">The try block.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderSync<T> Do<T>(Func<CancellationToken, T> tryBlock, CancellationToken cancellationToken = default)
            => new TryBuilderSync<T>(tryBlock, cancellationToken);

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Do/execute.
        /// </summary>
        /// <param name="tryBlock">The try block.</param>
        /// <param name="cancellationToken">
        ///     (Optional) A token that allows processing to be cancelled.
        /// </param>
        /// <returns>
        ///     A TryBuilderSync.
        /// </returns>
        /// =================================================================================================
        public static TryBuilderSync Do(Action<CancellationToken> tryBlock, CancellationToken cancellationToken = default)
            => new TryBuilderSync(() => { tryBlock(cancellationToken); }, cancellationToken);
    }

    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Non-generic sync builder for actions not returning a value.
    /// </summary>
    /// <seealso cref="T:TryToExecute.Builder.TryBuilderSync{Object}"/>
    /// =================================================================================================
    public sealed class TryBuilderSync : TryBuilderSync<object>
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderSync"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// =================================================================================================
        public TryBuilderSync(Action action)
            : base(() =>
            {
                action();

                return null;
            })
        {
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="TryBuilderSync"/> class.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <param name="cancellationToken">A token that allows processing to be cancelled.</param>
        /// =================================================================================================
        public TryBuilderSync(Action action, CancellationToken cancellationToken)
            : base(_ =>
            {
                action();

                return null;
            }, cancellationToken)
        {
        }
    }
}