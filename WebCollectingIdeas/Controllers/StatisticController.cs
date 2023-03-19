using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class StatisticController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
