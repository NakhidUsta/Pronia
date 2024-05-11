namespace Pronia.Models.ViewModels
{
    public class HomeViewModel
    {
       public IEnumerable<Slide> Slides { get; set; }=new List<Slide>();
       public IEnumerable<Product> ProductsFeatured { get; set; }= new List<Product>();
        public IEnumerable<Product> ProductsLatest { get; set; } = new List<Product>();
        public IEnumerable<Product> ProductsCheapest { get; set; }= new List<Product>();
    }
}
