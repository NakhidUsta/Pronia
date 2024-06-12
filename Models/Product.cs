using Pronia.Extencions.Enums;

namespace Pronia.Models
{
    public class Product
    {
        public ImageType Type;

        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public ICollection<ProductImage> Images { get; set; } = new List<ProductImage> ();
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public ICollection<ProductTag> ProductTags { get; set; }=new List<ProductTag> ();
        public ICollection<ProductColor> ProductColors { get; set; } = new List<ProductColor>();
        public string Sku { get; set; }=null!;
    }
}
