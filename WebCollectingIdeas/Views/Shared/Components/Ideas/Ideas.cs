using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Views.Shared.Components.Ideas
{
    public class Ideas : ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public Ideas(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IViewComponentResult Invoke(int id)
        {
            IEnumerable<Idea> objIdeasList = _unitOfWork.Idea.GetAll();
            ViewBag.TopicId = id;
            return View("_PartialIdea",objIdeasList);
        }
    }
}
