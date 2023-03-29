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
            if (_unitOfWork.Department.GetAll().Count() == 0)
            {
                Department obj = new Department();
                obj.Name = SD.Role_User_Administrator;
                _unitOfWork.Department.Add(obj);
            }
            // create roles default
            if (!_roleManager.RoleExistsAsync(SD.Role_User_Administrator).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Administrator)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_QAManager)).GetAwaiter().GetResult();
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
                        Path = "/AccountProfile/acc (0).jpg"
                    }, "abcABC@123").GetAwaiter().GetResult(); //pass: Admin123*
                ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_User_Administrator).GetAwaiter().GetResult();
            }

            return;

        }
    }
}
