using AggregatedGenericResultMessage.Abstractions;
using AggregatedGenericResultMessage;
using AggregatedGenericResultMessage.Enums;
using AggregatedGenericResultMessage.Extensions.Result.Messages;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using System;
using TryExecuteTests.Base;
#pragma warning disable CS0162

namespace TryExecuteTests.Tests.Base;

[TestClass]
public class TestTryExecuteAbstrBaseTests : TestTryExecuteAbstrBase
{
    private ILogger<TestTryExecuteAbstrBaseTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TestTryExecuteAbstrBaseTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TestTryExecuteAbstrBaseTests>();
    }

    [TestMethod]
    public async Task TryAsync_ExecFuncTaskTResult_FailureExceptionTaskTResult_ShouldFailWithMessage_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;
                throw new Exception("Exception");

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;
                await Task.CompletedTask;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec.IsSuccess);
        Assert.AreEqual(2, exec.Messages.Count);
        Assert.AreEqual("ResultFailure", exec.GetFirstMessage());
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public async Task TryAsync_ExecFuncTaskTResult_FailureExceptionTaskTResult_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 100;

        var exec = await TryToExecuteAsync<IResult>(
            execFunc: async () =>
            {
                changedValue++;

                return await Task.FromResult(Result.Success());
            },
            onFailureResult: async (Exception ex) =>
            {
                changedExceptionValue++;
                await Task.CompletedTask;

                return Result.Failure("ResultFailure").AddMessage("FailureMessage", MessageType.Error);
            });

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec.IsSuccess);
        Assert.AreEqual(0, exec.Messages.Count);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(100, changedExceptionValue);
    }
}
