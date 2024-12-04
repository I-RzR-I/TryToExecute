#region U S A G E S

using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Abstractions;
using Microsoft.Extensions.Logging;
using TryToExecute.CodeExec;

#endregion

namespace TryExecuteTests;

[TestClass]
public class TryCatchExecuteStaticAsyncTests : TryCatchExecuteStaticBase
{
    private ILogger<TryCatchExecuteStaticAsyncTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteStaticAsyncTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteStaticAsyncTests>();
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_FuncAndTResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync(async () =>
        {
            throw new Exception("Exception");

            return await Task.FromResult(0);
        }, -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_FuncAndTResult_WithFuncExceptionDefault_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync(async () =>
        {
            throw new Exception("Exception");

            return await Task.FromResult(0);
        }, () =>
        {
            _logger.LogError("Internal error!");

            return -999;
        });

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync(async () => await Task.FromResult(1), -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithFuncResult_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () => await Task.FromResult(Result.Success()),
            () => Result.Failure("ResultFailure"));

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithFuncResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () =>
            {
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            () => Result.Failure("ResultFailure"));

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithAsyncErrorFuncResult_ShouldPass_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () => await Task.FromResult(Result.Success()),
            async () => await Task.FromResult(Result.Failure("ResultFailure")));

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
    }

    [TestMethod]
    public async Task TryToExecFuncAsync_WithAsyncErrorFuncResult_ShouldFail_Test()
    {
        var exec = await TryToExecuteAsync<IResult>(
            async () =>
            {
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            async () => await Task.FromResult(Result.Failure("ResultFailure")));

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
    }
}