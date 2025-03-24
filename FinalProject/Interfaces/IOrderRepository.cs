using FinalProject.Entities;

namespace FinalProject.Interfaces
{
    public interface IOrderRepository
    {
        Task<bool> AddOrder(Order order);
        Task<IEnumerable<Order>> GetOrdersByUserId(Guid id);
    }
}
