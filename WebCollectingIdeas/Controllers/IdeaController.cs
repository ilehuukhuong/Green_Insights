using CollectingIdeas.DataAccess.Data;
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
        private readonly UserManager<IdentityUser> _userManager;
        public IdeaController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            IEnumerable<Topic> objTopicList = _unitOfWork.Topic.GetAll();
            return View(objTopicList);
        }        
        public ActionResult Detail(int id)
        {
            var obj = _unitOfWork.Idea.GetFirstOrDefault(x => x.Id == id);
            return View(obj);
        }
        public IActionResult View(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var obj = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            };
            return View(obj);
        }
        public IActionResult Create(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(
                    u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );
            ViewBag.TopicId = id;
            ViewBag.CategoryList = CategoryList;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("TopicId,Title,Description,CategoryId")] Idea obj)
        {
            obj.Id = 0;
            obj.IdentityUserId = _userManager.GetUserId(HttpContext.User);
            if (ModelState.IsValid)
            {
                _unitOfWork.Idea.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("View", "Idea", new { @id = obj.TopicId });
            }
            TempData["Deleted"] = "Create failed";
            return RedirectToAction("Create", "Idea", new { @id = obj.TopicId });
        }
    }
}