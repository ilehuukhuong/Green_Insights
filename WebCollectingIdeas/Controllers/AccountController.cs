using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace WebCollectingIdeas.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AccountController(IUnitOfWork unitOfWork)
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
            IEnumerable<ApplicationUser> items = _unitOfWork.ApplicationUser.GetAll(includeProperties:"Department").OrderByDescending(x => x.FirstName);
            if (!string.IsNullOrEmpty(Searchtext))
            {
                items = items.Where(x => x.FirstName.Contains(Searchtext) || x.LastName.Contains(Searchtext));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public IActionResult Upsert(string? id) 
        { 

            return View(); 
        }
    }
}
