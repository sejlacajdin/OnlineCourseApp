using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class QuestionDisplayVM
    {
        public int QuestionID { get; set; }
        public int TotalQuestionInSet { get; set; }
        public int QuestionNumber { get; set; }
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string Question { get; set; }
        public QuestionType QuestionType { get; set; }
        public double Point { get; set; }
        public List<QuestionOption> Options { get; set; }
    }

    public class QuestionOption
    {
        public int ChoiceID { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
    }
}
