using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class Error404Controller : Controller
    {
        [Route("error/404")]
        public IActionResult Error404()
        {
            return View("Error404");
        }

    }
}
