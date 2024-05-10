using Microsoft.AspNetCore.Mvc;

namespace Pronia.Controllers
{
    public class HomeController:Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
