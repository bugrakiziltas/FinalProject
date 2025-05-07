using FinalProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<MessageModel> MessageModels { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<MessageModel>().HasOne(vm => vm.Sender).WithMany().HasForeignKey(vm => vm.SenderId).OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<MessageModel>().HasOne(vm => vm.Receiver).WithMany().HasForeignKey(vm => vm.ReceiverId).OnDelete(DeleteBehavior.NoAction);
        }
    }
}
