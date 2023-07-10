using System;

namespace MyBud.OrdersApi.Models.Request
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
