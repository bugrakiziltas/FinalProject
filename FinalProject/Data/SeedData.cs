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
            var crmUser = await userManager.FindByNameAsync(crmEmail);
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
            var custEmail2 = "cust2@gmail.com";
            var customer2 = await userManager.FindByNameAsync(custEmail2);
            if (customer2 == null)
            {
                customer2 = new IdentityUser { UserName = custEmail2, Email = custEmail2 };
                var createCustResult = await userManager.CreateAsync(customer2, "Cust123!");
                if (createCustResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(customer2, SD.Role_Cust);
                }
            }
            //var productList = new List<Product>()
            //{
            //    new Product
            //    {
            //        Name="Man Tshirt",
            //        Description="Man Tshirt",
            //        Price=200,
            //        ImageUrl="product-item-1.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Man Pant",
            //        Description="Man Pant",
            //        Price=200,
            //        ImageUrl="product-item-2.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Man Sweatshirt",
            //        Description="Man SweatShirt",
            //        Price=200,
            //        ImageUrl="product-item-3.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },new Product
            //    {
            //        Name="Woman Tshirt",
            //        Description="Woman Tshirt",
            //        Price=200,
            //        ImageUrl="product-item-4.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Woman Dress",
            //        Description="Woman Dress",
            //        Price=200,
            //        ImageUrl="product-item-5.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Woman Shoes",
            //        Description="Woman Shoes",
            //        Price=200,
            //        ImageUrl="product-item-6.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },new Product
            //    {
            //        Name="Watch",
            //        Description="Watch",
            //        Price=200,
            //        ImageUrl="product-item-7.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Necklace",
            //        Description="Necklace",
            //        Price=200,
            //        ImageUrl="product-item-8.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //    new Product
            //    {
            //        Name="Earrings",
            //        Description="Earrings",
            //        Price=200,
            //        ImageUrl="product-item-9.jpg",
            //        CreatedByUserId = Guid.Parse(adminUser.Id),
            //        IdentityUser = adminUser,
            //        IdentityUserId = Guid.Parse(adminUser.Id),
            //        CreatedOn = DateTimeOffset.UtcNow,
            //    },
            //};

            //var categoryList = new List<Category>()
            //{
            //        new Category
            //        {
            //            CategoryName = "Man",
            //            Products = new List<Product> { productList[0],productList[1],productList[2] },
            //            CreatedByUserId = Guid.Parse(adminUser.Id),
            //            CreatedOn = DateTimeOffset.UtcNow,

            //        },
            //        new Category
            //        {
            //            CategoryName = "Woman",
            //            Products = new List<Product> { productList[3],productList[4],productList[5] },
            //            CreatedByUserId = Guid.Parse(adminUser.Id),


            //        },
            //        new Category
            //        {
            //            CategoryName = "Accessories",
            //            Products = new List<Product> { productList[6], productList[7], productList[8] } ,
            //            CreatedByUserId = Guid.Parse(adminUser.Id),

            //        },
            //};


            var categoryList = new List<Category>()
            {
                new Category
                {
                    CategoryName = "Man",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                    Products = new List<Product>()
                },
                new Category
                {
                    CategoryName = "Woman",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                    Products = new List<Product>()
                },
                new Category
                {
                    CategoryName = "Accessories",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                    Products = new List<Product>()
                },
                new Category
                {
                    CategoryName = "Electronics",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                    Products = new List<Product>()
                },
                new Category
                { 
                    CategoryName = "Home Decor",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow,
                    Products = new List<Product>()
                }
            };

            var productList = new List<Product>()
            {
                new Product
                {
                    Name = "Hat for Man",
                    Description = "",
                    Price = 129.99M,
                    ImageUrl = "product-item-1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Bag for Woman",  
                    Description = "",
                    Price = 49.99M,
                    ImageUrl = "product-item-7.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Hat for Woman",
                    Description = "",
                    Price = 39.99M,
                    ImageUrl = "product-item-10.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "T-Shirt for Men",
                    Description = "",
                    Price = 89.99M,
                    ImageUrl = "product-item-4.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Men Shirt",
                    Description = "",
                    Price = 59.99M,
                    ImageUrl = "wishlist-item3-resize.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Floral Summer Dress",
                    Description = "",
                    Price = 79.99M,
                    ImageUrl = "product-item-5.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                { 
                    Name = "High Waist Skinny Jeans",
                    Description = "",
                    Price = 69.99M,
                    ImageUrl = "product-item-2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Denim Pant",
                    Description = "",
                    Price = 79.99M,
                    ImageUrl = "product-item-3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                }
            };


            productList[0].Category = categoryList[2];
            productList[0].CategoryId = categoryList[2].Id;

            productList[1].Category = categoryList[2];
            productList[1].CategoryId = categoryList[2].Id;

            productList[2].Category = categoryList[2];
            productList[2].CategoryId = categoryList[2].Id;

            productList[3].Category = categoryList[0];
            productList[3].CategoryId = categoryList[0].Id;

            productList[4].Category = categoryList[0];
            productList[4].CategoryId = categoryList[0].Id;

            productList[5].Category = categoryList[0];
            productList[5].CategoryId = categoryList[0].Id;

            productList[6].Category = categoryList[1];
            productList[6].CategoryId = categoryList[1].Id;

            productList[7].Category = categoryList[1];
            productList[7].CategoryId = categoryList[1].Id;

            context.Categories.AddRange(categoryList);
            context.Products.AddRange(productList);
            context.SaveChanges();
        }
    }
}
