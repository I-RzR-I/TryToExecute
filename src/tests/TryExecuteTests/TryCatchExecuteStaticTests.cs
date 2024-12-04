#region U S A G E S

using Microsoft.Extensions.Logging;
using TryToExecute.CodeExec;


// ReSharper disable NotAccessedField.Local

#endregion

namespace TryExecuteTests;

[TestClass]
public class TryCatchExecuteStaticTests : TryCatchExecuteStaticBase
{
    private ILogger<TryCatchExecuteStaticTests> _logger;

    [TestInitialize]
    public void Init()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
            builder.AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("TryCatchExecuteStaticTests", LogLevel.Debug)
                .AddConsole());

        _logger = loggerFactory.CreateLogger<TryCatchExecuteStaticTests>();
    }

    [TestMethod]
    public void TryToExecFunc_ShouldPass_Test()
    {
        var exec = TryToExecute(() => 1, -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public void TryToExecFunc_ShouldFail_ParseBoolean_Test()
    {
        var exec = TryToExecute(
            () =>
            {
                var x = "sss";

                return bool.Parse(x);
            }, false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(false, exec);
    }

    [TestMethod]
    public void TryToExecFunc_ShouldPass_ParseBoolean_Test()
    {
        var exec = TryToExecute(
            () =>
            {
                var x = "true";

                return bool.Parse(x);
            }, false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(true, exec);
    }

    [TestMethod]
    public void TryToExecFuncAsync_FuncAndTResult_ShouldFail_Test()
    {
        var exec = TryToExecute(() =>
        {
            throw new Exception("Exception");

            return 0;
        }, -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
    }

    [TestMethod]
    public void TryToExecFuncAsync_FuncAndFuncTResult_ShouldFail_Test()
    {
        var exec = TryToExecute(() =>
        {
            throw new Exception("Exception");

            return 0;
        }, () => -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
    }

    [TestMethod]
    public void TryToExecFuncAsync_FuncAndFuncTResult_ShouldPass_Test()
    {
        var exec = TryToExecute(() => 1, () => -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public void TryToExec_ExecTResult_FailureTResult_ShouldPass_Test()
    {
        var exec = TryToExecute(1, -1);

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
    }

    [TestMethod]
    public void TryToExecAction_funcFinally_ShouldPass_Test()
    {
        var changedValue = 0;
        var exec = TryToExecute(1, -1, () => { changedValue = 10; return 1; });

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
        Assert.AreEqual(10, changedValue);
    }

    [TestMethod]
    public void TryToExecAction_ActionFinally_ShouldPass_Test()
    {
        var changedValue = 0;
        var exec = TryToExecute(1, -1, () => { changedValue = 10; });

        Assert.IsNotNull(exec);
        Assert.AreEqual(1, exec);
        Assert.AreEqual(10, changedValue);
    }

    [TestMethod]
    public void TryToExec_FuncT_ResultTFailure_FuncFinally_FGC_ShouldFail_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () => { throw new Exception(""); changedValue++; return 0; },
            -1, () => { changedFinallyValue++; return 999; }, false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
        Assert.AreEqual(0, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncT_ResultTFailure_FuncFinally_TGC_ShouldFail_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () => { throw new Exception(""); changedValue++; return 0; },
            -1, () => { changedFinallyValue++; return 999; }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-1, exec);
        Assert.AreEqual(0, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncT_ResultTFailure_FuncFinally_FGC_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () => { changedValue++; return 0; },
            -1, () => { changedFinallyValue++; return 999; }, false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncT_ResultTFailure_FuncFinally_TGC_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            () => { changedValue++; return 0; },
            -1, () => { changedFinallyValue++; return 999; }, true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }
}