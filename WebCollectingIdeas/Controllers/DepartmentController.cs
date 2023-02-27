using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebCollectingIdeas.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<Department> objDepartmentList = _unitOfWork.Department.GetAll();
            return View(objDepartmentList);
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
            var departmentformDb = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == id);
            //var departmentformDb = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Department.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Department.SingleOrDefault(u => u.Id == id);
            if (departmentformDb == null)
            {
                return NotFound();
            }
            return View(departmentformDb);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Department.GetFirstOrDefault(x => x.Id == id);
            //var obj = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
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
    }
}