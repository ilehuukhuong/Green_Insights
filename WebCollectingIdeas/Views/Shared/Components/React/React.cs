using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Views.Shared.Components.React
{
    public class React : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        public React(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IViewComponentResult Invoke(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            ViewBag.IdeaId = id;
            return View("_PartialReact", objView);
        }
    }
}

