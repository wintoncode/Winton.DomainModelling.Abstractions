using System;

namespace Winton.DomainModelling
{
    public abstract class Entity<TEntityId> : IEquatable<Entity<TEntityId>>
        where TEntityId : IEquatable<TEntityId>
    {
        protected Entity(TEntityId id)
        {
            Id = id;
        }

        public TEntityId Id { get; }

        public static bool operator ==(Entity<TEntityId> left, Entity<TEntityId> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Entity<TEntityId> left, Entity<TEntityId> right)
        {
            return !Equals(left, right);
        }

        public bool Equals(Entity<TEntityId> other)
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

            return !Id.Equals(default(TEntityId)) && !other.Id.Equals(default(TEntityId)) && Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Entity<TEntityId>);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}