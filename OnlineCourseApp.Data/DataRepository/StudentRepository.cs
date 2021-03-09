using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class StudentRepository : BaseRepository<Student>, IStudentRepository
    {
        public StudentRepository(MyDBContext db) : base(db) { }

        public int GetCurrentStudentId(int userId)
        {

            return db.Student.Where(s => s.User.Id == userId).Select(student => student.ID).FirstOrDefault(); 
        }

        public Student GetStudent(int userId)
        {
            return db.Student.Where(s => s.User.Id == userId).FirstOrDefault();
        }
    }
}
