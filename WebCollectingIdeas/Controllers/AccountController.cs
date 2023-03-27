using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using CollectingIdeas.Models.ViewModel;
using CollectingIdeas.Utility;
using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Text.Encodings.Web;
using System.Text;
using X.PagedList;
using WebCollectingIdeas.Areas.Identity.Pages.Account;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.IdentityModel.Tokens;

namespace WebCollectingIdeas.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUserStore<ApplicationUser> _userStore;
        private readonly UserManager<ApplicationUser> _userManager;
        public AccountController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment, IUserStore<ApplicationUser> userStore, UserManager<ApplicationUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
            _userStore = userStore;
            _userManager = userManager;
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
                items = items.Where(x => CollectingIdeas.Utility.Filter.FilterChar(x.FullName).Contains(CollectingIdeas.Utility.Filter.FilterChar(Searchtext)));
            }
            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            ViewBag.SearchText = Searchtext;
            return View(items);
        }
        public IActionResult Upsert(string? id) 
        { 
            AccountVM accountVM = new AccountVM();
            accountVM.account = new ApplicationUser();
            accountVM.DepartmentList = _unitOfWork.Department.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            if (id == null)
            {
                // Create account
                ViewBag.CreateAccount = true;
                return View(accountVM);
            }
            else
            {
                // Update account              
                accountVM.account = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
                return View(accountVM);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAccount(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            user.isDelete = !user.isDelete;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                TempData["Deleted"] = "Account delete failed";
                return Json(new { success = false });
            }
            if (user.isDelete == true)
            {
                TempData["Success"] = "Account delete sucessfully";
                return Json(new { success = true });
            }
            else
            {
                TempData["Success"] = "Account restore sucessfully";
                return Json(new { success = true });
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upsert(AccountVM obj, IFormFile? file)
        {
            obj.Password = "ABCabc123@";
            if (ModelState.IsValid)
            {
                // upload images
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString();
                    var uploads = Path.Combine(wwwRootPath, @"AccountProfile\Users");
                    var extension = Path.GetExtension(file.FileName);
                    if (obj.account.Path != null)
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.account.Path.TrimStart('\\'));
                        if (obj.account.Path.StartsWith(@"AccountProfile\Users\"))
                        {
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        
                    }
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName + extension), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    obj.account.Path = @"AccountProfile\Users\" + fileName + extension;
                }
                if (_unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == obj.account.Id) == null)
                {
                    var user = CreateUser();
                    await _userStore.SetUserNameAsync(user, obj.account.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, obj.Password);
                    user.FullName = obj.account.FirstName + " " + obj.account.LastName;
                    user.FirstName = obj.account.FirstName;
                    user.LastName = obj.account.LastName;
                    user.DepartmentId = obj.account.DepartmentId;
                    user.Email = obj.account.Email.ToLower();
                    user.NormalizedEmail = obj.account.Email.Normalize();
                    user.EmailConfirmed = obj.account.EmailConfirmed;
                    user.PhoneNumberConfirmed = obj.account.PhoneNumberConfirmed;
                    user.PhoneNumber = obj.account.PhoneNumber;
                    if (obj.account.Path == null)
                    {
                        Random r = new Random();
                        user.Path = @"AccountProfile/acc (" + r.Next(1, 20) + @").jpg";
                    }
                    else
                    {
                        user.Path = obj.account.Path;
                    }
                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_User_Visitor);
                        var userId = await _userManager.GetUserIdAsync(user);
                        var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    TempData["Success"] = "Account create sucessfully";
                }
                else
                {
                    var user = await _userManager.FindByIdAsync(obj.account.Id);
                    if (user == null)
                    {
                        return NotFound();
                    }
                    user.isDelete = obj.account.isDelete;
                    user.FullName = obj.account.FirstName + " " + obj.account.LastName;
                    user.DepartmentId = obj.account.DepartmentId;
                    user.FirstName = obj.account.FirstName;
                    user.LastName = obj.account.LastName;
                    if (obj.account.Path != null) {
                        user.Path = obj.account.Path;
                    }
                    user.Email = obj.account.Email;
                    user.UserName = obj.account.Email;
                    user.EmailConfirmed = obj.account.EmailConfirmed;
                    user.PhoneNumber = obj.account.PhoneNumber;
                    user.PhoneNumberConfirmed = obj.account.PhoneNumberConfirmed;
                    var result = await _userManager.UpdateAsync(user);
                    if (!result.Succeeded)
                    {
                        TempData["Deleted"] = "Account update failed";
                        return View(obj);
                    }
                    TempData["Success"] = "Account update sucessfully";
                }
                return RedirectToAction("index");
            }
            obj.DepartmentList = _unitOfWork.Department.GetAll().Select(
                u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }
                );
            return View(obj);
        }
        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public IActionResult UserRole(string id)
        {
            AccountVM accountVM = new AccountVM();
            accountVM.account = new ApplicationUser();
            accountVM.account = _unitOfWork.ApplicationUser.GetFirstOrDefault(u => u.Id == id);
            return View(accountVM);
        }

    }
}
