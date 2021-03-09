using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.Models.Account;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class CourseParticipantsVM
    {
        public int CoursePaxID { get; set; }
        public int StudentID { get; set; }
        public string StudentIDNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
