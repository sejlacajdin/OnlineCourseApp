using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Migrations;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class CourseRepository : BaseRepository<Course>, ICourseRepository
    {
        public CourseRepository(MyDBContext db) : base(db) { }

        public List<CourseVM> GetAllCourses()
        {
            return db.Course.Select(course => new CourseVM
            {
                CourseID = course.ID,
                CourseName = course.CourseName,
                Start = course.Start,
                End = (DateTime)course.End,
                Notes = course.Notes,
                ProfessorId = course.ProfessorId,
                CourseSectionID=course.CourseSectionID,
                CourseSection=course.CourseSection.Name,
                CourseParticipants = db.CourseParticipants.Where(c => c.CourseID == course.ID).Select(part => new CourseParticipantsVM
                {
                    CoursePaxID = part.ID,
                    StudentIDNumber = part.Student.StudentIDNumber,
                    FirstName = part.Student.User.FirstName,
                    LastName = part.Student.User.LastName,
                    Email = part.Student.User.Email
                }).ToList(),
                Exams = db.Exam.Where(c => c.CourseID == course.ID).Select(exam => new ExamVM
                { 
                     Title = exam.Title,
                    Instructions = exam.Instructions,
                    ActivationDate = exam.ActivationDate,
                     DeactivationDate  = exam.DeactivationDate,
                     TimeLimit = exam.TimeLimit,
                   RandomizeQuestions  = exam.RandomizeQuestions,              
                     CourseID = exam.CourseID,
                     Course = exam.Course.CourseName,
                   ExamOwnerID = exam.ExamOwnerID,
                   ExamOwner = exam.ExamOwner.UserName
                }).ToList()

            }).ToList();
        }

        public List<CourseVM> GetCoursesBasedOnSection(int courseSectionID)
        {
            return db.Course.Where(c => c.CourseSectionID == courseSectionID).Select(course => new CourseVM
            {
                CourseID = course.ID,
                CourseName = course.CourseName,
                Start = course.Start,
                End = (DateTime)course.End,
                Notes = course.Notes,
                CourseSectionID = course.CourseSectionID,
                CourseParticipants = db.CourseParticipants.Where(c => c.CourseID == course.ID).Select(part => new CourseParticipantsVM {

                    StudentIDNumber = part.Student.StudentIDNumber,
                    FirstName = part.Student.User.FirstName,
                    LastName = part.Student.User.LastName,
                    Email = part.Student.User.Email
                }).ToList()

            }).ToList();
        }
        public CourseDetaljiVM GetCoursesById(int courseID)
        {
            return db.Course.Where(c => c.ID == courseID).Select(course => new CourseDetaljiVM
            {
                CourseID = course.ID,
                CourseName = course.CourseName,
                Start = course.Start,
                End = (DateTime)course.End,
                Notes = course.Notes,
                CourseSectionID = course.CourseSectionID,
                CourseSection = string.Join("",db.CourseSection.Where(c => c.ID == course.CourseSectionID).Select(c => c.Name).FirstOrDefault()),
                CourseParticipants = db.CourseParticipants.Where(c => c.CourseID == course.ID).Select(part => new CourseParticipantsVM
                {
                    CoursePaxID = part.ID,
                    StudentID = part.Student.ID,
                    StudentIDNumber = part.Student.StudentIDNumber,
                    FirstName = part.Student.User.FirstName,
                    LastName = part.Student.User.LastName,
                    Email = part.Student.User.Email
                }).ToList()

            }).FirstOrDefault();
        }

        public List<SelectListItem> GetCourseList()
        {
            return db.Course.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.CourseName
            }).ToList();
        }

        public List<SelectListItem> GetCourseListByOwner(int ownerID)
        {
            return db.Course.Where(c => c.ProfessorId == ownerID).Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.CourseName
            }).ToList();
        }

        public List<CourseVM> GetAppliedCourses(int studentID)
        {
            List<CourseParticipants> applications = db.CourseParticipants.Where(c => c.StudentID == studentID).ToList();
            List<CourseVM> courses = db.Course.Select(course => new CourseVM
            {
                CourseID = course.ID,
                CourseName = course.CourseName,
                Start = course.Start,
                End = (DateTime)course.End,
                Notes = course.Notes,
                ProfessorId = course.ProfessorId,
                CourseSectionID = course.CourseSectionID,
                CourseSection = course.CourseSection.Name
            }).ToList();

            return courses.Where(c => applications.Any(a => a.CourseID == c.CourseID)).ToList();
        }
        public List<CourseVM> GetNotAppliedCourses(int studentID)
        {
            List<CourseParticipants> applications = db.CourseParticipants.Where(c => c.StudentID == studentID).ToList();
            List<CourseVM> courses = db.Course.Select(course => new CourseVM
            {
                CourseID = course.ID,
                CourseName = course.CourseName,
                Start = course.Start,
                End = (DateTime)course.End,
                Notes = course.Notes,
                ProfessorId = course.ProfessorId,
                CourseSectionID = course.CourseSectionID,
                CourseSection = course.CourseSection.Name
            }).ToList();

            return courses.Where(c => applications.All(a => a.CourseID != c.CourseID)).ToList();
        }

    }
}
