#region U S A G E S

using Microsoft.Extensions.Logging;
using System;
using TryToExecute.CodeExec;

// ReSharper disable RedundantArgumentDefaultValue
// ReSharper disable RedundantLambdaParameterType
// ReSharper disable NotAccessedField.Local
#pragma warning disable CS8618
#pragma warning disable CS0162

#endregion

namespace TryExecuteTests.Tests.Static;

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

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: -1,
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: -1,
            exceptionLogger: _logger,
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: -100,
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-100, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: -100,
            exceptionLogger: _logger,
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-100, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_FuncTResult_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: -1,
            exceptionLogger: _logger,
            finallyExecFunc: () => { changedFinallyValue++; _logger.LogInformation("Finally exec"); return 999; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_FuncTResult_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: -1,
            exceptionLogger: _logger,
            finallyExecFunc: () => { changedFinallyValue++; _logger.LogInformation("Finally exec"); return 999; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_FuncTResult_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: -100,
            exceptionLogger: _logger,
            finallyExecFunc: () => { changedFinallyValue++; _logger.LogInformation("Finally exec"); return 999; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-100, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_TResult_ILogger_FuncTResult_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: -100,
            exceptionLogger: _logger,
            finallyExecFunc: () => { changedFinallyValue++; _logger.LogInformation("Finally exec"); return 999; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-100, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(11, changedFinallyValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_FuncTResult_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_FuncTResult_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_FuncTResult_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncExceptionTResult_FuncTResult_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: (ex) => { _logger.LogError(ex, "Exception"); return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncTResult_FuncTResult_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncTResult_FuncTResult_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncTResult_FuncTResult_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResult_FuncTResult_FuncTResult_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            finallyExecFunc: () => { _logger.LogInformation("Finally"); return changedFinallyValue = -888; },
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(-888, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResultExecFunc_FuncTResultOnFailureResult_ExceptionLogger_GCF_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResultExecFunc_FuncTResultOnFailureResult_ExceptionLogger_GCT_ShouldPass_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            exceptionLogger: _logger,
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(0, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(101, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResultExecFunc_FuncTResultOnFailureResult_ExceptionLogger_GCF_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            exceptionLogger: _logger,
            forceCallGarbageCollector: false);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }

    [TestMethod]
    public void TryToExec_FuncTResultExecFunc_FuncTResultOnFailureResult_ExceptionLogger_GCT_ShouldPass_Exception_Test()
    {
        var changedValue = 0;
        var changedFinallyValue = 10;
        var changedExceptionValue = 101;
        var exec = TryToExecute(
            execFunc: () => { changedValue++; throw new Exception("Ex1"); return 0; },
            onFailureResult: () => { return changedExceptionValue = -999; },
            exceptionLogger: _logger,
            forceCallGarbageCollector: true);

        Assert.IsNotNull(exec);
        Assert.AreEqual(-999, exec);
        Assert.AreEqual(1, changedValue);
        Assert.AreEqual(10, changedFinallyValue);
        Assert.AreEqual(-999, changedExceptionValue);
    }
}