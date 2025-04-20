namespace ShopZone.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }

        // Une commande peut avoir plusieurs articles de commande (relation un-à-plusieurs)
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}