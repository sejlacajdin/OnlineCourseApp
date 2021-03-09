using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Account;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.RepositoryInterfaces
{
    public interface IQuestionRepository : IBaseRepository<Question>
    {
        public List<SelectListItem> GetQuestionCategories();
        public Question GetLastQuestion();
        public List<QuestionExamListVM> GetQuestionByExam(int examID);
        public QuestionDetailsVM GetQuestionDetails(int questionID);
        public QuestionEditVM EditQuestion(int questionID);
        public int GetLastQuestionNumber(int examID);
        public QuestionDisplayVM GetQuestionByNumber(int? qno, int examID);
        public QuestionInfoVM GetQuestionInfoByChoice(int examID, int questionNumber);
        public List<ExamAnsweredQuestion> GetPointsByChoices(QuestionInfoVM info, Answers choices, int studentID);
        public int GetNextQuestionNumber(int examID, int questionID); 
        public int GetPreviousQuestionNumber(int examID, int questionID);
        public double GetTotalExamPoints(int examID);
    }
}
