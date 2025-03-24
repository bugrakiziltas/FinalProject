using FinalProject.Entities;
using FinalProject.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Data
{
    public class SeedData
    {
        public static async Task SeedAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            if (await context.Users.AnyAsync())
            {
                return;
            }
            if (!await roleManager.RoleExistsAsync(SD.Role_Admin))
            {
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Admin));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Crm));
                await roleManager.CreateAsync(new IdentityRole(SD.Role_Cust));
            }
            var adminEmail = "admin@gmail.com";
            var adminUser = await userManager.FindByNameAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser { UserName = adminEmail, Email = adminEmail };
                var createAdminResult = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, SD.Role_Admin);
                }
            }
            var crmEmail = "crm@gmail.com";
            var crmUser= await userManager.FindByNameAsync(crmEmail);
            if (crmUser == null)
            {
                crmUser = new IdentityUser { UserName = crmEmail, Email = crmEmail };
                var createCrmResult = await userManager.CreateAsync(crmUser, "Crm123!");
                if (createCrmResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(crmUser, SD.Role_Crm);
                }
            }
            var custEmail = "cust@gmail.com";
            var customer = await userManager.FindByNameAsync(custEmail);
            if (customer == null)
            {
                customer = new IdentityUser { UserName = custEmail, Email = custEmail };
                var createCustResult = await userManager.CreateAsync(customer, "Cust123!");
                if (createCustResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer, SD.Role_Cust);
                }
            }
            var productList = new List<Product>()
            {
                new Entities.Product
                {
                    Name="furniture1",
                    Description="Godlike furniture1",
                    Price=200,
                    ImageUrl="furniture1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUser = adminUser,
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                },
                new Entities.Product
                {
                    Name="furniture2",
                    Description="Godlike furniture2",
                    Price=200,
                    ImageUrl="furniture2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUser = adminUser,
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                },
                new Entities.Product
                {
                    Name="furniture2",
                    Description="Godlike furniture2",
                    Price=200,
                    ImageUrl="furniture3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUser = adminUser,
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                },
            };
            context.Products.AddRange(productList);
            context.SaveChanges();
        }
    }
}
