using DotNetECommerce.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Entities
{
    public class Order:DotNetECommerce.Domain.Entities.EntityBase<Guid>
    {
        public List<Product> BuyedProducts { get; set; }
        public decimal Total { get; set; }
        public Guid IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
