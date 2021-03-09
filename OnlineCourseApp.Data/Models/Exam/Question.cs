using OnlineCourse.Model.Models.BaseEntity;
using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models
{
    public class Question : BaseEntity
    {
        [ForeignKey(nameof(QuestionCategory))]
        public int QuestionCategoryID { get; set; }
        public virtual QuestionCategory QuestionCategory { get; set; }

        public QuestionType QuestionType { get; set; }
        public int? QuestionNumber { get; set; }

        [ForeignKey(nameof(Exam))]
        public int ExamID { get; set; }
        public virtual Exam Exam { get; set; }
        public double Points { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }
    }
}
