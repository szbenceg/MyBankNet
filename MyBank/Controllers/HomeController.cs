using Microsoft.AspNetCore.Mvc;

namespace MyBank.Controllers
{
    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
