namespace ShopZone.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        // Une catégorie peut avoir plusieurs produits(relation un-à-plusieurs)
        public ICollection<Product> Products { get; set; }
    }
}