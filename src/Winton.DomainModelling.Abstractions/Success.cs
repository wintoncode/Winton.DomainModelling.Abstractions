// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Contains static methods to make working with void results easier.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleType",
        Justification = "Generic and non-generic version of the same class.")]
    public static class Success
    {
        /// <summary>
        ///     Creates a <see cref="Success{Unit}" /> to represent a successful result
        ///     that does not contain any data.
        /// </summary>
        /// <returns>
        ///     A new <see cref="Success{Unit}" />.
        /// </returns>
        public static Success<Unit> Unit()
        {
            return new Success<Unit>(DomainModelling.Unit.Value);
        }
    }

    /// <inheritdoc />
    /// <summary>
    ///     A result indicating a success.
    /// </summary>
    [SuppressMessage(
        "StyleCop.CSharp.MaintainabilityRules",
        "SA1402:FileMayOnlyContainASingleType",
        Justification = "Generic and non-generic version of the same class.")]
    public sealed class Success<TData> : Result<TData>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Success{TData}" /> class.
        /// </summary>
        /// <param name="data">The data encapsulated by the result.</param>
        /// <returns>A new instance of <see cref="Success{TData}" />.</returns>
        public Success(TData data)
        {
            Data = data;
        }

        /// <summary>
        ///     Gets the data.
        /// </summary>
        public TData Data { get; }

        /// <inheritdoc />
        public override Result<TNewData> Combine<TOtherData, TNewData>(
            Result<TOtherData> other,
            Func<TData, TOtherData, TNewData> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            return other.Match<Result<TNewData>>(
                otherData => new Success<TNewData>(combineData(Data, otherData)),
                otherError => new Failure<TNewData>(otherError));
        }

        /// <inheritdoc />
        public override T Match<T>(Func<TData, T> onSuccess, Func<Error, T> onFailure)
        {
            return onSuccess(Data);
        }

        /// <inheritdoc />
        public override Result<TNewData> Select<TNewData>(Func<TData, TNewData> mapData)
        {
            return new Success<TNewData>(mapData(Data));
        }

        /// <inheritdoc />
        public override async Task<Result<TNewData>> Select<TNewData>(Func<TData, Task<TNewData>> mapData)
        {
            TNewData data = await mapData(Data);
            return new Success<TNewData>(data);
        }

        /// <inheritdoc />
        public override Result<TNextData> Then<TNextData>(Func<TData, Result<TNextData>> onSuccess)
        {
            return onSuccess(Data);
        }

        /// <inheritdoc />
        public override Task<Result<TNextData>> Then<TNextData>(Func<TData, Task<Result<TNextData>>> onSuccess)
        {
            return onSuccess(Data);
        }
    }
}