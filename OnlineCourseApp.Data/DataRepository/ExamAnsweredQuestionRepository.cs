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
    public class ExamAnsweredQuestionRepository : BaseRepository<ExamAnsweredQuestion>, IExamAnsweredQuestionRepository
    {
        public ExamAnsweredQuestionRepository(MyDBContext db) : base(db) { }
        public List<ExamAnsweredQuestion> GetAnswers(int questionID, int studentID)
        {
            return db.ExamAnsweredQuestion.Where(x => x.QuestionID == questionID && x.StudentID == studentID)
                .Select(x => new ExamAnsweredQuestion
                { 
                    ID = x.ID,
                  ChoiceID = x.ChoiceID,
                  Answer = x.Answer
                }).ToList();
        }

        public bool ExamTaken(int studentID, int examID)
        {
            List<ExamAnsweredQuestion> answers = db.ExamAnsweredQuestion.Where(e => e.StudentID == studentID).ToList();
            List<Question> questions = db.Question.Where(q =>q.ExamID == examID).ToList();
            var questionList = questions.Where(q => answers.Any(a => a.QuestionID == q.ID)).ToList();

            if (questionList.Count() > 0)
                return true;
            else return false;
        }

        public bool ExamTakenForProfessor(int examID)
        {
            List<ExamAnsweredQuestion> answers = db.ExamAnsweredQuestion.ToList();
            List<Question> questions = db.Question.Where(q => q.ExamID == examID).ToList();
            var questionList = questions.Where(q => answers.Any(a => a.QuestionID == q.ID)).ToList();

            if (questionList.Count() > 0)
                return true;
            else return false;
        }

        public List<ResultsListVM> ResultsForStudent(int studentID)
        {

            var list = db.ExamAnsweredQuestion.Where(e => e.StudentID == studentID).GroupBy(x => x.QuestionID).Select(s => new
            {
                Question = s.Key,
                Result = s.Sum(a => a.MarkScored)
            }).ToList();

            var exams = list.GroupBy(l => db.Question.Where(a => l.Question == a.ID).Select(e => e.ExamID).ToList()).Select(s => new {
                Exam = s.Key,
                Result = s.Sum(a => a.Result)
            }).ToList();

            var results = exams.GroupBy(e => e.Exam.FirstOrDefault()).Select(x => new
            {
                ExamID = x.Key,
                Result = x.Sum(a => a.Result)
            }).ToList();

            return results.Select(x => new ResultsListVM {
                StudentID = studentID,
                ExamID = x.ExamID,
                ExamName = db.Exam.Where(e => e.ID == x.ExamID).Select(e => e.Title).FirstOrDefault(),
                Date = db.Exam.Where(e => e.ID == x.ExamID).Select(e => e.ActivationDate).FirstOrDefault(),
                CourseName = db.Exam.Where(e => e.ID == x.ExamID).Select(e => db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault()).FirstOrDefault(),
                Result = Math.Round((x.Result * 100.00) / db.Question.Where(q => q.ExamID == x.ExamID).Sum(p => p.Points),2)
            }).ToList();
        }

        public ResultsListVM ExamResultForStudent(int studentID, int examID)
        {
            var list = db.ExamAnsweredQuestion.Where(e => e.StudentID == studentID).GroupBy(x => x.QuestionID).Select(s => new
            {
                Question = s.Key,
                Result = s.Sum(a => a.MarkScored)
            }).ToList();

            var exams = list.GroupBy(l => db.Question.Where(a => l.Question == a.ID && a.ExamID == examID).Select(e => e.ExamID).ToList()).Select(s => new {
                Exam = s.Key,
                Result = s.Sum(a => a.Result)
            }).ToList();


                var results = exams.GroupBy(e => e.Exam.FirstOrDefault()).Select(x => new
                {
                    Exam = x.Key,
                    Result = x.Sum(a => a.Result)
                }).ToList();

            if (results.Count() > 0)
            {
                if(results[0].Exam > 0)
                {
                    return results.Select(x => new ResultsListVM
                    {
                        StudentID = studentID,
                        ExamID = x.Exam,
                        ExamName = db.Exam.Where(e => e.ID == x.Exam).Select(e => e.Title).FirstOrDefault(),
                        Date = db.Exam.Where(e => e.ID == x.Exam).Select(e => e.ActivationDate).FirstOrDefault(),
                        CourseName = db.Exam.Where(e => e.ID == x.Exam).Select(e => db.Course.Where(c => c.ID == e.CourseID).Select(c => c.CourseName).FirstOrDefault()).FirstOrDefault(),
                        Result = Math.Round((x.Result * 100.00) / db.Question.Where(q => q.ExamID == x.Exam).Sum(p => p.Points), 2)
                    }).FirstOrDefault();

                }
                else
                    return null;
            }
            else
                return null;
        }

        public ExamResultsVM ExamResults(int examID)
        {
            List<ResultsListVM> results = new List<ResultsListVM>();

            foreach (var item in db.Student)
            {
                var studentResult = ExamResultForStudent(item.ID, examID);
                if(studentResult != null)
                results.Add(studentResult);
            }

            return new ExamResultsVM
            {
                ExamID = examID,
                Results = results.Select(s => new ExamResultsVM.StudentResults
                {
                    StudentID = s.StudentID,
                    StudentName = db.Student.Where(x => x.ID == s.StudentID).Select(x => (x.User.FirstName +' '+ x.User.LastName)).FirstOrDefault(),
                    Result = s.Result
                }).ToList()
            };
        }
    }
}

