// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     An error indicating that the action being performed is not authorized.
    /// </summary>
    public class UnauthorizedError : Error
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="UnauthorizedError" /> class.
        /// </summary>
        /// <param name="detail">The detail that describes the error.</param>
        /// <returns>A new instance of <see cref="UnauthorizedError" />.</returns>
        public UnauthorizedError(string detail)
            : base("Unauthorized", detail)
        {
        }
    }
}