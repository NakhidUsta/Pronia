using Pronia.Models;

namespace Pronia.Areas.Manage.ViewModels
{
    public class CategoryIndexVm
    {
        public IEnumerable<Category> Categories=new List<Category>();
        public int CurrentPage { get; set; }
        public int TotalPage { get; set; }
    }
}
