// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class AsyncResultExtensionsTests
    {
        public sealed class Then : AsyncResultExtensionsTests
        {
            public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Func<int, Result<int>>(i => new Success<int>(i + 1)),
                    new Success<int>(2)
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Boom!"))),
                    new Func<int, Result<int>>(i => new Success<int>(i + 1)),
                    new Failure<int>(new Error("Boom!"))
                }
            };

            public static IEnumerable<object[]> AsyncOnSuccessTestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Func<int, Task<Result<int>>>(i => Task.FromResult<Result<int>>(new Success<int>(i + 1))),
                    new Success<int>(2)
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Boom!"))),
                    new Func<int, Task<Result<int>>>(i => Task.FromResult<Result<int>>(new Success<int>(i + 1))),
                    new Failure<int>(new Error("Boom!"))
                }
            };

            [Theory]
            [MemberData(nameof(TestCases))]
            private async Task ShouldAwaitTheResultAndCallThen(
                Task<Result<int>> resultTask,
                Func<int, Result<int>> onSuccess,
                Result<int> expected)
            {
                Result<int> result = await resultTask.Then(onSuccess);

                result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            }

            [Theory]
            [MemberData(nameof(AsyncOnSuccessTestCases))]
            private async Task ShouldAwaitTheResultAndCallThenWithAsynchronousFunc(
                Task<Result<int>> resultTask,
                Func<int, Task<Result<int>>> onSuccess,
                Result<int> expected)
            {
                Result<int> result = await resultTask.Then(onSuccess);

                result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
            }
        }
    }
}