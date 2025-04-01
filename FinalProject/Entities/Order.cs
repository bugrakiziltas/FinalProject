
using FiinalProject.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Entities
{
    public class Order: EntityBase<Guid>
    {
        public List<Product> BuyedProducts { get; set; }
        public decimal Total { get; set; }
        public Guid IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
