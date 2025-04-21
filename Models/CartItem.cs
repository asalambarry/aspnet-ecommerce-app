using System.ComponentModel.DataAnnotations;

namespace ShopZone.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public string CartId { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;

        [Range(1, 100)]
        public int Quantity { get; set; }
        public DateTime DateCreated { get; set; }
    }
}