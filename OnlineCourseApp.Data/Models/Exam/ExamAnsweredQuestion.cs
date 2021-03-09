using OnlineCourse.Model.Models.BaseEntity;
using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models
{
    public class ExamAnsweredQuestion : BaseEntity
    {
        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(Question))]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        [ForeignKey(nameof(Choice))]
        public int ChoiceID { get; set; }
        public virtual Choice Choice { get; set; }
        public string Answer { get; set; }
        public double MarkScored { get; set; }
    }
}
