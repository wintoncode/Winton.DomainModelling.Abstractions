// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling
{
    public class SuccessTests
    {
        public sealed class Combine : SuccessTests
        {
            [Fact]
            private void ShouldCombineDataElementsIfOtherResultIsASuccess()
            {
                var success = new Success<int>(1);

                Result<int> combined = success.Combine(
                    new Success<int>(2),
                    (i, j) => i + j,
                    (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

                combined.Should().BeEquivalentTo(new Success<int>(3));
            }

            [Fact]
            private void ShouldReturnFailureWithOtherErrorIfTheOtherResultIsAFailure()
            {
                var success = new Success<int>(1);
                var otherResult = new Failure<int>(new Error("Error", "Boom!"));

                Result<int> combined = success.Combine(
                    otherResult,
                    (i, j) => i + j,
                    (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

                combined.Should().BeEquivalentTo(otherResult);
            }
        }

        public sealed class Match : SuccessTests
        {
            [Fact]
            private void ShouldInvokeOnSuccessFunc()
            {
                var success = new Success<int>(1);

                bool matchedSuccess = success.Match(_ => true, _ => false);

                matchedSuccess.Should().BeTrue();
            }
        }

        public sealed class Select : SuccessTests
        {
            [Fact]
            private void ShouldProjectDataToNewType()
            {
                var success = new Success<int>(1);

                Result<string> result = success.Select(i => $"{i}");

                result.Should().BeEquivalentTo(new Success<string>("1"));
            }

            [Fact]
            private async Task ShouldProjectDataToNewTypeAsynchronously()
            {
                var success = new Success<int>(1);

                Result<string> result = await success.Select(i => Task.FromResult($"{i}"));

                result.Should().BeEquivalentTo(new Success<string>("1"));
            }
        }

        public sealed class Then : SuccessTests
        {
            [Fact]
            private void ShouldInvokeOnSuccessFunc()
            {
                var success = new Success<int>(1);

                Result<int> result = success.Then(i => new Success<int>(i + 1));

                result.Should().BeEquivalentTo(new Success<int>(2));
            }

            [Fact]
            private async Task ShouldInvokeOnSuccessFuncAsynchronously()
            {
                async Task<Result<int>> OnSuccess(int i)
                {
                    await Task.Yield();
                    return new Success<int>(i + 1);
                }

                var success = new Success<int>(1);

                Result<int> result = await success.Then(OnSuccess);

                result.Should().BeEquivalentTo(new Success<int>(2));
            }
        }

        public sealed class Unit : SuccessTests
        {
            [Fact]
            private void ShouldReturnSuccessWithUnitData()
            {
                Success<DomainModelling.Unit> success = Success.Unit();

                success.Should().BeEquivalentTo(new Success<DomainModelling.Unit>(DomainModelling.Unit.Value));
            }
        }
    }
}