// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-01-15 16:42
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-01-20 19:34
// ***********************************************************************
//  <copyright file="TestTryExecuteAbstrBase.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using TryToExecute.CodeExec;
// ReSharper disable RedundantLambdaParameterType
// ReSharper disable UnusedParameter.Local
// ReSharper disable NotAccessedField.Local
// ReSharper disable RedundantArgumentDefaultValue
#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Base
{
    public abstract partial class TestTryExecuteAbstrBase : TryCatchExecuteBase
    {
        protected virtual async Task<TResult> TryAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Exception, Task<TResult>> onFailureResult)
            => await TryToExecuteAsync(execFunc, onFailureResult, () =>
            {
                execFunc = null;
                onFailureResult = null;
            }, false);

        protected virtual async Task<TResult> TryAsync<TResult>(
            Func<Task<TResult>> execFunc,
            Func<Exception, TResult> onFailureResult)
            => await TryToExecuteAsync(execFunc, onFailureResult, () =>
            {
                execFunc = null;
                onFailureResult = null;
            }, false);

        protected virtual async Task<TResult> TryAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<TResult> onFailureResult,
            ILogger<TLogger> exceptionLogger)
            => await TryToExecuteAsync(execFunc, onFailureResult, finallyExecAction: () =>
            {
                execFunc = null;
                onFailureResult = null;
                exceptionLogger = null;
            }, exceptionLogger: exceptionLogger, forceCallGarbageCollector: false);

        protected virtual async Task<TResult> TryAsync<TResult, TLogger>(
            Func<Task<TResult>> execFunc,
            Func<Task<TResult>> onFailureResult,
            ILogger<TLogger> exceptionLogger)
            => await TryToExecuteAsync(execFunc, onFailureResult, finallyExecAction: () =>
            {
                execFunc = null;
                onFailureResult = null;
                exceptionLogger = null;
            }, exceptionLogger: exceptionLogger, forceCallGarbageCollector: false);
    }
}