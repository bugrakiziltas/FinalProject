namespace DotNetECommerce.Domain.Common
{
    public interface IEntityBase<T>
    {
        public T Id { get; set; }
    }
}
