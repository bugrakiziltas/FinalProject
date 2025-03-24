
using DotNetECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Entities
{
    public class Product : EntityBase<Guid>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }


        public Guid IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }


        //public Guid CategoryId { get; set; }
        //public Category Category { get; set; }


    }
}
