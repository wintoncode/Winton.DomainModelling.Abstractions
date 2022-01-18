// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling;

public class AsyncResultExtensionsTests
{
    public sealed class Combine : AsyncResultExtensionsTests
    {
        public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Success<int>(2),
                    new Success<int>(3)
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(2)),
                    new Failure<int>(new Error("Error", "Boom!")),
                    new Failure<int>(new Error("Error", "Boom!"))
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Ka"))),
                    new Success<int>(2),
                    new Failure<int>(new Error("Error", "Ka"))
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Ka"))),
                    new Failure<int>(new Error("Error", "Boom!")),
                    new Failure<int>(new Error("Error", "Ka-Boom!"))
                }
            };

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallCombine(
            Task<Result<int>> resultTask,
            Result<int> other,
            Result<int> expected)
        {
            Result<int> combined = await resultTask.Combine(
                other,
                (i, j) => i + j,
                (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

            combined.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndTheOtherResultAndCallCombine(
            Task<Result<int>> resultTask,
            Result<int> other,
            Result<int> expected)
        {
            Result<int> combined = await resultTask.Combine(
                Task.FromResult(other),
                (i, j) => i + j,
                (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

            combined.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }
    }

    public sealed class Match : AsyncResultExtensionsTests
    {
        public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    true
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    false
                }
            };

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallMatch(Task<Result<int>> resultTask, bool expected)
        {
            bool isSuccess = await resultTask.Match(_ => true, _ => false);

            isSuccess.Should().Be(expected);
        }
    }

    public sealed class OnSuccess : AsyncResultExtensionsTests
    {
        public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    true
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    false
                }
            };

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallAsyncOnSuccess(Task<Result<int>> resultTask, bool expected)
        {
            var invoked = false;

            async Task OnSuccess()
            {
                await Task.Yield();
                invoked = true;
            }

            await resultTask.OnSuccess(OnSuccess);

            invoked.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallAsyncOnSuccessWithParameter(
            Task<Result<int>> resultTask,
            bool expected)
        {
            var invoked = false;

            async Task OnSuccess(int i)
            {
                await Task.Yield();
                invoked = true;
            }

            await resultTask.OnSuccess(OnSuccess);

            invoked.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallOnSuccess(Task<Result<int>> resultTask, bool expected)
        {
            var invoked = false;

            await resultTask.OnSuccess(() => invoked = true);

            invoked.Should().Be(expected);
        }

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallOnSuccessWithParameter(
            Task<Result<int>> resultTask,
            bool expected)
        {
            var invoked = false;

            await resultTask.OnSuccess(i => invoked = true);

            invoked.Should().Be(expected);
        }
    }

    public sealed class Select : AsyncResultExtensionsTests
    {
        public static IEnumerable<object[]> AsyncTestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Func<int, Task<string>>(i => Task.FromResult($"{i}")),
                    new Success<string>("1")
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    new Func<int, Task<string>>(i => Task.FromResult($"{i}")),
                    new Failure<string>(new Error("Error", "Boom!"))
                }
            };

        public static IEnumerable<object[]> TestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Func<int, string>(i => $"{i}"),
                    new Success<string>("1")
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    new Func<int, string>(i => $"{i}"),
                    new Failure<string>(new Error("Error", "Boom!"))
                }
            };

        [Theory]
        [MemberData(nameof(TestCases))]
        private async Task ShouldAwaitTheResultAndCallSelect(
            Task<Result<int>> resultTask,
            Func<int, string> selectData,
            Result<string> expected)
        {
            Result<string> result = await resultTask.Select(selectData);

            result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }

        [Theory]
        [MemberData(nameof(AsyncTestCases))]
        private async Task ShouldAwaitTheResultAndCallSelectWithAsynchronousFunc(
            Task<Result<int>> resultTask,
            Func<int, Task<string>> selectData,
            Result<string> expected)
        {
            Result<string> result = await resultTask.Select(selectData);

            result.Should().BeEquivalentTo(expected, options => options.RespectingRuntimeTypes());
        }
    }

    public sealed class Then : AsyncResultExtensionsTests
    {
        public static IEnumerable<object[]> AsyncTestCases => new List<object[]>
            {
                new object[]
                {
                    Task.FromResult<Result<int>>(new Success<int>(1)),
                    new Func<int, Task<Result<int>>>(i => Task.FromResult<Result<int>>(new Success<int>(i + 1))),
                    new Success<int>(2)
                },
                new object[]
                {
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    new Func<int, Task<Result<int>>>(i => Task.FromResult<Result<int>>(new Success<int>(i + 1))),
                    new Failure<int>(new Error("Error", "Boom!"))
                }
            };

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
                    Task.FromResult<Result<int>>(new Failure<int>(new Error("Error", "Boom!"))),
                    new Func<int, Result<int>>(i => new Success<int>(i + 1)),
                    new Failure<int>(new Error("Error", "Boom!"))
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
        [MemberData(nameof(AsyncTestCases))]
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