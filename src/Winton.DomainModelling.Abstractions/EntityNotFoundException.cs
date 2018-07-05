// Copyright (c) Winton. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     An error indicating that an entity could not be found.
    /// </summary>
    public class EntityNotFoundException : DomainException
    {
        private EntityNotFoundException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///     Creates a new instance of the <see cref="EntityNotFoundException" /> class using the generic constraint to generate
        ///     the message.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <typeparam name="TEntityId">The type of the entity id.</typeparam>
        /// <returns>A new instance of <see cref="EntityNotFoundException" />.</returns>
        public static EntityNotFoundException Create<TEntity, TEntityId>()
            where TEntity : Entity<TEntityId>
            where TEntityId : IEquatable<TEntityId>
        {
            return new EntityNotFoundException($"The specified {typeof(TEntity).Name} could not be found.");
        }
    }
}