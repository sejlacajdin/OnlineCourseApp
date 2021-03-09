using OnlineCourse.Model.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models
{
    public class Choice : BaseEntity
    {
        [ForeignKey(nameof(Question))]
        public int QuestionID { get; set; }
        public virtual Question Question { get; set; }
        public string Text { get; set; }
        public double Points { get; set; }
        public bool? IsCorrect { get; set; }
    }
}
