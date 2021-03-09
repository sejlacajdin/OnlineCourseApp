using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ExamDetailsVM
    {
        public int ExamID { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime DeactivationDate { get; set; }
        public TimeSpan TimeLimit { get; set; }
        public bool RandomizeQuestions { get; set; }
        public int NumOfQuestions { get; set; }
        public int CourseID { get; set; }
        public string Course { get; set; }
        public int ExamOwnerID { get; set; }
        public string ExamOwner { get; set; }
    }
}
