using Microsoft.AspNetCore.Mvc;

namespace IdentityServer.Controllers
{
    public class HomeABCController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
