using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebCollectingIdeas.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Department> items = _unitOfWork.Department.GetAll().OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => CollectingIdeas.Utility.Filter.FilterChar(x.Name).Contains(CollectingIdeas.Utility.Filter.FilterChar(Searchtext)));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Create()
        {
            return View();
        }
        //Post Ceate
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Add(obj);
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
            var departmentformDb = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == id);
            //var departmentformDb = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
            if (departmentformDb == null)
            {
                return NotFound();
            }
            return View(departmentformDb);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Department obj)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Department.Update(obj);
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
            var departmentformDbFirst = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (departmentformDbFirst == null)
            {
                return NotFound();
            }
            return View(departmentformDbFirst);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _unitOfWork.Department.Remove(obj);
                _unitOfWork.Save();
                TempData["Deleted"] = "Delete successfully";
                return RedirectToAction("index");
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == id);
            if (obj != null)
            {
                _unitOfWork.Department.Remove(obj);
                _unitOfWork.Save();
                TempData["Deleted"] = "Delete successfully";
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteAll(string ids)
        {
            if (!string.IsNullOrEmpty(ids))
            {
                var items = ids.Split(',');
                if (items != null && items.Any())
                {
                    foreach (var item in items)
                    {
                        var obj = _unitOfWork.Department.GetFirstOrDefault(u => u.Id == Convert.ToInt32(item));
                        _unitOfWork.Department.Remove(obj);
                        _unitOfWork.Save();
                        TempData["Deleted"] = "Delete successfully";
                    }
                }
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }
    }
}
