using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
