using OnlineCourse.Model.Models.BaseEntity;
using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models
{
    public class QuestionDuration :BaseEntity
    {
        [ForeignKey(nameof(Student))]
        public int StudentID { get; set; }
        public virtual Student Student { get; set; }

        [ForeignKey(nameof(Question))]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }

        public TimeSpan RequestTime { get; set; }
        public TimeSpan LeaveTime { get; set; }
        public TimeSpan AnsweredTime { get; set; }
    }
}
