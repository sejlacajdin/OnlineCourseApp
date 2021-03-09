using OnlineCourse.Model.Models.BaseEntity;
using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models
{
    public class Exam : BaseEntity
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime DeactivationDate { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool RandomizeQuestions { get; set; }
        public bool IsActive { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

        [ForeignKey(nameof(AppUser))]
        public int ExamOwnerID { get; set; }
        public virtual AppUser ExamOwner { get; set; }
    }
}
