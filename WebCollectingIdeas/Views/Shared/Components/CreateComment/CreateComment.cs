using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Views.Shared.Components.CreateComment
{
    public class CreateComment : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CreateComment(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int id)
        {
            Comment comment = new Comment();
            ViewBag.IdeaId = id;
            return View("_PartialCreateComment", comment);
        }
    }
}