using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
   public  class QuestionDetailsVM
    {
        public int QuestionID { get; set; }
        public int QuestionCategoryID { get; set; }
        public string QuestionCategory { get; set; }
        public QuestionType QuestionType { get; set; }
        public int QuestionNumber { get; set; }
        public double Points { get; set; }
        public bool IsActive { get; set; }
        public string Text { get; set; }
        public List<Choices> Choises { get; set; }

        public class Choices
        {
            public int ChoiceID { get; set; }
            public string Text { get; set; }
            public double Points { get; set; }
            public string IsCorrect { get; set; }
        }
    }
}
