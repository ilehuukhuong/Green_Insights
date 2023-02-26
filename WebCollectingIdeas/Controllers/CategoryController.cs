using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;

namespace WebCollectingIdeas.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _unitOfWork.Category.GetAll();
            return View(objCategoryList);
        }
        public IActionResult Create()
        {           
            return View();
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id ==null || id == 0)
            {
                return NotFound();
            }
            //var categoryformDb = _db.Categories.Find(id);
            var categoryformDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryformDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryformDbFirst);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Category.Update(obj);
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
            //var categoryformDb = _db.Categories.Find(id);
            var categoryformDbFirst = _unitOfWork.Category.GetFirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryformDbFirst == null)
            {
                return NotFound();
            }
            return View(categoryformDbFirst);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryformDbFirst = _db.Categories.FirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Category.Remove(obj);
                _unitOfWork.Save();
                TempData["Deleted"] = "Delete successfully";
                return RedirectToAction("index");
            }
        }
    }
}
