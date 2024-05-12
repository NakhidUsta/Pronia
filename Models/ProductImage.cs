using Pronia.Extencions.Enums;

namespace Pronia.Models
{
    public class ProductImage
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public ImageType Type { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}
