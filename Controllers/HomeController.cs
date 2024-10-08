using Microsoft.AspNetCore.Mvc;

namespace PinThePlace.Controllers 
{
    public class HomeController : Controller
    {
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }
    }
}