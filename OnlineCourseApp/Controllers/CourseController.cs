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
    public class CourseController : NotificationsController
    {
        private ICourseRepository courseRepository;
        private ICourseSectionRepository2 courseSectionRepository2;
        private IDocumentShareRepository documentShareRepository;
        private readonly UserManager<AppUser> _userManager;

        public CourseController(UserManager<AppUser> userManager, ICourseRepository courseRepository, ICourseSectionRepository2 courseSectionRepository2,
                                IDocumentShareRepository documentShareRepository)
        {
            _userManager = userManager;
            this.courseRepository = courseRepository;
            this.courseSectionRepository2 = courseSectionRepository2;
            this.documentShareRepository = documentShareRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Prikaz(int courseSectionID)
        {
            var courses = courseRepository.GetCoursesBasedOnSection(courseSectionID);
            return View(courses);
        }
        public IActionResult CourseApplications()
        {
            var courses = courseRepository.GetAllCourses();
            return View(courses);
        }
        public IActionResult DodajForma()
        {

            ViewData["courseSections"] = courseSectionRepository2.GetCourseSectionList();
            CourseAddVM Model = new CourseAddVM();
            return PartialView(Model);

        }
        public async Task<IActionResult> Dodaj(CourseAddVM c)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            
            Course course = new Course
            {
                CourseName =c.CourseName,
                Start =c.Start,
                End = c.End,
                Notes= c.Notes,
                ProfessorId = user.Id,
                CourseSectionID = c.CourseSectionID,  
                EnableCourseSessions = c.EnableCourseSessions,
                EnableVideoRecording = c.EnableVideoRecording

    };
            courseRepository.Add(course);
            SuccessNotification = "Uspješno ste dodali kurs.";
            return RedirectToAction("GetCoursesByProfessor", "Course");
        }

    
        public IActionResult EditCourse(int courseID)
        {
            ViewData["courseSections"] = courseSectionRepository2.GetCourseSectionList();
            var course = courseRepository.GetById(courseID);

            if (course == null)
            {
                ViewBag.ErrorMessage = "Kurs nije pronađen";
                return View("_NotFound");
            }

            CourseEditVM courseVM = new CourseEditVM
            {
                  CourseID =course.ID,
                  CourseName =course.CourseName,
                  Start =course.Start,
                  End =course.End,
                  Notes =course.Notes,
                  CourseSectionID = course.CourseSectionID,
                  EnableCourseSessions =course.EnableCourseSessions,
                  EnableVideoRecording  = course.EnableVideoRecording
             };
           
            return PartialView(courseVM);
        }
        public IActionResult SaveEditCourse(CourseEditVM c)
        {
            var course = courseRepository.GetById(c.CourseID);
            if (course == null)
            {
                ViewBag.ErrorMessage = "Kurs nije pronađen";
                return View("_NotFound");
            }
            else
            {
                course.CourseName = c.CourseName;
                course.Start = c.Start;
                course.End = c.End;
                course.Notes = c.Notes;
                course.CourseSectionID = c.CourseSectionID;
                course.EnableCourseSessions = c.EnableCourseSessions;
                course.EnableVideoRecording = c.EnableVideoRecording;

                courseRepository.Update(course);
                SuccessNotification = "Uspješno ste ažurirali kurs.";
            }
            return RedirectToAction("GetCoursesByProfessor", "Course");
        }


        public IActionResult Obrisi(int CourseID)
        {
            courseRepository.Delete(CourseID);
            SuccessNotification = "Uspješno ste obrisali kurs.";
            return RedirectToAction("GetCoursesByProfessor", "Course");
        }

        public async Task<IActionResult> GetCoursesByProfessor()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var courses = courseRepository.GetAllCourses().Where(c => c.ProfessorId == user.Id).ToList();
            return View("ProfessorCourses", courses);
        }

        public async Task<IActionResult> Detalji(int courseID)
        {
            var course = courseRepository.GetCoursesById(courseID);
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            if (course == null)
            {
                ViewBag.ErrorMessage = "Kurs nije pronađen";
                return View("_NotFound");
            }
            ViewData["Documents"] = documentShareRepository.GetDocumentByUserAndCourse(user.Id, courseID); 
            return View(course);
        }

    }
}