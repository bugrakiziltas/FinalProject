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

            //productList[0].Category = categoryList[0];
            //productList[0].CategoryId = categoryList[0].Id;

            //productList[1].Category = categoryList[0];
            //productList[1].CategoryId = categoryList[0].Id;

            //productList[2].Category = categoryList[0];
            //productList[2].CategoryId = categoryList[0].Id;

            //productList[3].Category = categoryList[1];
            //productList[3].CategoryId = categoryList[1].Id;

            //productList[4].Category = categoryList[1];
            //productList[4].CategoryId = categoryList[1].Id;

            //productList[5].Category = categoryList[1];
            //productList[5].CategoryId = categoryList[1].Id;

            //productList[6].Category = categoryList[2];
            //productList[6].CategoryId = categoryList[2].Id;

            //productList[7].Category = categoryList[2];
            //productList[7].CategoryId = categoryList[2].Id;

            //productList[8].Category = categoryList[2];
            //productList[8].CategoryId = categoryList[2].Id;

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
                    Name = "Classic Leather Jacket",
                    Description = "Premium-quality leather jacket made from genuine top-grain leather for a rugged yet refined look.",
                    Price = 129.99M,
                    ImageUrl = "product-item-1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Slim Fit Chinos",
                    Description = "Versatile slim-fit chinos crafted from stretchable cotton for everyday comfort and style.",
                    Price = 49.99M,
                    ImageUrl = "product-item-2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Cotton Crewneck T-Shirt (5-Pack)",
                    Description = "Essential crewneck t-shirts pack made of breathable cotton, includes five vibrant colors for versatile everyday wear.",
                    Price = 39.99M,
                    ImageUrl = "product-item-3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Men's Running Sneakers",
                    Description = "Lightweight breathable running sneakers with cushioned soles and mesh upper for maximum comfort during workouts.",
                    Price = 89.99M,
                    ImageUrl = "product-item-4.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Analog Quartz Wrist Watch",
                    Description = "Precision quartz analog wristwatch with a genuine leather strap and water resistance up to 50 meters.",
                    Price = 159.99M,
                    ImageUrl = "product-item-5.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Stonewashed Denim Jeans",
                    Description = "Classic stonewashed denim jeans with a comfortable straight-leg fit and five-pocket styling.",
                    Price = 59.99M,
                    ImageUrl = "product-item-6.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Floral Summer Dress",
                    Description = "Lightweight floral print dress made of breathable fabric, perfect for summer outings and beach days.",
                    Price = 79.99M,
                    ImageUrl = "product-item-7.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                { 
                    Name = "High Waist Skinny Jeans",
                    Description = "Figure-hugging high-rise skinny jeans with a snug fit, made from stretch denim for comfort.",
                    Price = 69.99M,
                    ImageUrl = "product-item-8.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Silk Chiffon Scarf",
                    Description = "Elegant silk chiffon scarf with an abstract print that adds a pop of color and sophistication to any outfit.",
                    Price = 29.99M,
                    ImageUrl = "product-item-9.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Ankle Strap Sandals",
                    Description = "Comfortable ankle-strap sandals with cushioned footbeds and a low block heel, perfect for casual or dressy wear.",
                    Price = 49.99M,
                    ImageUrl = "product-item-10.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Cotton Button-Down Shirt",
                    Description = "Crisp cotton button-down shirt with a tailored fit, suitable for both office and casual occasions.",
                    Price = 39.99M,
                    ImageUrl = "post-image1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Handcrafted Leather Handbag",
                    Description = "Premium handcrafted leather handbag with an adjustable shoulder strap and multiple interior compartments.",
                    Price = 149.99M,
                    ImageUrl = "post-image2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Men's Leather Belt",
                    Description = "Classic black leather belt with a timeless silver-tone buckle, adjustable for a perfect fit.",
                    Price = 24.99M,
                    ImageUrl = "post-image3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Women's Gold Hoop Earrings",
                    Description = "Set of lightweight gold-plated hoop earrings that offer elegance and versatile style for any occasion.",
                    Price = 19.99M,
                    ImageUrl = "post-image4.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Unisex Baseball Cap",
                    Description = "Adjustable cotton baseball cap with an embroidered logo, breathable material for comfort during outdoor activities.",
                    Price = 14.99M,
                    ImageUrl = "post-image5.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Polarized Sunglasses",
                    Description = "UV-protected polarized sunglasses with a sturdy acetate frame and scratch-resistant lenses for clear vision.",
                    Price = 34.99M,
                    ImageUrl = "post-image6.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Wool Fedora Hat",
                    Description = "Classic wool fedora hat with a decorative band around the crown, ideal for adding a stylish touch to outfits.",
                    Price = 44.99M,
                    ImageUrl = "post-image7.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Silicone Sport Watch Band",
                    Description = "Durable silicone replacement band compatible with multiple smartwatch brands, easy to install and comfortable for workouts.",
                    Price = 17.99M,
                    ImageUrl = "post-image8.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Wireless Noise-Canceling Headphones",
                    Description = "Over-ear wireless headphones with Bluetooth 5.0, active noise cancellation, and 20-hour battery life.",
                    Price = 129.99M,
                    ImageUrl = "post-image9.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Smart Home Speaker",
                    Description = "Voice-controlled smart speaker with built-in virtual assistant, premium sound quality, and seamless smart home integration.",
                    Price = 89.99M,
                    ImageUrl = "insta-item1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "4K Action Camera",
                    Description = "Waterproof action camera capable of recording 4K video at 30fps, with a wide-angle lens and mounting accessories.",
                    Price = 249.99M,
                    ImageUrl = "insta-item2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Portable Bluetooth Speaker",
                    Description = "Compact waterproof Bluetooth speaker with deep bass, LED lights, and up to 12 hours of playtime.",
                    Price = 39.99M,
                    ImageUrl = "insta-item3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Fitness Tracker Watch",
                    Description = "Slim fitness tracker watch with heart rate monitor and sleep tracking functionality, water-resistant and lightweight.",
                    Price = 49.99M,
                    ImageUrl = "insta-item4.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Virtual Reality Headset",
                    Description = "Immersive virtual reality headset compatible with most smartphones, featuring adjustable straps and integrated head tracking.",
                    Price = 199.99M,
                    ImageUrl = "insta-item5.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Ceramic Table Lamp",
                    Description = "Modern ceramic table lamp with a linen fabric shade, providing warm ambient lighting to any room.",
                    Price = 59.99M,
                    ImageUrl = "insta-item6.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Decorative Throw Pillow",
                    Description = "Plush decorative throw pillow with a soft velvet cover and geometric embroidery, adds comfort and style to any sofa.",
                    Price = 24.99M,
                    ImageUrl = "banner-image-1.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Abstract Wall Art Print",
                    Description = "Colorful abstract print on a framed canvas, perfect for brightening up living room or office walls.",
                    Price = 79.99M,
                    ImageUrl = "banner-image-2.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Scented Candle Set",
                    Description = "Set of 3 scented soy candles in reusable glass jars, infused with lavender, vanilla, and sandalwood fragrances.",
                    Price = 29.99M,
                    ImageUrl = "banner-image-3.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Indoor Potted Plant",
                    Description = "Lush green Boston fern indoor plant in a ceramic pot, easy to care for and perfect for home or office decor.",
                    Price = 19.99M,
                    ImageUrl = "banner-image-4.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                },
                new Product
                {
                    Name = "Vintage Area Rug",
                    Description = "Soft vintage-style area rug with intricate patterns and a plush feel, adds character to any room floor.",
                    Price = 149.99M,
                    ImageUrl = "banner-image-5.jpg",
                    CreatedByUserId = Guid.Parse(adminUser.Id),
                    IdentityUserId = Guid.Parse(adminUser.Id),
                    CreatedOn = DateTimeOffset.UtcNow
                }
            };

            for (int i = 0; i < productList.Count; i++)
            {
                int categoryIndex = i / 6;

                if (categoryIndex < categoryList.Count)
                {
                    productList[i].Category = categoryList[categoryIndex];
                    productList[i].CategoryId = categoryList[categoryIndex].Id;
                }
            }


            context.Categories.AddRange(categoryList);
            context.Products.AddRange(productList);
            context.SaveChanges();
        }
    }
}
