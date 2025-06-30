using FinalProject.Dtos.Product;
using FinalProject.Entities;

namespace FinalProject.Interfaces
{
    public interface IProductService
    {
        //Task<bool> CreateProductAsync(CreateProductDto product, CancellationToken cancellationToken = default);
        Task<bool> CreateProductAsync(CreateProductDto product, string createdByUserId, string imageUrl);
        //Task<bool> UpdateProductAsync(UpdateProductDto product, Guid id, CancellationToken cancellationToken = default);
        Task<bool> UpdateProductAsync(UpdateProductDto product, string imageUrl, string createdByUserId, CancellationToken cancellationToken = default);
        Task<bool> DeleteProductAsync(Guid productId, CancellationToken cancellationToken = default);
        Task<Product> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
        //Task<IEnumerable<Product>> GetMeetingsByUserIdAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<List<Category>> GetAllCategoriesAsync();
    }
}
