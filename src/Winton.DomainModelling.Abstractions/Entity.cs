// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     A base class to implement entity types, which are defined by their identity rather than their attributes.
    /// </summary>
    /// <typeparam name="TEntityId">The ID type for the entity type.</typeparam>
    public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>
        where TEntityId : IEquatable<TEntityId>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Entity{TEntityId}" /> class.
        /// </summary>
        /// <param name="id">The ID for the entity.</param>
        protected Entity(TEntityId id)
        {
            Id = id;
        }

        /// <summary>
        ///     Gets the ID for the entity.
        /// </summary>
        public TEntityId Id { get; }

        /// <summary>
        ///     Indicates whether two entities are equal.
        /// </summary>
        /// <param name="left">The first entity.</param>
        /// <param name="right">The second entity.</param>
        /// <returns><see langword="true" /> if the entities have the same ID; otherwise, <see langword="false" />.</returns>
        public static bool operator ==(Entity<TEntityId>? left, Entity<TEntityId>? right)
        {
            return Equals(left, right);
        }

        /// <summary>
        ///     Indicates whether two entities are unequal.
        /// </summary>
        /// <param name="left">The first entity.</param>
        /// <param name="right">The second entity.</param>
        /// <returns><see langword="true" /> if the entities have different IDs; otherwise, <see langword="false" />.</returns>
        public static bool operator !=(Entity<TEntityId>? left, Entity<TEntityId>? right)
        {
            return !Equals(left, right);
        }

        /// <inheritdoc />
        /// <summary>
        ///     Indicates whether the current entity is equal to another entity.
        /// </summary>
        /// <param name="other">An entity to compare with this entity.</param>
        /// <returns>
        ///     <see langword="true" /> if the current entity has the same ID as the <paramref name="other" /> entity;
        ///     otherwise, <see langword="false" />.
        /// </returns>
        public bool Equals(Entity<TEntityId>? other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            if (GetType() != other.GetType())
            {
                return false;
            }

            return !Id.Equals(default!) && !other.Id.Equals(default!) && Id.Equals(other.Id);
        }

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>
        ///     <see langword="true" /> if the specified object is equal to the current object; otherwise,
        ///     <see langword="false" />.
        /// </returns>
        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TEntityId>);
        }

        /// <summary>
        ///     Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current object.</returns>
        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}