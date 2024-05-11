namespace Pronia.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string ImagePrimary { get; set; } = null!;
        public string ImageSecondary { get; set; } = null!;
    }
}
