// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Winton.DomainModelling;

/// <inheritdoc cref="IEquatable{Unit}" />
/// <inheritdoc cref="IComparable{Unit}" />
/// /// <inheritdoc cref="IComparable" />
/// <summary>
///     Represents a void type, since <see cref="T:System.Void" /> is not a valid return type in C#.
/// </summary>
public struct Unit : IEquatable<Unit>, IComparable<Unit>, IComparable
{
    /// <summary>
    ///     Gets the default and only value of the <see cref="Unit" /> type.
    /// </summary>
    public static Unit Value { get; } = default;

    /// <summary>
    ///     Determines whether the first <cref see="Unit" /> is equal to the second <cref see="Unit" />.
    /// </summary>
    /// <param name="first">
    ///     The first <cref see="Unit" />.
    /// </param>
    /// <param name="second">
    ///     The second <cref see="Unit" />.
    /// </param>
    /// <returns>
    ///     <c>true</c>, as <cref see="Unit" /> only has a single value and all instances are therefore equal.
    /// </returns>
    public static bool operator ==(Unit first, Unit second)
    {
        return true;
    }

    /// <summary>
    ///     Determines whether the first <cref see="Unit" /> is not equal to the second <cref see="Unit" />.
    /// </summary>
    /// <param name="first">
    ///     The first <cref see="Unit" />.
    /// </param>
    /// <param name="second">
    ///     The second <cref see="Unit" />.
    /// </param>
    /// <returns>
    ///     <c>false</c>, as <cref see="Unit" /> only has a single value and all instances are therefore equal.
    /// </returns>
    public static bool operator !=(Unit first, Unit second)
    {
        return false;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Compares the current object with another object of the same type.
    /// </summary>
    /// <param name="other">
    ///     An object to compare with this object.
    /// </param>
    /// <returns>
    ///     Zero, as <cref see="Unit" /> only has a single value and all instances are therefore equal.
    /// </returns>
    public int CompareTo(Unit other)
    {
        return 0;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Determines whether this <cref see="Unit" /> is equal to another <cref see="Unit" />.
    /// </summary>
    /// <param name="other">
    ///     A <cref see="Unit" /> to compare with this <cref see="Unit" />.
    /// </param>
    /// <returns>
    ///     <c>true</c>, as <cref see="Unit" /> only has a single value and all instances are therefore equal.
    /// </returns>
    public bool Equals(Unit other)
    {
        return true;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is Unit;
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return 0;
    }

    /// <inheritdoc />
    /// <summary>
    ///     Compares the current object with another object.
    /// </summary>
    /// <param name="obj">
    ///     A <cref see="Unit" /> to compare with this <cref see="Unit" />.
    /// </param>
    /// <returns>
    ///     Zero, as <cref see="Unit" /> only has a single value and all instances are therefore equal.
    /// </returns>
    int IComparable.CompareTo(object? obj)
    {
        return 0;
    }
}
