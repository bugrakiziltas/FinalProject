using FinalProject.Entities;

namespace FinalProject.Interfaces
{
    public interface IShoppingCartRepository
    {
        Task<bool> AddToCart(ShoppingCart cart);
        Task<IEnumerable<ShoppingCart>> GetShoppingCartItems(Guid id);
        Task<bool> RemoveFromShoppingCart(Guid productId, Guid userId);
        Task<bool> ClearCart(Guid id);
    }
}
