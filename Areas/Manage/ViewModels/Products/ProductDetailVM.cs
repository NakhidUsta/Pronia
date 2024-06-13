using Pronia.Models;

namespace Pronia.Areas.Manage.ViewModels
{
    public class ProductDetailVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string Sku { get; set; } = null!;
        public decimal Price { get; set; }
        public ICollection<ProductImage> Images { get; set; } = null!;

        public string CategoryName { get; set; } = null!;
        public ICollection<Tag> Tags { get; set; } 
        public ICollection<Color> Colors { get; set; } 
        
    }
}
