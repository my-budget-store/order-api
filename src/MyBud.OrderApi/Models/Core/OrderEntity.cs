using System;
using System.ComponentModel.DataAnnotations;

namespace MyBud.OrdersApi.Models.Core
{
    public class OrderEntity
    {
        [Key]
        public int OrderId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderPlacedDate { get; set; }

        // Navigation property for the collection of ordered items
        public ICollection<OrderItemEntity> OrderItems { get; set; }
    }
}
