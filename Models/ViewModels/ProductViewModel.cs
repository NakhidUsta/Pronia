namespace Pronia.Models.ViewModels
{
    public class ProductViewModel
    {
        public IEnumerable<Product> ProductRelated { get; set; } = new List<Product>();
        public Product ProductDetail { get; set; } = null!;
    }
}
