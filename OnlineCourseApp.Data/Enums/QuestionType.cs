using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Enums
{
    public enum QuestionType
    {
        [Display(Name = "Više ponuđenih više tačnih")]
        MultipleChoice = 1,

        [Display(Name = "Više ponuđenih jedno tačno")]
        Matching = 2,

        [Display(Name = "Esej")]
        Essay = 3,
 
    }
}
