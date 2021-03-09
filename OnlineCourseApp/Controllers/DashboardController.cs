using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.Enums;

namespace OnlineCourseApp.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private IAnnouncementRepository _announcementRepository;
        private readonly UserManager<AppUser> _userManager;
        private ICourseRepository _courseRepository;
        private IStudentRepository _studentRepository;
        public DashboardController(IAnnouncementRepository announcementRepository, ICourseRepository courseRepository, UserManager<AppUser> userManager, IStudentRepository studentRepository)
        {
            _announcementRepository = announcementRepository;
            _courseRepository = courseRepository;
            _userManager = userManager;
            _studentRepository = studentRepository;
        }
        public IActionResult Home(string permission = null)
        {
            HttpContext.Session.SetString("ref", "dashboard");

            if (permission != null || Permission != null)
            {
                if (permission == null && Permission != null)
                    permission = Permission;
                
                Permission = permission;
                if (User.IsInRole(permission))
                {
                    if (permission == "Admin")
                    {
                        return View($"{permission}Home", new DashboardVM { Announcements = _announcementRepository.GetLastAnnouncementsForAdmin() });

                    }
                    AnnouncementFilterType x = permission == "Student" ? AnnouncementFilterType.AllStudents : AnnouncementFilterType.AllProfessors;
                    return View($"{permission}Home", new DashboardVM { Announcements = _announcementRepository.GetLastAnnouncements(x), Courses = _courseRepository.GetAllCourses() });
                }
            }
            if (User.IsInRole("Admin"))
            {
                Permission = "Admin";
                return View("AdminHome", new DashboardVM { Announcements = _announcementRepository.GetLastAnnouncementsForAdmin() });
            }

            else if (User.IsInRole("Profesor"))
            {
                Permission = "Profesor";
                return View("ProfesorHome", new DashboardVM { Announcements = _announcementRepository.GetLastAnnouncements(AnnouncementFilterType.AllProfessors) });
            }

            else if (User.IsInRole("Student"))
            {
                string userID = _userManager.GetUserId(User);
                int studentID = _studentRepository.GetCurrentStudentId(int.Parse(userID));
                Permission = "Student";
                return View("StudentHome", new DashboardVM { Announcements = _announcementRepository.GetLastAnnouncements(AnnouncementFilterType.AllStudents), Courses = _courseRepository.GetNotAppliedCourses(studentID) });
            }
            else
                return RedirectToAction("Login", "Account");
        }
    }

}

