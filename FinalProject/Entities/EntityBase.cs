using DotNetECommerce.Domain.Common;

namespace DotNetECommerce.Domain.Entities
{
    public abstract class EntityBase<TKey> : IEntityBase<TKey>
    {
        public virtual TKey Id { get; set; }

        public Guid? CreatedByUserId { get; set; }
        public DateTimeOffset? CreatedOn { get; set; }

        public DateTimeOffset? ModifiedOn { get; set; }
        public string? ModifiedByUserId { get; set; }

        public string? DeletedByUserId { get; set; }
        public DateTimeOffset? DeletedOn { get; set; }
        public bool IsDeleted { get; set; }
    }
}

