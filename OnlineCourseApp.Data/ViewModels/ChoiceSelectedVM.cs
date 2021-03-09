using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ChoiceSelectedVM
    {
        public int ChoiceID { get; set; }
        public string IsChecked { get; set; }
    }

    public class Answers
    {
        public int ExamID { get; set; }
        public int QuestionID { get; set; }
        public List<ChoiceSelectedVM> UserChoices { get; set; }
        public string Answer { get; set; }
        public string Direction { get; set; }

        public List<int> UserSelectedID
        {
            get
            {
                return UserChoices == null? new List<int>() : UserChoices.Where(x => x.IsChecked == "on" || "true".Equals(x.IsChecked, StringComparison.InvariantCultureIgnoreCase)).Select(x => x.ChoiceID).ToList();
            }
        }
    }
}
