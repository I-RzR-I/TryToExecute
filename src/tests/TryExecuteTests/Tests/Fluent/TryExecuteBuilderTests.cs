// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-11-24 21:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-24 21:47
// ***********************************************************************
//  <copyright file="TryExecuteBuilderTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using TryToExecute.Builder;
using TryToExecute.Enums;
using TryToExecute.Models;

namespace TryExecuteTests.Tests.Fluent
{
    [TestClass]
    public class TryExecuteBuilderTests
    {
        [TestMethod]
        public void DoAction_Test()
        {
            int finallyResult = 0;

            var build = TryBuilder.Do(() => 10)
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

        [TestMethod]
        public void DoAction_Throw_ArgumentEx_Test()
        {
            int finallyResult = 0;
            int exceptionResult = 0;

            var build = TryBuilder.Do(() =>
                {
                    throw new ArgumentException("Exception");

                    return 10;
                })
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Catch<ArgumentNullException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 2;
                })
                .Finally(() => finallyResult = 1)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(ArgumentException), build.Exception.GetType());
            Assert.AreEqual((int)default, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(1, exceptionResult);
        }

        [TestMethod]
        public void DoAction_Throw_ArgumentNullEx_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;

            var build = TryBuilder.Do(() =>
                {
                    throw new ArgumentNullException("Exception");

                    return 10;
                })
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Catch<ArgumentNullException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 2;
                })
                .Finally(() => finallyResult = 1)
                .Finally(() => finallyResult = -1)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), build.Exception.GetType());
            Assert.AreEqual((int)default, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(2, exceptionResult);
        }

        [TestMethod]
        public void DoAction_Throw_ArgumentNullEx_Retry_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int retryCount = 0;
            int retryCount2 = 0;

            var build = TryBuilder.Do(() =>
                {
                    retryCount++;
                    throw new ArgumentNullException("Exception");

                    return 10;
                })
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Catch<ArgumentNullException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 2;
                })
                .Finally(() => finallyResult = 1)
                .Finally(() => finallyResult = -1)
                .Retry(new TryRetryPolicy(new TryRetryOptions()
                {
                    Strategy = TryRetryBackoffStrategy.Fixed,
                    MaxAttempts = 3,
                    OnRetry = (i, exception) => { retryCount2 = i; }
                }))
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(typeof(ArgumentNullException), build.Exception.GetType());
            Assert.AreEqual((int)default, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(2, exceptionResult);
            Assert.AreEqual(3, retryCount);
            Assert.AreEqual(3, retryCount2);
        }

        [TestMethod]
        public void DoAction_FallBack_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int fallBack = -1;

            var build = TryBuilder.Do(() =>
                {
                    throw new ArgumentNullException("Exception");

                    return 10;
                })
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Catch<ArgumentNullException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 2;
                })
                .Fallback(() => fallBack = 5)
                .Finally(() => finallyResult = 1)
                .Finally(() => finallyResult = -1)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(5, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(2, exceptionResult);
        }

        [TestMethod]
        public void DoAction_Func_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;

            var build = TryBuilder.Do(() =>
                {
                    result1 = 10;
                })
                .Catch<ArgumentException>(exception =>
                {
                    Console.Write(exception.ToString());
                    exceptionResult = 1;
                })
                .Catch<ArgumentNullException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 2;
                })
                .Catch<NullReferenceException>((exception, token) =>
                {

                    Console.Write(exception.ToString());
                    exceptionResult = 3;
                })
                .Finally(() => finallyResult = 1)
                .Finally(() => finallyResult = -1)
                .Execute();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(10, result1);
            Assert.AreEqual(null, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(0, exceptionResult);
        }
    }
}

