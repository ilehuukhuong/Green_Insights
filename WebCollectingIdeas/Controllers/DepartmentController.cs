using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebCollectingIdeas.Data;
using WebCollectingIdeas.Models;

namespace WebCollectingIdeas.Controllers
{
    public class DepartmentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DepartmentController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            IEnumerable<Department> objDepartmentList = _db.Departments.ToList();
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
                _db.Departments.Add(obj);
                _db.SaveChanges();
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
            var departmentformDb = _db.Departments.Find(id);
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
                _db.Departments.Update(obj);
                _db.SaveChanges();
                TempData["Success"] = "Edit successfully";
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
            var departmentformDb = _db.Departments.Find(id);
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
            var obj = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            else
            {
                _db.Departments.Remove(obj);
                _db.SaveChanges();
                TempData["Success"] = "Delete successfully";
                return RedirectToAction("index");
            }

            return View(obj);

        }
    }
}