// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Extension methods for asynchronous results which make it possible to chain async results together
    ///     using a fluent API in the same way as synchronous results.
    /// </summary>
    public static class AsyncResultExtensions
    {
        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it is successful after it has been awaited.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The async result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public static async Task<Result<TNewData>> Then<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, Result<TNewData>> onSuccess)
        {
            Result<TData> result = await resultTask;
            return result.Then(onSuccess);
        }

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it is successful after it has been awaited.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for chaining serial operations together that return results.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The async result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The asynchronous function that is invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     If this result is a success, then the result of <paramref>onSuccess</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public static async Task<Result<TNewData>> Then<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, Task<Result<TNewData>>> onSuccess)
        {
            Result<TData> result = await resultTask;
            return await result.Then(onSuccess);
        }
    }
}
