// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     A result indicating a failure.
    /// </summary>
    public sealed class Failure<TData> : Result<TData>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Failure{TData}" /> class.
        /// </summary>
        /// <param name="error">The error that caused the result to be a failure.</param>
        /// <returns>A new instance of <see cref="Failure{TData}" />.</returns>
        public Failure(Error error)
        {
            Error = error;
        }

        /// <summary>
        ///     Gets the error that caused the failure.
        /// </summary>
        public Error Error { get; }

        /// <inheritdoc />
        public override Result<TData> Catch(Func<Error, Result<TData>> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TData>> Catch(Func<Error, Task<Result<TData>>> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Result<TNewData> Combine<TOtherData, TNewData>(
            Result<TOtherData> other,
            Func<TData, TOtherData, TNewData> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            return other.Match<Result<TNewData>>(
                otherData => new Failure<TNewData>(Error),
                otherError => new Failure<TNewData>(combineErrors(Error, otherError)));
        }

        /// <inheritdoc />
        public override T Match<T>(Func<TData, T> onSuccess, Func<Error, T> onFailure)
        {
            return onFailure(Error);
        }

        /// <inheritdoc />
        public override Result<TData> OnFailure(Action onFailure)
        {
            return OnFailure(_ => onFailure());
        }

        /// <inheritdoc />
        public override Result<TData> OnFailure(Action<Error> onFailure)
        {
            onFailure(Error);
            return this;
        }

        /// <inheritdoc />
        public override Task<Result<TData>> OnFailure(Func<Task> onFailure)
        {
            return OnFailure(_ => onFailure());
        }

        /// <inheritdoc />
        public override async Task<Result<TData>> OnFailure(Func<Error, Task> onFailure)
        {
            await onFailure(Error);
            return this;
        }

        /// <inheritdoc />
        public override Result<TData> OnSuccess(Action onSuccess)
        {
            return this;
        }

        /// <inheritdoc />
        public override Result<TData> OnSuccess(Action<TData> onSuccess)
        {
            return this;
        }

        /// <inheritdoc />
        public override Task<Result<TData>> OnSuccess(Func<Task> onSuccess)
        {
            return Task.FromResult<Result<TData>>(this);
        }

        /// <inheritdoc />
        public override Task<Result<TData>> OnSuccess(Func<TData, Task> onSuccess)
        {
            return Task.FromResult<Result<TData>>(this);
        }

        /// <inheritdoc />
        public override Result<TNewData> Select<TNewData>(Func<TData, TNewData> selector)
        {
            return new Failure<TNewData>(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TNewData>> Select<TNewData>(Func<TData, Task<TNewData>> selector)
        {
            return Task.FromResult<Result<TNewData>>(new Failure<TNewData>(Error));
        }

        /// <inheritdoc />
        public override Result<TData> SelectError(Func<Error, Error> selector)
        {
            return new Failure<TData>(selector(Error));
        }

        /// <inheritdoc />
        public override async Task<Result<TData>> SelectError(Func<Error, Task<Error>> selector)
        {
            return new Failure<TData>(await selector(Error));
        }

        /// <inheritdoc />
        public override Result<TNextData> Then<TNextData>(Func<TData, Result<TNextData>> onSuccess)
        {
            return new Failure<TNextData>(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TNextData>> Then<TNextData>(Func<TData, Task<Result<TNextData>>> onSuccess)
        {
            return Task.FromResult<Result<TNextData>>(new Failure<TNextData>(Error));
        }
    }
}