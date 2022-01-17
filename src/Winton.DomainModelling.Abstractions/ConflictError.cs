// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Winton.DomainModelling;

/// <inheritdoc />
/// <summary>
///     An error indicating that a conflicting entity exists.
/// </summary>
public class ConflictError : Error
{
    /// <summary>
    ///     Initializes a new instance of the <see cref="ConflictError" /> class.
    /// </summary>
    /// <param name="detail">The detail that describes the error.</param>
    /// <returns>A new instance of <see cref="ConflictError" />.</returns>
    public ConflictError(string detail)
        : base("Conflict", detail)
    {
    }
}