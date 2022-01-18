// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Winton.DomainModelling;

public class FailureTests
{
    public sealed class Catch : FailureTests
    {
        [Fact]
        private void ShouldInvokeOnFailureFunc()
        {
            Result<int> OnFailure(Error e)
            {
                return new Failure<int>(new Error(e.Title, $"Ka-{e.Detail}"));
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = failure.Catch(e => OnFailure(e));

            result.Should().BeEquivalentTo(new Failure<int>(new Error("Error", "Ka-Boom!")));
        }

        [Fact]
        private async Task ShouldInvokeOnFailureFuncAsynchronously()
        {
            async Task<Result<int>> OnFailure(Error e)
            {
                await Task.Yield();
                return new Failure<int>(new Error(e.Title, $"Ka-{e.Detail}"));
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = await failure.Catch(async e => await OnFailure(e));

            result.Should().BeEquivalentTo(new Failure<int>(new Error("Error", "Ka-Boom!")));
        }
    }

    public sealed class Combine : FailureTests
    {
        [Fact]
        private void ShouldCombineErrorsIfTheOtherResultIsAFailure()
        {
            var failure = new Failure<int>(new Error("Error", "Ka"));

            Result<int> combined = failure.Combine(
                new Failure<int>(new Error("Error", "Boom!")),
                (i, j) => i + j,
                (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

            combined.Should().BeEquivalentTo(new Failure<int>(new Error("Error", "Ka-Boom!")));
        }

        [Fact]
        private void ShouldReturnFailureWithOriginalErrorIfOtherIsASuccess()
        {
            var failure = new Failure<int>(new Error("Error", "Ka"));

            Result<int> combined = failure.Combine(
                new Success<int>(2),
                (i, j) => i + j,
                (error, otherError) => new Error("Error", $"{error.Detail}-{otherError.Detail}"));

            combined.Should().BeEquivalentTo(failure);
        }
    }

    public sealed class Match : FailureTests
    {
        [Fact]
        private void ShouldInvokeOnFailureFunc()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            bool matchedFailure = failure.Match(_ => false, _ => true);

            matchedFailure.Should().BeTrue();
        }
    }

    public sealed class OnFailure : FailureTests
    {
        [Fact]
        private void ShouldInvokeAction()
        {
            var invoked = false;
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            failure.OnFailure(() => invoked = true);

            invoked.Should().BeTrue();
        }

        [Fact]
        private void ShouldNotInvokeActionWithParameters()
        {
            var invoked = false;
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            failure.OnFailure(i => invoked = true);

            invoked.Should().BeTrue();
        }

        [Fact]
        private async Task ShouldNotInvokeAsyncAction()
        {
            var invoked = false;
            async Task OnFailure()
            {
                await Task.Yield();
                invoked = true;
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            await failure.OnFailure(OnFailure);

            invoked.Should().BeTrue();
        }

        [Fact]
        private async Task ShouldNotInvokeAsyncActionWithParameters()
        {
            var invoked = false;
            async Task OnFailure(Error e)
            {
                await Task.Yield();
                invoked = true;
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            await failure.OnFailure(OnFailure);

            invoked.Should().BeTrue();
        }

        [Fact]
        private void ShouldReturnOriginalResult()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = failure.OnFailure(() => { });

            result.Should().BeSameAs(failure);
        }

        [Fact]
        private async Task ShouldReturnOriginalResultWhenAsyncAction()
        {
            async Task OnFailure()
            {
                await Task.Yield();
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = await failure.OnFailure(OnFailure);

            result.Should().BeSameAs(failure);
        }
    }

    public sealed class OnSuccess : FailureTests
    {
        [Fact]
        private void ShouldNotInvokeAction()
        {
            var invoked = false;
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            failure.OnSuccess(() => invoked = true);

            invoked.Should().BeFalse();
        }

        [Fact]
        private void ShouldNotInvokeActionWithParameters()
        {
            var invoked = false;
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            failure.OnSuccess(i => invoked = true);

            invoked.Should().BeFalse();
        }

        [Fact]
        private async Task ShouldNotInvokeAsyncAction()
        {
            var invoked = false;
            async Task OnSuccess()
            {
                await Task.Yield();
                invoked = true;
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            await failure.OnSuccess(OnSuccess);

            invoked.Should().BeFalse();
        }

        [Fact]
        private async Task ShouldNotInvokeAsyncActionWithParameters()
        {
            var invoked = false;
            async Task OnSuccess(int i)
            {
                await Task.Yield();
                invoked = true;
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            await failure.OnSuccess(OnSuccess);

            invoked.Should().BeFalse();
        }

        [Fact]
        private void ShouldReturnOriginalResult()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = failure.OnSuccess(() => { });

            result.Should().BeSameAs(failure);
        }

        [Fact]
        private async Task ShouldReturnOriginalResultWhenAsyncAction()
        {
            async Task OnSuccess()
            {
                await Task.Yield();
            }

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = await failure.OnSuccess(OnSuccess);

            result.Should().BeSameAs(failure);
        }
    }

    public sealed class Select : FailureTests
    {
        [Fact]
        private void ShouldReturnOriginalFailure()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<string> result = failure.Select(i => $"{i}");

            result.Should().BeEquivalentTo(failure);
        }

        [Fact]
        private async Task ShouldReturnOriginalFailureAsynchronously()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<string> result = await failure.Select(i => Task.FromResult($"{i}"));

            result.Should().BeEquivalentTo(failure);
        }
    }

    public sealed class SelectError : SuccessTests
    {
        [Fact]
        private void ShouldProjectError()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = failure.SelectError(e => new NotFoundError(e.Detail));

            result.Should().BeEquivalentTo(new Failure<int>(new NotFoundError("Boom!")));
        }

        [Fact]
        private async Task ShouldProjectErrorAsynchronously()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = await failure.SelectError(
                e => Task.FromResult<Error>(new NotFoundError(e.Detail)));

            result.Should().BeEquivalentTo(new Failure<int>(new NotFoundError("Boom!")));
        }
    }

    public sealed class Then : FailureTests
    {
        [Fact]
        private void ShouldReturnFailureWithOriginalError()
        {
            var failure = new Failure<int>(new Error("Error", "Boom!"));

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

            var failure = new Failure<int>(new Error("Error", "Boom!"));

            Result<int> result = await failure.Then(OnSuccess);

            result.Should().BeEquivalentTo(failure);
        }
    }
}