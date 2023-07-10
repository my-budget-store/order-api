using MyBud.OrdersApi.Models.Request;

namespace MyBud.OrdersApi.Interfaces
{
    public interface IOrderRepository
    {
        Task<Models.Core.OrderEntity?> GetOrderById(int OrderId);
        Task<IQueryable<Models.Core.OrderEntity>> GetOrders();
        Task<IEnumerable<Models.Core.OrderEntity>> SearchOrders(int year);
        Task<bool> CreateOrder(List<OrderItem> Order, Guid _userId);
        Task<Models.Core.OrderEntity> UpdateOrder(Models.Core.OrderEntity Order);
        Task<bool> DeleteOrder(Models.Core.OrderEntity Order);
    }
}
