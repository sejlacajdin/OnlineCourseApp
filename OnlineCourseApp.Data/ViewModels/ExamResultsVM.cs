using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ExamResultsVM
    {
        public int ExamID { get; set; }
        public List<StudentResults> Results { get; set; }
        public class StudentResults
        {
            public int StudentID { get; set; }
            public string StudentName { get; set; }
            public double Result { get; set; }
        }
    }
}
