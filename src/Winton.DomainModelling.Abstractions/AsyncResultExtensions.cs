// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Threading.Tasks;

namespace Winton.DomainModelling
{
    /// <summary>
    ///     Extension methods for asynchronous results which make it possible to chain asynchronous results together
    ///     using a fluent API in the same way as synchronous results.
    /// </summary>
    public static class AsyncResultExtensions
    {
        /// <summary>
        ///     Invokes the specified action if the result was successful and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation has been successful.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnSuccess<TData>(
            this Task<Result<TData>> resultTask,
            Action onSuccess)
        {
            return await resultTask.OnSuccess(data => onSuccess());
        }

        /// <summary>
        ///     Invokes the specified action if the result was successful and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation has been successful.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnSuccess<TData>(
            this Task<Result<TData>> resultTask,
            Action<TData> onSuccess)
        {
            Result<TData> result = await resultTask;
            return result.OnSuccess(onSuccess);
        }

        /// <summary>
        ///     Invokes the specified action if the result was successful and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation has been successful.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnSuccess<TData>(
            this Task<Result<TData>> resultTask,
            Func<Task> onSuccess)
        {
            return await resultTask.OnSuccess(data => onSuccess());
        }

        /// <summary>
        ///     Invokes the specified action if the result was successful and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation has been successful.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result represents a success.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnSuccess<TData>(
            this Task<Result<TData>> resultTask,
            Func<TData, Task> onSuccess)
        {
            Result<TData> result = await resultTask;
            return await result.OnSuccess(onSuccess);
        }

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="selectData">
        ///     The function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selectData</paramref> function
        ///     if this result is a success, otherwise the original error.
        /// </returns>
        public static async Task<Result<TNewData>> Select<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, TNewData> selectData)
        {
            Result<TData> result = await resultTask;
            return result.Select(selectData);
        }

        /// <summary>
        ///     Projects a successful result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the new result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="selectData">
        ///     The asynchronous function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selectData</paramref> function
        ///     if this result is a success, otherwise the original error.
        /// </returns>
        public static async Task<Result<TNewData>> Select<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, Task<TNewData>> selectData)
        {
            Result<TData> result = await resultTask;
            return await result.Select(selectData);
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
        ///     The asynchronous result that this extension method is invoked on.
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
        ///     The asynchronous result that this extension method is invoked on.
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