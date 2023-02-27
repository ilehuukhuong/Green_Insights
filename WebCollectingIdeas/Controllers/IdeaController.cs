using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebCollectingIdeas.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public IdeaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Topic> objTopicList = _unitOfWork.Topic.GetAll();
            return View(objTopicList);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var item = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            if (item == null)
            {
                return NotFound();
            }
            return View(item);
        }
        //public ActionResult Edit(int id)
        //{
        //    var item = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
        //    return View(item);
        //}
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Idea obj,int id)
        {
            if (ModelState.IsValid)
            {
                obj.IdentityUserId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
                _unitOfWork.Idea.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
    }
}
