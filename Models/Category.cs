using System.ComponentModel.DataAnnotations;

namespace Pronia.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        public string Name { get; set; } = null!;
        public ICollection<Product> Products { get; set; }=new List<Product>();
    }
}
