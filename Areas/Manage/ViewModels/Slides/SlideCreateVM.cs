using System.ComponentModel.DataAnnotations.Schema;

namespace Pronia.Areas.Manage.ViewModels
{
    public class SlideCreateVM
    {

        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string SubTitle { get; set; } = null!;
    
        public string ButtonText { get; set; } = null!;
        public int Order { get; set; }
      
        public IFormFile Photo { get; set; } = null!;

    }
}
