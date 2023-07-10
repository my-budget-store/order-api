using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyBud.OrdersApi.Models.Core
{
    public class OrderItemEntity
    {
        // Foreign key property
        [JsonIgnore]
        public int OrderId { get; set; }

        [JsonIgnore]
        public OrderEntity Order { get; set; }
        [Key]
        public int OrderItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

    }
}
