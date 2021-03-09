using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class QuestionEditVM
    {
        public int QuestionID { get; set; }
        public int ExamID { get; set; }
        public int QuestionCategoryID { get; set; }
        public QuestionCategory QuestionCategory { get; set; }
        public QuestionType QuestionType { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Bodovi")]
        public double Points { get; set; }
        public bool IsActive { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Tekst pitanja")]
        public string Text { get; set; }
        public List<SelectListItem> QuestionCategoryList { get; set; }
        public List<SelectListItem> QuestionTypeList { get; set; }
    }
}
