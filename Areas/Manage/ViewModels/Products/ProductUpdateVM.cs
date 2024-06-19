using Pronia.Models;
using System.ComponentModel.DataAnnotations;

namespace Pronia.Areas.Manage.ViewModels
{
    public class ProductUpdateVM
    {
        public int Id { get; set; }
        [Required]
        public string Sku { get; set; } = null!;
        [Required]
        public string Name { get; set; } = null!;

        public string? Description { get; set; }
        [Range(1, 1000)]
        public decimal Price { get; set; }
        
        public IFormFile? MainPhoto { get; set; } 
        
        public IFormFile? HoverPhoto { get; set; } 

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }
        public IEnumerable<ProductImage> Images { get; set; } = new List<ProductImage>();
        public IEnumerable<Category>? Categories { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
        public ICollection<int> TagIds { get; set; } = null!;
        public IEnumerable<IFormFile>? AdditionalPhoto { get; set; }

        public IEnumerable<Color>? Colors { get; set; }
        public ICollection<int> ColorIds { get; set; } = null!;
        public ICollection<int>? ImageIds { get; set; } 
    }
}
