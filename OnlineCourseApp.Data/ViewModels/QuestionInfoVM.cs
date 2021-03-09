using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class QuestionInfoVM
    {
        public int QuestionID { get; set; }
        public int QuestionCategoryID { get; set; }
        public QuestionType QuestionType { get; set; }
        public int? QuestionNumber { get; set; }
        public int ExamID { get; set; }
        public double Points { get; set; }
    }
}
