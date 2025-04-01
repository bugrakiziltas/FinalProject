using FiinalProject.Entities;

namespace FinalProject.Entities
{
    public class Category : EntityBase<Guid>
    {
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; }

    }
}
