using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;

namespace OnlineCourseApp.Controllers
{
    [Authorize]
    public class CourseParticipantController : NotificationsController
    {
        private readonly UserManager<AppUser> _userManager;
        private ICourseParticipantRepository courseParticipantRepository;
        private IStudentRepository studentRepository;
        private ICourseRepository _courseRepository;
        private IDocumentShareRepository _documentShareRepository;
        public CourseParticipantController(ICourseParticipantRepository courseParticipantRepository,
                                            IStudentRepository studentRepository,
                                            UserManager<AppUser> userManager,
                                            ICourseRepository courseRepository,
                                             IDocumentShareRepository documentShareRepository)
        {
            this.courseParticipantRepository = courseParticipantRepository;
            this.studentRepository = studentRepository;
            _userManager = userManager;
            _courseRepository = courseRepository;
            _documentShareRepository = documentShareRepository;
        }
        public IActionResult AddParticipant(int courseID)
        {
            string userID = _userManager.GetUserId(User);
            CourseParticipants application = new CourseParticipants
            {
                CourseID = courseID,
                StudentID = studentRepository.GetCurrentStudentId(int.Parse(userID)),
            };

            if (courseParticipantRepository.ApplicationExist(application) == true)
            {
                ErrorNotification = "You have already applied for this course.";
            }  
            else
            {
            courseParticipantRepository.Add(application);
                SuccessNotification = "You successfully applied for the course.";
            }
            
            return RedirectToAction("Home", "Dashboard");
        }

        public IActionResult MyCourses()
        {
            string userID = _userManager.GetUserId(User);
            int studentID = studentRepository.GetCurrentStudentId(int.Parse(userID));
            var courses = _courseRepository.GetAppliedCourses(studentID);
            return View(courses);
        }

        public IActionResult Detalji(int courseID)
        {
            var course = _courseRepository.GetCoursesById(courseID);

            if (course == null)
            {
                ViewBag.ErrorMessage = "Kurs nije pronađen";
                return View("_NotFound");
            }
            ViewData["Documents"] = _documentShareRepository.GetDocumentByCourse( courseID);
            return View(course);
        }

        public IActionResult DeleteParticipant(int paxID, int courseID)
        {
            courseParticipantRepository.Delete(paxID);
            SuccessNotification = "Uspješno ste obrisali prijavljenog.";

            return RedirectToAction("Detalji", "Course", new { courseID });
        }
        public IActionResult DeleteParticipantAdmin(int paxID, int courseID)
        {
            courseParticipantRepository.Delete(paxID);
            SuccessNotification = "Uspješno ste obrisali prijavljenog.";

            return RedirectToAction("CourseParticipantsList", "CourseParticipant", new { courseID });
        }
        public IActionResult CourseParticipantsList(int courseID)
        {
            ViewBag.CourseID = courseID;
            var paxList = courseParticipantRepository.GetCourseParticipantList(courseID);
            return View(paxList);
        }

    }
}