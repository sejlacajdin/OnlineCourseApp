using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class CourseParticipantRepository : BaseRepository<CourseParticipants>, ICourseParticipantRepository
    {
        public CourseParticipantRepository(MyDBContext db) : base(db) { }

        public bool ApplicationExist(CourseParticipants application)
        {
            return db.CourseParticipants.Any(item => item.CourseID == application.CourseID && item.StudentID == application.StudentID);

        }
        public List<CourseParticipantsVM> GetCourseParticipantList(int courseID)
        {
            return db.CourseParticipants.Where(c => c.CourseID == courseID).Select(part => new CourseParticipantsVM
            {
                CoursePaxID = part.ID,
                StudentIDNumber = part.Student.StudentIDNumber,
                FirstName = part.Student.User.FirstName,
                LastName = part.Student.User.LastName,
                Email = part.Student.User.Email
            }).ToList();
        }
    }

}