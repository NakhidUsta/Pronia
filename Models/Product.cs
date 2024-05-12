namespace Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage> ();
    }
}
