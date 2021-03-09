using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.ViewModels;

namespace OnlineCourseApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class SettingsController : NotificationsController
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private IUserRepository _userRepository;

        public SettingsController(UserManager<AppUser> userManager,
                                 SignInManager<AppUser> signInManager,
                                 IUserRepository userRepository)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Professors()
        {
            var users = _userRepository.GetAccounts(null, null, null).ToList();
            var professors = users.Where(x => _userManager.GetRolesAsync(x).Result.Contains("Profesor")).ToList();
            var model = new List<ProfessorsVM>();
            if (users.Any())
            {
                foreach (var x in professors)
                {
                    var m = new ProfessorsVM
                    {
                        ID = x.Id,
                        FullName = x.FirstName + " " + x.LastName,
                        Username = x.UserName,
                        Email = x.Email,
                    };
                    model.Add(m);
                }
            }

            return View(model);
        }

        public IActionResult AddProfessor()
        {
            return PartialView();
        }

        public async Task<IActionResult> SaveProfessor(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Password = model.Password,
                    UserName = model.UserName,
                    Email = model.Email,
                    RegistrationDate = DateTime.Now
                };
                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    user.LastLoginDate = DateTime.Now;
                    await _userManager.AddToRoleAsync(user, "Profesor");
                    await _signInManager.SignInAsync(user, false);
                    SuccessNotification = "Uspješno ste dodali novog profesora.";
                }
                else
                {
                    foreach (var x in result.Errors)
                    {
                        ModelState.AddModelError("", x.Description);
                    }
                }
            }

            return RedirectToAction("Professors","Settings");
        }
    }
}