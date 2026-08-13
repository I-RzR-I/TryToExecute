#region U S A G E S

using AggregatedGenericResultMessage.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
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
