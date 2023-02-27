using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class TopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Topic> objTopicList = _unitOfWork.Topic.GetAll();
            return View(objTopicList);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Topic obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Topic.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var topicformDb = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            if (topicformDb == null)
            {
                return NotFound();
            }
            return View(topicformDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Topic obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Topic.Update(obj);
                _unitOfWork.Save();
                TempData["Edited"] = "Edit successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var topicformDb = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            if (topicformDb == null)
            {
                return NotFound();
            }
            return View(topicformDb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Topic.Remove(obj);
                _unitOfWork.Save();
                TempData["Deleted"] = "Delete successfully";
                return RedirectToAction("index");
            }
        }
    }
}
