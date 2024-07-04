using System.ComponentModel.DataAnnotations;

namespace Pronia.Models.ViewModels
{
    public class LoginVM
    {
        [Required]
        public string UserOrEmail { get; set; }=null!;
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
        [Required]
        public bool RememberMe { get; set; }
    }
}
