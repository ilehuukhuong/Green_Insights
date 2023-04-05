using CollectingIdeas.DataAccess.Data;
using CollectingIdeas.DataAccess.Repository.IRepository;
using CollectingIdeas.Models;
using CollectingIdeas.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CollectingIdeas.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        private readonly IUnitOfWork _unitOfWork;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db,
            IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
            _unitOfWork = unitOfWork;
        }
        public void Initialize()
        {
            // migration
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
            if (_unitOfWork.Department.GetFirstOrDefault(u=>u.Name == "Visitor") == null)
            {
                Department obj = new Department();
                obj.Name = "Visitor";
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
            }
            if (_unitOfWork.Department.GetFirstOrDefault(u => u.Name == SD.Role_User_Administrator) == null)
            {
                Department obj = new Department();
                obj.Name = SD.Role_User_Administrator;
                _unitOfWork.Department.Add(obj);
                _unitOfWork.Save();
            }
            // create roles default
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Administrator).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Administrator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_QAManager)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_QACoordinator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Staff)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Visitor)).GetAwaiter().GetResult();
            
                //if roles are not created,then we will create admin user as well
                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "admin@admin.com",
                        Email = "admin@admin.com",
                        FullName = "Administrator",
                        PhoneNumber = "84385190202",
                        DepartmentId = 1,
                        FirstName = "Admin",
                        LastName = "admin",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@admin.com");
                _userManager.AddToRoleAsync(user, SD.Role_User_Administrator).GetAwaiter().GetResult();
                
                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "ilehuukhuong@gmail.com",
                        Email = "ilehuukhuong@gmail.com",
                        FullName = "Le Huu Khuong",
                        PhoneNumber = "84385190202",
                        DepartmentId = 1,
                        FirstName = "Khuong",
                        LastName = "Le",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userKhuong = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "ilehuukhuong@gmail.com");
                _userManager.AddToRoleAsync(userKhuong, SD.Role_User_QAManager).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "phamhiep22102002@gmail.com",
                        Email = "phamhiep22102002@gmail.com",
                        FullName = "Pham Van Hiep",
                        PhoneNumber = "84823447445",
                        isQA = true,
                        DepartmentId = 2,
                        FirstName = "Hiep",
                        LastName = "Pham",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userHiep = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "phamhiep22102002@gmail.com");
                _userManager.AddToRoleAsync(userHiep, SD.Role_User_QACoordinator).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "anhnpgcs200323@fpt.edu.vn",
                        Email = "anhnpgcs200323@fpt.edu.vn",
                        FullName = "Nguyen Phuc Anh",
                        PhoneNumber = "84908984108",
                        DepartmentId = 2,
                        FirstName = "Anh",
                        LastName = "Nguyen",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userPA = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "anhnpgcs200323@fpt.edu.vn");
                _userManager.AddToRoleAsync(userPA, SD.Role_User_Staff).GetAwaiter().GetResult();

                _userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "congminhs875@gmail.com",
                        Email = "congminhs875@gmail.com",
                        FullName = "Leu Minh Cong",
                        PhoneNumber = "84964696447",
                        DepartmentId = 2,
                        FirstName = "Cong",
                        LastName = "Leu",
                        Path = "AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: abcABC@123
                ApplicationUser userCong = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "congminhs875@gmail.com");
                _userManager.AddToRoleAsync(userCong, SD.Role_User_Staff).GetAwaiter().GetResult();
            }

            return;

        }
    }
}
