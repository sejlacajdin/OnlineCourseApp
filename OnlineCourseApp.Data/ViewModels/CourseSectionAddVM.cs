using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class CourseSectionAddVM
    {
        public int CourseSectionID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [MaxLength(30, ErrorMessage = "Maksimalan broj znakova je 30")]
        [DisplayName("Oblast kursa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [MaxLength(50, ErrorMessage = "Maksimalan broj znakova je 50")]
        [DisplayName("Opis")]
        public string Description { get; set; }

        [DisplayName("Tip kursa")]
        public int CourseTypeID { get; set; }
        public string CourseType { get; set; }

        [DisplayName("Glavna oblast kursa")]
        public int? CourseParentID { get; set; }
        public string CourseParent { get; set; }

    }
}
