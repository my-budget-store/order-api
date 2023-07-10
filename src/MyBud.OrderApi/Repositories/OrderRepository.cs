using Microsoft.EntityFrameworkCore;
using MyBud.OrdersApi.Interfaces;
using MyBud.OrdersApi.Models.Core;
using MyBud.OrdersApi.Models.Request;

namespace MyBud.OrdersApi.Repositories
{
    //TODO: modify "template" variable names as per your use case
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderContext _context;

        public OrderRepository(OrderContext context)
        {
            _context = context;
        }

        public async Task<Models.Core.OrderEntity?> GetOrderById(int templateId)
        {
            var template = await _context.Orders.FindAsync(templateId);

            return template;
        }

        public async Task<IQueryable<OrderEntity>> GetOrders()
        {
            var orders = await _context.Orders
                .Include(order => order.OrderItems) // Include the OrderItems navigation property
                .ToListAsync();

            return orders.AsQueryable();
        }

        public async Task<bool> CreateOrder(List<OrderItem> orderItems, Guid userId)
        {
            var orderItemEntities = orderItems.Select(item => new OrderItemEntity()
            {
                OrderItemId = item.OrderItemId,
                ProductId = item.ProductId,
                Quantity = item.Quantity,
            }).ToList();

            var order = new OrderEntity()
            {
                UserId = userId,
                OrderPlacedDate = DateTime.UtcNow,
                OrderItems = orderItemEntities
            };

            await _context.Orders.AddAsync(order);
            await _context.OrderItems.AddRangeAsync(orderItemEntities);

            _context.SaveChanges();

            return true;
        }

        public Task<IEnumerable<OrderEntity>> SearchOrders(int year)
        {
            var templates = _context.Orders.Where(p => p.OrderPlacedDate.Year == year).AsEnumerable();

            return Task.FromResult(templates);
        }

        public async Task<Models.Core.OrderEntity> UpdateOrder(Models.Core.OrderEntity template)
        {
            var existingOrder = await _context.Orders.FindAsync(template.OrderId);

            if (existingOrder != null)
                _context.Update(existingOrder);

            await _context.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<Models.Core.OrderEntity> UpdateOrderPrice(Models.Core.OrderEntity template)
        {
            var existingOrder = await _context.Orders.FindAsync(template.OrderId);

            if (existingOrder != null)
            {
                //ToDo: Implement Logic here
                _context.Update(existingOrder);
            }

            await _context.SaveChangesAsync();

            return existingOrder;
        }

        public async Task<bool> DeleteOrder(Models.Core.OrderEntity template)
        {
            var templatestate = _context.Remove(template);
            await _context.SaveChangesAsync();

            if (templatestate.State.Equals(Microsoft.EntityFrameworkCore.EntityState.Deleted))
            {
                return true;
            }

            return false;
        }
    }
}
