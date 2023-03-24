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
            var obj = _unitOfWork.Topic.GetFirstOrDefault(i => i.Id == id);
            return View("_PartialIdea",obj);
        }
    }
}
