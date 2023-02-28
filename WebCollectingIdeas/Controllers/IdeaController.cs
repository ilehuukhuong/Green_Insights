using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace WebCollectingIdeas.Controllers
{
    public class IdeaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public IdeaController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
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
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );
            ViewBag.CategoryList = CategoryList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Idea obj, int id)
        {
            obj.IdentityUserId = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
            obj.TopicId = 1;
            _unitOfWork.Idea.Add(obj);
            _unitOfWork.Save();
            TempData["Success"] = "Create successfully";
            return RedirectToAction("index");
            if (ModelState.IsValid)
            {
                _unitOfWork.Idea.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
    }
}