// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-15 16:45
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-20 19:34
// ***********************************************************************
//  <copyright file="TestTryExecuteAbstrResultBase.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable RedundantArgumentDefaultValue
#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Base
{
    public abstract partial class TestTryExecuteAbstrBase
    {
        protected virtual async Task<IResult<TResult>> TryAsync<TResult>(
            Func<Task<IResult<TResult>>> execFunc,
            Func<Exception, Task<IResult<TResult>>> onFailureResult)
            => await TryToExecuteAsync(execFunc, onFailureResult, () =>
            {
                execFunc = null;
                onFailureResult = null;
            }, false);

        protected virtual async Task<IResult<TResult>> TryAsync<TResult>(
            Func<Task<IResult<TResult>>> execFunc,
            Func<Exception, IResult<TResult>> onFailureResult)
            => await TryToExecuteAsync(execFunc, onFailureResult, () =>
            {
                execFunc = null;
                onFailureResult = null;
            }, false);

        protected virtual async Task<IResult<TResult>> TryAsync<TResult, TLogger>(
            Func<Task<IResult<TResult>>> execFunc,
            Func<IResult<TResult>> onFailureResult,
            ILogger<TLogger> exceptionLogger)
            => await TryToExecuteAsync(execFunc, onFailureResult, finallyExecAction: () =>
            {
                execFunc = null;
                onFailureResult = null;
                exceptionLogger = null;
            }, exceptionLogger: exceptionLogger, forceCallGarbageCollector: false);

        protected virtual async Task<IResult<TResult>> TryAsync<TResult, TLogger>(
            Func<Task<IResult<TResult>>> execFunc,
            Func<Task<IResult<TResult>>> onFailureResult,
            ILogger<TLogger> exceptionLogger)
            => await TryToExecuteAsync(execFunc, onFailureResult, finallyExecAction: () =>
            {
                execFunc = null;
                onFailureResult = null;
                exceptionLogger = null;
            }, exceptionLogger: exceptionLogger, forceCallGarbageCollector: false);
    }
}