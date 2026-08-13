using System;
using TryToExecute.Builder;

namespace TryExecuteTests.Tests.Fluent
{
    [TestClass]
    public class TryBuilderSyncOtTTests
    {
        [TestMethod]
        public void DoAction_Test()
        {
            int finallyResult = 0;

            var build = new TryBuilderSync<int>(() => 10)
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                })
                .Finally(() => finallyResult = 1)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(10, build.Value);
            Assert.AreEqual(1, finallyResult);
        }
    }
}
