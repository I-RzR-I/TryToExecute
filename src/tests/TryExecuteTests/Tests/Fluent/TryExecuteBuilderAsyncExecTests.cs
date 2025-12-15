// ***********************************************************************
//  Assembly         : RzR.Shared.Extensions.TryExecuteTests
//  Author           : RzR
//  Created On       : 2025-11-30 21:11
// 
//  Last Modified By : RzR
//  Last Modified On : 2025-11-30 21:03
// ***********************************************************************
//  <copyright file="TryExecuteBuilderAsyncExecTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

using System;
using System.Threading.Tasks;
using TryToExecute.Builder;
using TryToExecute.Enums;
using TryToExecute.Models;

namespace TryExecuteTests.Tests.Fluent
{
    [TestClass]
    public class TryExecuteBuilderAsyncExecTests
    {
        [TestMethod]
        public async Task TryBuilder_Async_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;

            var build = await TryBuilder.Do(async () =>
            {
                await Task.CompletedTask;

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

                return Task.FromResult(0);
            })
            .Catch<NullReferenceException>((exception, token) =>
            {

                Console.Write(exception.ToString());
                exceptionResult = 3;

                return Task.Delay(0, token);
            })
            .Finally(() => finallyResult -= 1)
            .Finally(() => finallyResult -= 1)
            .Finally(async () =>
            {
                finallyResult -= 1;
                await Task.CompletedTask;
            })
            .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(10, build.Value);
            Assert.AreEqual(7, finallyResult);
            Assert.AreEqual(0, exceptionResult);
        }

        [TestMethod]
        public async Task TryBuilder_Async_FalBack_Test()
        {
            int fallBack = -1;
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;

            var build = await TryBuilder.Do(async () =>
            {
                await Task.CompletedTask;

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

                return Task.FromResult(0);
            })
            .Catch<NullReferenceException>((exception, token) =>
            {

                Console.Write(exception.ToString());
                exceptionResult = 3;

                return Task.Delay(0, token);
            })
            .Finally(() => finallyResult -= 1)
            .Finally(() => finallyResult -= 1)
            .Finally(async () =>
            {
                finallyResult -= 1;
                await Task.CompletedTask;
            })
            .Fallback(() => fallBack = 5)
            .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(10, build.Value);
            Assert.AreEqual(7, finallyResult);
            Assert.AreEqual(0, exceptionResult);
            Assert.AreEqual(-1, fallBack);
        }

        [TestMethod]
        public async Task TryBuilder_Async_FalBack_Exception_Test()
        {
            int fallBack = -1;
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;
            int retryCount2 = 0;

            var build = await TryBuilder.Do(async () =>
                {
                    throw new NullReferenceException("Null");
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

                return Task.FromResult(0);
            })
            .Catch<NullReferenceException>((exception, token) =>
            {

                Console.Write(exception.ToString());
                exceptionResult = 3;

                return Task.Delay(0, token);
            })
            .Finally(() => finallyResult -= 1)
            .Finally(() => finallyResult -= 1)
            .Finally(async () =>
            {
                finallyResult -= 1;
                await Task.CompletedTask;
            })
            .Fallback(() => fallBack = 5)
            .Retry(new TryRetryPolicy(new TryRetryOptions()
            {
                Strategy = TryRetryBackoffStrategy.Fixed,
                MaxAttempts = 3,
                OnRetry = (i, exception) => { retryCount2 = i; }
            }))
            .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(5, build.Value);
            Assert.AreEqual(7, finallyResult);
            Assert.AreEqual(3, exceptionResult);
            Assert.AreEqual(5, fallBack);
            Assert.AreEqual(3, retryCount2);
        }

        [TestMethod]
        public async Task TryBuilder_Async_Exception_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;

            var build = await TryBuilder.Do(async () =>
            {
                await Task.CompletedTask;

                return 10;
            })
            .Catch<ArgumentException>(async exception =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 1;
            })
            .Catch<ArgumentNullException>(async (exception, token) =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 2;
            })
            .Catch<NullReferenceException>(async (exception, token) =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 3;
            })
            .Finally(() => finallyResult = 1)
            .Finally(() => finallyResult = -1)
            .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsTrue(build.Succeeded);
            Assert.IsNull(build.Exception);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(10, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(0, exceptionResult);
        }

        [TestMethod]
        public async Task TryBuilder_Async_Exception_Handle_Test()
        {
            int finallyResult = 10;
            int exceptionResult = 0;
            int result1 = 0;

            var build = await TryBuilder.Do(async () =>
            {
                throw new ArgumentNullException("Ex2");
            })
            .Catch<ArgumentException>(async exception =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 1;
            })
            .Catch<ArgumentNullException>(async (exception, token) =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 2;
            })
            .Catch<NullReferenceException>(async (exception, token) =>
            {
                await Task.CompletedTask;
                Console.Write(exception.ToString());
                exceptionResult = 3;
            })
            .Finally(() => finallyResult = 1)
            .Finally(() => finallyResult = -1)
            .ExecuteAsync();

            Assert.IsNotNull(build);
            Assert.IsFalse(build.Succeeded);
            Assert.IsNotNull(build.Exception);
            Assert.AreEqual(0, result1);
            Assert.AreEqual(null, build.Value);
            Assert.AreEqual(1, finallyResult);
            Assert.AreEqual(2, exceptionResult);
        }
    }
}

