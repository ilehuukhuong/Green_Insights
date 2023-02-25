using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using CollectingIdeas.DataAccess.Data;

namespace WebCollectingIdeas.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objCategoryList = _db.Categories.ToList();
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
                _db.Categories.Add(obj);
                _db.SaveChanges();
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
            var categoryformDb = _db.Categories.Find(id);
            //var categoryformDbFirst = _db.Categories.FirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryformDb == null)
            {
                return NotFound();
            }
            return View(categoryformDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                _db.SaveChanges();
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
            var categoryformDb = _db.Categories.Find(id);
            //var categoryformDbFirst = _db.Categories.FirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (categoryformDb == null)
            {
                return NotFound();
            }
            return View(categoryformDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _db.Categories.Find(id);
            //var categoryformDbFirst = _db.Categories.FirstOrDefault(u=>u.Id== id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _db.Categories.Remove(obj);
                _db.SaveChanges();
                TempData["Deleted"] = "Delete successfully";
                return RedirectToAction("index");
            }             

        }
    }
}
