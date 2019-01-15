// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Represents the result of an operation which may succeed or fail.
    /// </summary>
    /// <remarks>
    ///     Use <c>Result&lt;Unit&gt;</c> for operations that would otherwise be <c>void</c>.
    /// </remarks>
    /// <typeparam name="TData">
    ///     The type of data encapsulated by the result.
    /// </typeparam>
    public abstract class Result<TData>
    {
        /// <summary>
        ///     Combines this result with another.
        ///     If both are successful then <paramref>combineData</paramref> is invoked;
        ///     else if either is a failure then <paramref>combineErrors</paramref> is invoked.
        /// </summary>
        /// <typeparam name="TOtherData">
        ///     The type of data in the other result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the combined result.
        /// </typeparam>
        /// <param name="other">
        ///     The other result to be combined with this one.
        /// </param>
        /// <param name="combineData">
        ///     The function that is invoked to combine the data when both of the results are successful.
        /// </param>
        /// <param name="combineErrors">
        ///     The function that is invoked to combine the errors when either of the results is a failure.
        /// </param>
        /// <returns>
        ///     A new <see cref="Result{TNewData}" />.
        /// </returns>
        public abstract Result<TNewData> Combine<TOtherData, TNewData>(
            Result<TOtherData> other,
            Func<TData, TOtherData, TNewData> combineData,
            Func<Error, Error, Error> combineErrors);

        /// <summary>
        ///     Used to match on whether this result is a success or a failure.
        /// </summary>
        /// <typeparam name="T">
        ///     The type that is returned.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result represents a success.
        /// </param>
        /// <param name="onFailure">
        ///     The function that is invoked if this result represents a failure.
        /// </param>
        /// <returns>
        ///     A value that is mapped from either the data or the error.
        /// </returns>
        public abstract T Match<T>(Func<TData, T> onSuccess, Func<Error, T> onFailure);

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="mapData">
        ///     The function that is invoked to map the data.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>mapData</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Result<TNewData> Select<TNewData>(Func<TData, TNewData> mapData);

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="mapData">
        ///     The asynchronous function that is invoked to map the data.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>mapData</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Task<Result<TNewData>> Select<TNewData>(Func<TData, Task<TNewData>> mapData);

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it was successful.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful form chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Result<TNewData> Then<TNewData>(Func<TData, Result<TNewData>> onSuccess);

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it was successful.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful form chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="onSuccess">
        ///     The asynchronous function that is invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public abstract Task<Result<TNewData>> Then<TNewData>(Func<TData, Task<Result<TNewData>>> onSuccess);
    }
}