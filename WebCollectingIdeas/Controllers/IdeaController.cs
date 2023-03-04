using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using CollectingIdeas.Models.ViewModel;
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
        private IWebHostEnvironment _webHostEnvironment;
        public IdeaController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            IEnumerable<Topic> objTopicList = _unitOfWork.Topic.GetAll();
            return View(objTopicList);
        }
        public ActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var obj = _unitOfWork.Idea.GetFirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            };
            ViewBag.CategoryName = _unitOfWork.Category.GetFirstOrDefault(x => x.Id == obj.CategoryId).Name;
            ViewBag.TopicName = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == obj.TopicId).Name;
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
            IdeaVM ideaVM = new IdeaVM();
            ideaVM.idea = new Idea();
            ideaVM.TopicList = _unitOfWork.Topic.GetAll().Select(
                    u => new SelectListItem()
                    {
                        Text = u.Name,
                        Value = u.Id.ToString()
                    }
                );
            ideaVM.CategoryList = _unitOfWork.Category.GetAll().Select(
                u => new SelectListItem()
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            ViewBag.TopicId = id;
            return View(ideaVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdeaVM obj, IFormFile? file)
        {
            obj.idea.IdentityUserId = _userManager.GetUserId(HttpContext.User);
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var userMail = User.FindFirstValue(ClaimTypes.Email);
                    string uploads = Path.Combine(wwwRootPath, @"file/topic_" + obj.idea.TopicId + @"/" + userMail + @"_" + obj.idea.Title);
                    if (!Directory.Exists(uploads))
                    {
                        Directory.CreateDirectory(uploads);
                    }
                    var extension = Path.GetExtension(file.FileName);
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.idea.Path = @"file/topic_" + obj.idea.TopicId + @"/" + userMail + @"_" + obj.idea.Title + @"/" + fileName + extension;
                }
                _unitOfWork.Idea.Add(obj.idea);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("View", "Idea", new { @id = obj.idea.TopicId });
            }
            TempData["Deleted"] = "Create failed";
            return RedirectToAction("Create", "Idea", new { @id = obj.idea.TopicId });
        }
    }
}