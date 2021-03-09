using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Enums;
using OnlineCourseApp.Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class QuestionAddVM
    {
        public int QuestionID { get; set; }
        public int QuestionCategoryID { get; set; }
        public List<SelectListItem> QuestionCategory { get; set; }
        public int QuestionTypeID { get; set; }
        public List<SelectListItem> QuestionType { get; set; }
        public int ExamID { get; set; }
        public virtual Exam Exam { get; set; }
        public int QuestionNumber { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Bodovi")]
        public double Points { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Tekst pitanja")]
        public string Text { get; set; }
    }
}
