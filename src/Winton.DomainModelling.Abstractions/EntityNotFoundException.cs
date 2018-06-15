using System;

namespace Winton.DomainModelling
{
    /// <inheritdoc />
    /// <summary>
    ///     An error indicating that an entity could not be found.
    /// </summary>
    /// <typeparam name="TEntity">The type of entity.</typeparam>
    /// <typeparam name="TEntityId">The type of entity id.</typeparam>
    public class EntityNotFoundException<TEntity, TEntityId> : DomainException
        where TEntity : Entity<TEntityId>
        where TEntityId : IEquatable<TEntityId>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityNotFoundException{TEntity, TEntityId}" /> class.
        /// </summary>
        public EntityNotFoundException()
            : base($"The specified {typeof(TEntity).Name} could not be found.")
        {
        }
    }
}