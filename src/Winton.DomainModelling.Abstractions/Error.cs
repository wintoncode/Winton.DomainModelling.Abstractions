// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Winton.DomainModelling
{
    /// <summary>
    ///     The base class of all errors in the domain model.
    /// </summary>
    /// <remarks>
    ///     In a domain model it is better to explicitly model errors rather than throw exceptions.
    ///     Errors are legitimate outcomes of operations in a domain model where an operation is
    ///     not guaranteed to succeed. Errors can occur for several reasons, such as not being able
    ///     to find a requested entity or because the inputs received were invalid.
    ///     On the other hand, exceptions should be used in exceptional circumstances that have
    ///     occurred due to factors outside of the domain model's control, e.g. network failures.
    ///     In general, problems in the domain model should be modelled as errors and exceptions should
    ///     be reserved for cases where the user cannot be given information on how to correct the problem.
    /// </remarks>
    public class Error
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Error" /> class.
        /// </summary>
        /// <param name="title">The title for this type of error.</param>
        /// <param name="detail">The detail that describes the error.</param>
        /// <returns>A new instance of <see cref="Error" />.</returns>
        public Error(string title, string detail)
        {
            Title = title;
            Detail = detail;
        }

        /// <summary>
        ///     Gets the detail that describes the error.
        /// </summary>
        public string Detail { get; }

        /// <summary>
        ///     Gets the title of the error. This should be the same for all instances of the same error type.
        /// </summary>
        public string Title { get; }
    }
}