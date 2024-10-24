using Microsoft.AspNetCore.Mvc;

namespace ClaimApp.Controllers
{
    public class HomeController : Controller
    {
        // Action to display the landing page
        public IActionResult Index()
        {
            return View();
        }
    }
}
