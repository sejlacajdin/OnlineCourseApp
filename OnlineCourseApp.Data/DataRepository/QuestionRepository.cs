using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
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
using OnlineCourseApp.Enums;

namespace OnlineCourseApp.Data.DataRepository
{
    public class QuestionRepository : BaseRepository<Question>, IQuestionRepository
    {
        public QuestionRepository(MyDBContext db) : base(db) { }

        public List<SelectListItem> GetQuestionCategories()
        {
            return db.QuestionCategory.Select(c => new SelectListItem
            {
                Value = c.ID.ToString(),
                Text = c.CategoryName

            }).ToList();
        }
         public Question GetLastQuestion()
        {
            int max = db.Question.Max(p => p.ID);

            return db.Question.Find(max);
        }

        public List<QuestionExamListVM> GetQuestionByExam(int examID)
        {
            return db.Question.Where(q => q.ExamID == examID).Select(a => new QuestionExamListVM
            {
                QuestionID = a.ID,
                Text = a.Text

            }).ToList();
        }

        public QuestionDetailsVM GetQuestionDetails(int questionID)
        {
            return db.Question.Where(q => q.ID == questionID).Select(a => new QuestionDetailsVM 
            {
                QuestionID = a.ID,
                QuestionCategoryID = a.QuestionCategoryID,
                QuestionCategory = db.QuestionCategory.Where(c => c.ID == a.QuestionCategoryID).Select(c => c.CategoryName).FirstOrDefault(),
                QuestionType = a.QuestionType,
                QuestionNumber = (int)a.QuestionNumber,
                Points = a.Points,
                IsActive = a.IsActive,
                Text = a.Text,
                Choises = db.Choice.Where(c => c.QuestionID == questionID).Select(c => new QuestionDetailsVM.Choices 
                {
                    ChoiceID = c.ID,
                    Text = c.Text,
                    Points = c.Points,
                    IsCorrect = (bool)c.IsCorrect ? "Tačno" : "Netačno"
                }).ToList()

            }).FirstOrDefault();
        }

        public QuestionEditVM EditQuestion(int questionID)
        {
            Question question = db.Question.Find(questionID);

            if (question == null)
                return null;

            else return new QuestionEditVM
            {
                QuestionID = question.ID,
                ExamID = question.ExamID,
                QuestionCategoryID = question.QuestionCategoryID,
                QuestionCategory = question.QuestionCategory,
                QuestionType = question.QuestionType,
                IsActive = question.IsActive,
                Points = question.Points,
                Text = question.Text,
                QuestionCategoryList = new List<SelectListItem>(),
                QuestionTypeList = new List<SelectListItem>()
        };
        }
        public int GetLastQuestionNumber(int examID)
        {

            if (db.Question.Where(q => q.ExamID == examID).Count() <= 0)
                return 0;
            else
            return (int)db.Question.Where(q => q.ExamID == examID).Select(x => x.QuestionNumber ).ToList().LastOrDefault();
        }
        public QuestionDisplayVM GetQuestionByNumber(int? qno, int examID)
        {
            return db.Question.Where(q => q.ExamID == examID && q.QuestionNumber == qno).Select(x => new QuestionDisplayVM
            { 
                QuestionID = x.ID,
                QuestionType = x.QuestionType,
                QuestionNumber =(int) x.QuestionNumber,
                Question = x.Text,
                Point = x.Points,
                ExamID = x.ExamID,
                ExamName = x.Exam.Title,
                TotalQuestionInSet = db.Question.Where(q => q.ExamID == examID).Count(),
                Options = db.Choice.Where(c => c.QuestionID == x.ID).Select(c => new QuestionOption { 
                    ChoiceID = c.ID,
                    Text = c.Text
                    
                }).ToList()

            }).FirstOrDefault();
        }

        public QuestionInfoVM GetQuestionInfoByChoice(int examID, int questionNumber)
        {
            return db.Question.Where(x => x.ExamID == examID && x.QuestionNumber == questionNumber).Select(x => new QuestionInfoVM
            {
                QuestionID = x.ID,
                ExamID = x.ExamID,
                QuestionType = x.QuestionType,
                Points = x.Points
            }).FirstOrDefault();
        }

        public List<ExamAnsweredQuestion> GetPointsByChoices(QuestionInfoVM info, Answers choices, int studentID)
        {
           
            var points = db.Choice.Where(a => choices.UserSelectedID.Any(b => b == a.ID)).Select(a => new { a.ID, a.Points }).AsEnumerable();
      
                  return points.Select(x => new ExamAnsweredQuestion
                    {
                        StudentID = studentID,
                        QuestionID = info.QuestionID,
                        ChoiceID = x.ID,
                        Answer = "CHECKED",
                        MarkScored = Math.Floor((info.Points/100.00) * x.Points)
                    }).ToList();

        }

        public int GetNextQuestionNumber(int examID, int questionID)
        {
            if (db.Question.Where(x => x.ExamID == examID).OrderByDescending(x => x.QuestionNumber).Take(1).Select(x => x.QuestionNumber).FirstOrDefault() == questionID)
                return questionID;

            return (int)db.Question.Where(x => x.ExamID == examID && x.QuestionNumber > questionID).OrderBy(x => x.QuestionNumber)
                .Take(1).Select(x => x.QuestionNumber).FirstOrDefault();
        }
        public int GetPreviousQuestionNumber(int examID, int questionID)
        {
            if (questionID == 1)
                return 1;

            return (int)db.Question.Where(x => x.ExamID == examID && x.QuestionNumber < questionID).OrderByDescending(x => x.QuestionNumber)
                .Take(1).Select(x => x.QuestionNumber).FirstOrDefault();
        }

        public double GetTotalExamPoints(int examID)
        {
            return db.Question.Where(q => q.ExamID == examID).Sum(p => p.Points);
        }

    }
}

