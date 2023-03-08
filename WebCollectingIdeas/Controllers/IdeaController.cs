using ClosedXML.Excel;
using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using CollectingIdeas.Models.ViewModel;
using Ionic.Zip;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mime;
using System.Security.Claims;

namespace WebCollectingIdeas.Controllers
{
    [Authorize]
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
        public IActionResult Detail(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var objIdea = _unitOfWork.Idea.GetIdea(id);
            if (objIdea == null)
            {
                return NotFound();
            };
            var userId = _userManager.GetUserId(HttpContext.User);
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            ManageView(id, objIdea, userId, objView);
            return View(objIdea);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Like(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var objIdeaTemp = _unitOfWork.Idea.GetIdea(id);
            if (objIdeaTemp == null)
            {
                return NotFound();
            };
            var userId = _userManager.GetUserId(HttpContext.User);
            var objViewTemp = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            ManageView(id, objIdeaTemp, userId, objViewTemp);
            var objIdea = _unitOfWork.Idea.GetIdea(id);
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            switch (objView.React)
            {
                case -1:
                    objView.React = 1;
                    objIdea.Likes += 1;
                    objIdea.Dislikes -= 1;
                    break;
                case 1:
                    objView.React = 0;
                    objIdea.Likes -= 1;
                    break;
                default:
                    objView.React = 1;
                    objIdea.Likes += 1;
                    break;
            }
            _unitOfWork.Idea.Update(objIdea);
            _unitOfWork.View.Update(objView);
            _unitOfWork.Save();
            return Json(new { success = true });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Dislike(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var objIdeaTemp = _unitOfWork.Idea.GetIdea(id);
            if (objIdeaTemp == null)
            {
                return NotFound();
            };
            var userId = _userManager.GetUserId(HttpContext.User);
            var objViewTemp = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            ManageView(id, objIdeaTemp, userId, objViewTemp);
            var objIdea = _unitOfWork.Idea.GetIdea(id);
            var objView = _unitOfWork.View.GetFirstOrDefault(x => x.IdeaId == id && x.IdentityUserId == userId);
            switch (objView.React)
            {
                case -1:
                    objView.React = 0;
                    objIdea.Dislikes -= 1;
                    break;
                case 1:
                    objView.React = -1;
                    objIdea.Likes -= 1;
                    objIdea.Dislikes += 1;
                    break;
                default:
                    objView.React = -1;
                    objIdea.Dislikes += 1;
                    break;
            }
            _unitOfWork.Idea.Update(objIdea);
            _unitOfWork.View.Update(objView);
            _unitOfWork.Save();
            return Json(new { success = true });
        }
        public void ManageView(int id, Idea objIdea, string userId, View objView)
        {
            if (objView == null)
            {
                View objViewNew = new View();
                objViewNew.IdentityUserId = userId;
                objViewNew.IdeaId = id;
                objViewNew.LastVisit = DateTime.Now;
                _unitOfWork.View.Add(objViewNew);
                objIdea.Views += 1;
                _unitOfWork.Idea.Update(objIdea);
                _unitOfWork.Save();

            }
            else
            {
                objView.LastVisit = DateTime.Now;
                _unitOfWork.View.Update(objView);
                _unitOfWork.Save();
            }
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
        public IActionResult DownloadZip(int id)
        {
            string wwwRootPath = _webHostEnvironment.WebRootPath;
            // Path to the folder to be compressed.
            string folderPath = Path.Combine(wwwRootPath, @"file/topic_" + id); ;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            // Name of the zip file.
            string zipFileName = "topic_" + id + ".zip";

            // Path to the zip file to be created.
            string zipPath = Path.Combine(Path.GetTempPath(), zipFileName);

            // Use DotNetZip to create the zip file.
            using (ZipFile zip = new ZipFile())
            {
                // Add the files in the folder to the zip file.
                zip.AddDirectory(folderPath);

                // Save the zip file to the specified path.
                zip.Save(zipPath);
            }

            // Return the zip file to the user.
            byte[] fileBytes = System.IO.File.ReadAllBytes(zipPath);
            return File(fileBytes, MediaTypeNames.Application.Zip, zipFileName);
        }
        public IActionResult DownloadExcel(int id)
        {
            var ideas = _unitOfWork.Idea.GetAll();
            var workbook = new XLWorkbook();
            var worksheet = workbook.Worksheets.Add("Ideas");
            worksheet.Cell("A1").Value = "ID";
            worksheet.Cell("B1").Value = "Title";
            worksheet.Cell("C1").Value = "Description";
            worksheet.Cell("D1").Value = "Path";
            worksheet.Cell("E1").Value = "Views";
            worksheet.Cell("F1").Value = "Like";
            worksheet.Cell("G1").Value = "Dislike";
            int row = 2;
            int i = 1;
            foreach (var idea in ideas)
            {
                if (idea.TopicId == id)
                {
                    worksheet.Cell("A" + row).Value = i;
                    worksheet.Cell("B" + row).Value = idea.Title;
                    worksheet.Cell("C" + row).Value = idea.Description;
                    worksheet.Cell("D" + row).Value = idea.Path;
                    worksheet.Cell("E" + row).Value = idea.Views;
                    worksheet.Cell("F" + row).Value = idea.Likes;
                    worksheet.Cell("G" + row).Value = idea.Dislikes;
                    row++;
                    i++;
                }
            }
            var stream = new MemoryStream();
            workbook.SaveAs(stream);
            stream.Seek(0, SeekOrigin.Begin);
            var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileName = "Topic_" + id + ".xlsx";
            return File(stream, contentType, fileName);
        }
    }
}