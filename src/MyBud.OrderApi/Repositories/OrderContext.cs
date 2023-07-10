using Microsoft.EntityFrameworkCore;
using MyBud.OrdersApi.Models.Core;

namespace MyBud.OrdersApi.Repositories
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public DbSet<OrderEntity> Orders { get; set; }
        public DbSet<OrderItemEntity> OrderItems { get; set; }
    }
}
