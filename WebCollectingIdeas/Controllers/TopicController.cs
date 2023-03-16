﻿using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebCollectingIdeas.Controllers
{
    public class TopicController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public TopicController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index(string Searchtext, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Topic> items = _unitOfWork.Topic.GetAll().OrderByDescending(x => x.Id);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.Name.Contains(Searchtext));
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
        public IActionResult Create(Topic obj)
        {
            obj.CreateDatetime = DateTime.Now;
            if (obj.ClosureDate < DateTime.Now)
            {
                TempData["Deleted"] = "Closure Date must be after Today";
                return View(obj);
            }
            if (obj.FinalClosureDate < obj.ClosureDate)
            {
                TempData["Deleted"] = "Final Closure Date must be after Closure Date";
                return View(obj);
            }
            if (ModelState.IsValid)
            {
                _unitOfWork.Topic.Add(obj);
                _unitOfWork.Save();
                TempData["Success"] = "Create successfully";
                return RedirectToAction("index");
            }
            TempData["Deleted"] = "Create Failed";
            return View(obj);
        }
        public IActionResult Edit(int? id)
        {
            if (id == null || id <= 0)
            {
                return NotFound();
            }
            var topicformDb = _unitOfWork.Topic.GetFirstOrDefault(x => x.Id == id);
            //var departmentformDb = _db.Departments.Find(id);
            //var departmentformDbFirst = _db.Departments.FirstOrDefault(u=>u.Id== id);
            //var departmentformDbsingle = _db.Departments.SingleOrDefault(u => u.Id == id);
            if (topicformDb == null)
            {
                return NotFound();
            }
            return View(topicformDb);
        }
        //post Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Topic obj)
        {
            if (obj.ClosureDate < obj.CreateDatetime)
            {
                TempData["Deleted"] = "Closure Date must be after Create Date";
                return View(obj);
            }
            if (obj.FinalClosureDate < obj.ClosureDate)
            {
                TempData["Deleted"] = "Final Closure Date must be after Closure Date";
                return View(obj);
            }
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
            //var categoryformDb = _db.Categories.Find(id);
            var topicformDbFirst = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == id);
            //var categoryformDbsingle = _db.Categories.SingleOrDefault(u => u.Id == id);
            if (topicformDbFirst == null)
            {
                return NotFound();
            }
            return View(topicformDbFirst);
        }
        //post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePOST(int? id)
        {
            var obj = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == id);
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
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == id);
            if (obj != null)
            {
                _unitOfWork.Topic.Remove(obj);
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
                        var obj = _unitOfWork.Topic.GetFirstOrDefault(u => u.Id == Convert.ToInt32(item));
                        _unitOfWork.Topic.Remove(obj);
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