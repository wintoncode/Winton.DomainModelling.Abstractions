// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Winton.DomainModelling;

/// <inheritdoc />
/// <summary>
///     An error indicating that an entity could not be found.
/// </summary>
public class NotFoundError : Error
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="NotFoundError" /> class.
    /// </summary>
    /// <param name="detail">The detail that describes the error.</param>
    /// <returns>A new instance of <see cref="NotFoundError" />.</returns>
    public NotFoundError(string detail)
        : base("Not found", detail)
    {
    }
}
