namespace Pronia.Models.ViewModels
{
    public class BasketItemVm
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Image { get; set; } = null!; 
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int Count;
    }
}
