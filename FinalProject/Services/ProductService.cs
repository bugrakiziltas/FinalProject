using FinalProject.Data;
using FinalProject.Dtos.Product;
using FinalProject.Entities;
using FinalProject.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FinalProject.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IIdentityService _identityService;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductService(ApplicationDbContext applicationDbContext, IIdentityService identityService, UserManager<IdentityUser> userManager)
        {
            _applicationDbContext = applicationDbContext;
            _identityService = identityService;
            _userManager = userManager;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            var products = await _applicationDbContext.Products.ToListAsync();

            return products;
        }


        public async Task<bool> CreateProductAsync(CreateProductDto createProductDto, string createdByUserId, string imageUrl)
        {
            var applicationUser = await _identityService.GetUserByIdAsync(createdByUserId);

            Product product = new()
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                ImageUrl = imageUrl,
                Price = createProductDto.Price,
                CreatedByUserId = Guid.Parse(applicationUser.Id),
                IdentityUser = applicationUser,
                IdentityUserId = Guid.Parse(applicationUser.Id),
                CreatedOn = DateTimeOffset.UtcNow,
            };

            _applicationDbContext.Add(product);
            if (_applicationDbContext.SaveChanges() > 0)
            {
                return true;
            }

            return false;
        }

        public async Task<bool> UpdateProductAsync(UpdateProductDto updateProductDto, string imageUrl, string createdByUserId, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products.Where(x => x.Id == updateProductDto.Id).SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return false;
            }

            var applicationUser = await _identityService.GetUserByIdAsync(createdByUserId);

            if (applicationUser == null)
            {
                return false;
            }
            if(imageUrl != String.Empty)
            {
                string uploadsFolder = Path.Combine("wwwroot/Images");
                string serverFolder = Path.Combine(uploadsFolder, imageUrl);

                using (var stream = new FileStream(serverFolder, FileMode.Create))
                {
                    updateProductDto.Image.CopyTo(stream);
                }
                product.ImageUrl = imageUrl;
            }
            product.Name = updateProductDto.Name;
            product.Price = updateProductDto.Price;
            product.Description = updateProductDto.Description;

            _applicationDbContext.Update(product);
            _applicationDbContext.SaveChanges();

            return true;
        }

        public async Task<bool> DeleteProductAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products.Where(x => x.Id == productId).SingleOrDefaultAsync(cancellationToken);

            if (product == null)
            {
                return false;
            }

            _applicationDbContext.Remove(product);
            _applicationDbContext.SaveChanges();

            return true;
        }


        public async Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken)
        {
            var product = await _applicationDbContext.Products.Where(x => x.Id == productId).SingleOrDefaultAsync();

            return product;
        }

        //public async Task<IEnumerable<Meeting>> GetMeetingsByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

    }
}
