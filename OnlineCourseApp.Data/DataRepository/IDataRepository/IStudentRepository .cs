using OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.RepositoryInterfaces
{
    public interface IStudentRepository : IBaseRepository<Student>
    {
        public int GetCurrentStudentId(int userId);
        public Student GetStudent(int userId);
    }

}
