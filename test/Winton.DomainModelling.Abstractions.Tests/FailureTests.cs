// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class FailureTests
    {
        public sealed class Combine : FailureTests
        {
            [Fact]
            private void ShouldCombineErrorsIfTheOtherResultIsAFailure()
            {
                var failure = new Failure<int>(new Error("Ka"));

                Result<int> combined = failure.Combine(
                    new Failure<int>(new Error("Boom!")),
                    (i, j) => i + j,
                    (error, otherError) => new Error($"{error.Detail}-{otherError.Detail}"));

                combined.Should().BeEquivalentTo(new Failure<int>(new Error("Ka-Boom!")));
            }

            [Fact]
            private void ShouldReturnFailureWithOriginalErrorIfOtherIsASuccess()
            {
                var failure = new Failure<int>(new Error("Ka"));

                Result<int> combined = failure.Combine(
                    new Success<int>(2),
                    (i, j) => i + j,
                    (error, otherError) => new Error($"{error.Detail}-{otherError.Detail}"));

                combined.Should().BeEquivalentTo(failure);
            }
        }

        public sealed class Match : FailureTests
        {
            [Fact]
            private void ShouldInvokeOnFailureFunc()
            {
                var failure = new Failure<int>(new Error("Boom!"));

                bool matchedFailure = failure.Match(_ => false, _ => true);

                matchedFailure.Should().BeTrue();
            }
        }

        public sealed class Select : FailureTests
        {
            [Fact]
            private void ShouldReturnOriginalFailure()
            {
                var failure = new Failure<int>(new Error("Boom!"));

                Result<string> result = failure.Select(i => $"{i}");

                result.Should().BeEquivalentTo(failure);
            }

            [Fact]
            private async Task ShouldReturnOriginalFailureAsynchronously()
            {
                var failure = new Failure<int>(new Error("Boom!"));

                Result<string> result = await failure.Select(i => Task.FromResult($"{i}"));

                result.Should().BeEquivalentTo(failure);
            }
        }

        public sealed class Then : FailureTests
        {
            [Fact]
            private void ShouldReturnFailureWithOriginalError()
            {
                var failure = new Failure<int>(new Error("Boom!"));

                Result<int> result = failure.Then(i => new Success<int>(i + 1));

                result.Should().BeEquivalentTo(failure);
            }

            [Fact]
            private async Task ShouldReturnFailureWithOriginalErrorAsynchronously()
            {
                async Task<Result<int>> OnSuccess(int i)
                {
                    await Task.Yield();
                    return new Success<int>(i + 1);
                }

                var failure = new Failure<int>(new Error("Boom!"));

                Result<int> result = await failure.Then(OnSuccess);

                result.Should().BeEquivalentTo(failure);
            }
        }
    }
}