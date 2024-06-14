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
        [Required]
        public IFormFile? MainPhoto { get; set; } 
        [Required]
        public IFormFile? HoverPhoto { get; set; } 

        [Range(1, int.MaxValue)]
        public int CategoryId { get; set; }

        public IEnumerable<Category>? Categories { get; set; } 
        //public IEnumerable<Tag>? Tags { get; set; }
        //public IEnumerable<int> TagIds { get; set; } = null!;
        //public IEnumerable<IFormFile> AdditionalPhoto { get; set; } = null!;

        //public IEnumerable<Color>? Colors { get; set; }
        //public IEnumerable<int> ColorIds { get; set; } = null!;
    }
}
