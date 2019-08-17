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
        ///     Invokes another result generating function which takes as input the error of this result
        ///     if it is a failure after it has been awaited.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for handling errors.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The function that is invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     If this result is a failure, then the result of <paramref>onFailure</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public static async Task<Result<TData>> Catch<TData>(
            this Task<Result<TData>> resultTask,
            Func<Error, Result<TData>> onFailure)
        {
            Result<TData> result = await resultTask;
            return result.Catch(onFailure);
        }

        /// <summary>
        ///     Invokes another result generating function which takes as input the error of this result
        ///     if it is a failure after it has been awaited.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for handling errors.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The asynchronous function that is invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     If this result is a failure, then the result of <paramref>onFailure</paramref> function;
        ///     otherwise the original error.
        /// </returns>
        public static async Task<Result<TData>> Catch<TData>(
            this Task<Result<TData>> resultTask,
            Func<Error, Task<Result<TData>>> onFailure)
        {
            Result<TData> result = await resultTask;
            return await result.Catch(onFailure);
        }

        /// <summary>
        ///     Combines this result with another.
        ///     If both are successful then <paramref>combineData</paramref> is invoked;
        ///     else if either is a failure then <paramref>combineErrors</paramref> is invoked.
        /// </summary>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TOtherData">
        ///     The type of data in the other result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the combined result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
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
        public static async Task<Result<TNewData>> Combine<TData, TOtherData, TNewData>(
            this Task<Result<TData>> resultTask,
            Result<TOtherData> other,
            Func<TData, TOtherData, TNewData> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            Result<TData> result = await resultTask;
            return result.Combine(other, combineData, combineErrors);
        }

        /// <summary>
        ///     Combines this result with another.
        ///     If both are successful then <paramref>combineData</paramref> is invoked;
        ///     else if either is a failure then <paramref>combineErrors</paramref> is invoked.
        /// </summary>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="TOtherData">
        ///     The type of data in the other result.
        /// </typeparam>
        /// <typeparam name="TNewData">
        ///     The type of data in the combined result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="otherResultTask">
        ///     The other asynchronous result to be combined with this one.
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
        public static async Task<Result<TNewData>> Combine<TData, TOtherData, TNewData>(
            this Task<Result<TData>> resultTask,
            Task<Result<TOtherData>> otherResultTask,
            Func<TData, TOtherData, TNewData> combineData,
            Func<Error, Error, Error> combineErrors)
        {
            Result<TData> result = await resultTask;
            Result<TOtherData> otherResult = await otherResultTask;
            return result.Combine(otherResult, combineData, combineErrors);
        }

        /// <summary>
        ///     Used to match on whether this result is a success or a failure.
        /// </summary>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <typeparam name="T">
        ///     The type that is returned.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The function that is invoked if this result represents a success.
        /// </param>
        /// <param name="onFailure">
        ///     The function that is invoked if this result represents a failure.
        /// </param>
        /// <returns>
        ///     A value that is mapped from either the data or the error.
        /// </returns>
        public static async Task<T> Match<TData, T>(
            this Task<Result<TData>> resultTask,
            Func<TData, T> onSuccess,
            Func<Error, T> onFailure)
        {
            Result<TData> result = await resultTask;
            return result.Match(onSuccess, onFailure);
        }

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnFailure<TData>(
            this Task<Result<TData>> resultTask,
            Action onFailure)
        {
            return await resultTask.OnFailure(_ => onFailure());
        }

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnFailure<TData>(
            this Task<Result<TData>> resultTask,
            Action<Error> onFailure)
        {
            Result<TData> result = await resultTask;
            return result.OnFailure(onFailure);
        }

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The asynchronous action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnFailure<TData>(
            this Task<Result<TData>> resultTask,
            Func<Task> onFailure)
        {
            return await resultTask.OnFailure(_ => onFailure());
        }

        /// <summary>
        ///     Invokes the specified action if the result is a failure and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op and the original success is retained.
        ///     This is useful for publishing domain model notifications when an operation fails.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onFailure">
        ///     The asynchronous action that will be invoked if this result is a failure.
        /// </param>
        /// <returns>
        ///     The original result.
        /// </returns>
        public static async Task<Result<TData>> OnFailure<TData>(
            this Task<Result<TData>> resultTask,
            Func<Error, Task> onFailure)
        {
            Result<TData> result = await resultTask;
            return await result.OnFailure(onFailure);
        }

        /// <summary>
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result is a success.
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
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The action that will be invoked if this result is a success.
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
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result is a success.
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
        ///     Invokes the specified action if the result is a success and returns the original result.
        /// </summary>
        /// <remarks>
        ///     If this result is a failure then this is a no-op and the original failure is retained.
        ///     This is useful for publishing domain model notifications when an operation succeeds.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="onSuccess">
        ///     The asynchronous action that will be invoked if this result is a success.
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
        /// <param name="selector">
        ///     The function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a success, otherwise the original failure.
        /// </returns>
        public static async Task<Result<TNewData>> Select<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, TNewData> selector)
        {
            Result<TData> result = await resultTask;
            return result.Select(selector);
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
        /// <param name="selector">
        ///     The asynchronous function that is invoked to select the data.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a success, otherwise the original failure.
        /// </returns>
        public static async Task<Result<TNewData>> Select<TData, TNewData>(
            this Task<Result<TData>> resultTask,
            Func<TData, Task<TNewData>> selector)
        {
            Result<TData> result = await resultTask;
            return await result.Select(selector);
        }

        /// <summary>
        ///     Projects a failed result's data from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="selector">
        ///     The function that is invoked to select the error.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a failure, otherwise the original success.
        /// </returns>
        public static async Task<Result<TData>> SelectError<TData>(
            this Task<Result<TData>> resultTask,
            Func<Error, Error> selector)
        {
            Result<TData> result = await resultTask;
            return result.SelectError(selector);
        }

        /// <summary>
        ///     Projects a failed result's error from one type to another.
        /// </summary>
        /// <remarks>
        ///     If this result is a success then this is a no-op.
        /// </remarks>
        /// <typeparam name="TData">
        ///     The type of data encapsulated by the result.
        /// </typeparam>
        /// <param name="resultTask">
        ///     The asynchronous result that this extension method is invoked on.
        /// </param>
        /// <param name="selector">
        ///     The asynchronous function that is invoked to select the error.
        /// </param>
        /// <returns>
        ///     A new result containing either; the output of the <paramref>selector</paramref> function
        ///     if this result is a failure, otherwise the original success.
        /// </returns>
        public static async Task<Result<TData>> SelectError<TData>(
            this Task<Result<TData>> resultTask,
            Func<Error, Task<Error>> selector)
        {
            Result<TData> result = await resultTask;
            return await result.SelectError(selector);
        }

        /// <summary>
        ///     Invokes another result generating function which takes as input the data of this result
        ///     if it is a success after it has been awaited.
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
        ///     The function that is invoked if this result is a success.
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
        ///     if it is a success after it has been awaited.
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
        ///     The asynchronous function that is invoked if this result is a success.
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