using Microsoft.AspNetCore.Mvc;

namespace Pronia.Models
{
    public class BasketItem
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public Product Product { get; set; } = null!;
        public int ProductId { get; set; }
        public AppUser AppUser { get; set; } = null!;
        public string AppUserId { get; set; } = null!;
    }
}
