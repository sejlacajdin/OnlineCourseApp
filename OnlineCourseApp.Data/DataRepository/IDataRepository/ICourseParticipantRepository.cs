using OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.RepositoryInterfaces
{
    public interface ICourseParticipantRepository : IBaseRepository<CourseParticipants>
    {
        public bool ApplicationExist(CourseParticipants application);
        public List<CourseParticipantsVM> GetCourseParticipantList(int courseID);
 
    }

}
