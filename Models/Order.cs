using System.ComponentModel.DataAnnotations;

namespace ShopZone.Models
{
    public class Order
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        [Required]
        public string Status { get; set; } = string.Empty;

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}