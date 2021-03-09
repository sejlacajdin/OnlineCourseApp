using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ResultsListVM
    {
        public int StudentID { get; set; }
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string CourseName { get; set; }
        public DateTime Date { get; set; }
        public double Result { get; set; }
    }
}
