using Microsoft.AspNetCore.Identity;
using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Account;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class ExamRepository : BaseRepository<Exam>, IExamRepository
    {
        public ExamRepository(MyDBContext db) : base(db) { }

        public List<ExamPreviewVM> GetAllExams()
        {
            return db.Exam.Select(e => new ExamPreviewVM
            {
                ExamID = e.ID,
                Title = e.Title,
                CourseID = e.CourseID,
                Course = db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault(),
            }).ToList();
        }

        public List<ExamPreviewVM> GetAllExamsByProfessor(int ownerID)
        {
            return db.Exam.Where(e => e.ExamOwnerID == ownerID).Select(e => new ExamPreviewVM
            {
                ExamID = e.ID,
                Title = e.Title,
                CourseID = e.CourseID,
                Course = db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault(),
            }).ToList();
        }

        public List<ExamPreviewVM> GetAllExamsByAppliedCourses(int userID)
        {
            var applications = db.CourseParticipants.Where(c => c.StudentID == userID).ToList();
            var exams = db.Exam.Where(e => e.IsActive == true).Select(e => new ExamPreviewVM
            {
                ExamID = e.ID,
                Title = e.Title,
                CourseID = e.CourseID,
                Course = db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault(),
            }).ToList();
            return exams.Where(e => applications.Any(a => a.CourseID == e.CourseID)).ToList();
        }

        public ExamEditVM EditExam(int examID)
        {
            Exam exam = db.Exam.Find(examID);

            if (exam == null)
                return null;

            else return new ExamEditVM
            {
                ExamID = exam.ID,
                ActivationDate = exam.ActivationDate,
                DeactivationDate = exam.DeactivationDate,
               Title = exam.Title,
               TimeLimit =(int) exam.TimeLimit.TotalMinutes,
               Instructions = exam.Instructions,
               Active = exam.IsActive,
               CourseID = exam.CourseID,
               Course = db.Course.Find(exam.CourseID)               
            };
        }

        public ExamDetailsVM GetExamById(int examID)
        {
            return db.Exam.Where(e => e.ID == examID).Select(e => new ExamDetailsVM
            {
                ExamID = e.ID,
                Title = e.Title,
                Instructions = e.Instructions,
                ActivationDate = e.ActivationDate,
                DeactivationDate = e.DeactivationDate,
                TimeLimit = e.TimeLimit,
                RandomizeQuestions = e.RandomizeQuestions, 
                NumOfQuestions = db.Question.Where(q => q.ExamID == e.ID).Count(),
                CourseID = e.CourseID,
                Course = db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault(),
                ExamOwnerID = e.ExamOwnerID,
                ExamOwner = db.Users.Where(u => u.Id == e.ExamOwnerID).Select(e => e.FirstName + ' ' + e.LastName).FirstOrDefault()
            }).FirstOrDefault();
        }
    }
}

