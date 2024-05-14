namespace Pronia.Models
{
    public class ProductTag
    {
        public int Id { get; set; }
        public int TagId { get; set; }
        public int ProductId { get; set; }
        public Tag Tag { get; set; } = null!;
        public Product Product { get; set; } = null!; 
    }
}
