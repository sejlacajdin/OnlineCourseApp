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
    public interface IExamRepository : IBaseRepository<Exam>
    {
       public List<ExamPreviewVM> GetAllExams();
        public ExamEditVM EditExam(int examID);
        public ExamDetailsVM GetExamById(int examID);
        public List<ExamPreviewVM> GetAllExamsByProfessor(int ownerID);
        public List<ExamPreviewVM> GetAllExamsByAppliedCourses(int userID);

    }
}
