using FinalProject.Data;
using FinalProject.Entities;
using FinalProject.Interfaces;

using Microsoft.EntityFrameworkCore;
namespace FinalProject.Services
{
    public class ShoppingCartRepository : IShoppingCartRepository
    {
        private readonly ApplicationDbContext _context;
        public ShoppingCartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddToCart(ShoppingCart cart)
        {
            _context.ShoppingCarts.Add(cart);
            if (_context.SaveChanges() > 0) return true;
            return false;
        }
        public async Task<bool> ClearCart(Guid id)
        {
            var items=await GetShoppingCartItems(id);
            _context.ShoppingCarts.RemoveRange(items);
            if(_context.SaveChanges() > 0)return true;
            return false;
        }
        public async Task<IEnumerable<ShoppingCart>> GetShoppingCartItems(Guid id)
        {
            return await _context.ShoppingCarts.Where(c => c.IdentityUserId == id).Include(c => c.Product).ToListAsync();
        }
        public async Task<bool> RemoveFromShoppingCart(Guid productId, Guid userId)
        {
            var shoppingCartProduct= await _context.ShoppingCarts.FirstOrDefaultAsync(c => c.IdentityUserId == userId && c.Product.Id==productId);
            if (shoppingCartProduct != null)
            {
                _context.ShoppingCarts.Remove(shoppingCartProduct);
                if(_context.SaveChanges() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        public async Task<int> CountAsync(Guid id)
        {
           return await _context.ShoppingCarts.Where(x=>x.IdentityUserId == id).CountAsync();
        }
    }
}
