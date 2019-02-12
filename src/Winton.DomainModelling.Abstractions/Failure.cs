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
        public override Result<TNewData> Select<TNewData>(Func<TData, TNewData> selectData)
        {
            return new Failure<TNewData>(Error);
        }

        /// <inheritdoc />
        public override Task<Result<TNewData>> Select<TNewData>(Func<TData, Task<TNewData>> selectData)
        {
            return Task.FromResult<Result<TNewData>>(new Failure<TNewData>(Error));
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