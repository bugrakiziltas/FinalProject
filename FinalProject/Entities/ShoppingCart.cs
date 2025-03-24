using DotNetECommerce.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace FinalProject.Entities
{
    public class ShoppingCart : EntityBase<Guid>
    {
        public Guid ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever]
        public Product Product { get; set; }
        public Guid IdentityUserId { get; set; }
        public IdentityUser IdentityUser { get; set; }
    }
}
