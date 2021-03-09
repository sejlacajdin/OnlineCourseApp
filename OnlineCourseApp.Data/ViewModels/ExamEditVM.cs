using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class ExamEditVM
    {
        public int ExamID { get; set; }
        [Required(ErrorMessage = "Obavezno polje")]
        [MaxLength(60, ErrorMessage = "Maksimalan broj znakova je 60")]
        [DisplayName("Naslov")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [MaxLength(200, ErrorMessage = "Maksimalan broj znakova je 200")]
        [DisplayName("Upute")]
        public string Instructions { get; set; }

        [DisplayName("Datum aktivacije")]
        public DateTime ActivationDate { get; set; }
        [DisplayName("Datum deaktivacije")]
        public DateTime DeactivationDate { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Trajanje u minutama")]
        public int TimeLimit { get; set; }
        public bool Active { get; set; }
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }

    }
}
