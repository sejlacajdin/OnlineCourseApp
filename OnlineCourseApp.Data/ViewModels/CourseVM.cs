using OnlineCourseApp.Data.Migrations;
using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
   public class CourseVM
    {
        public int CourseID { get; set; }
        public int ProfessorId { get; set; }
        [DisplayName("Naziv kursa")]
        public string CourseName { get; set; }
        [DisplayName("Početak")]
        public DateTime Start { get; set; }
        [DisplayName("Kraj")]
        public DateTime End { get; set; }
        [DisplayName("Bilješka")]
        public string Notes { get; set; }
        public int CourseSectionID { get; set; }
        public string CourseSection { get; set; }
        public virtual List<CourseParticipantsVM> CourseParticipants { get; set; }
        public virtual List<ExamVM> Exams { get; set; }


    }
}
