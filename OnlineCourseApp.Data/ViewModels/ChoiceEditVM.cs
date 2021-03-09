using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ChoiceEditVM
    {
        public int ChoiceID { get; set; }
        public int QuestionID { get; set; }
        public int ExamID { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Odgovor")]
        public string Text { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Bodovi")]
        public double Points { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        public bool? IsCorrect { get; set; }

        public List<SelectListItem> correctList { get; set; }
    }
}
