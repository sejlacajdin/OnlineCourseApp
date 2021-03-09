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
    public interface IExamAnsweredQuestionRepository : IBaseRepository<ExamAnsweredQuestion>
    {
        public List<ExamAnsweredQuestion> GetAnswers(int questionID, int studentID);
        public bool ExamTaken(int studentID, int examID);
        public bool ExamTakenForProfessor(int examID);
        public List<ResultsListVM> ResultsForStudent(int studentID);
        public ResultsListVM ExamResultForStudent(int studentID, int examID);
        public ExamResultsVM ExamResults(int examID);
    }
}
