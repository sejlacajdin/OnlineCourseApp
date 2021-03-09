using OnlineCourseApp.Data.Models.Basic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
    public class CourseSectionEditVM
    {
        public int CourseSectionID { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Naziv sekcije")]
        public string Name { get; set; }

        [MaxLength(50, ErrorMessage = "Maksimalan broj znakova je 50")]
        [DisplayName("Opis")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Obavezno polje")]
        [DisplayName("Tip kursa")]
        public int CourseTypeID { get; set; }
        public virtual CourseType CourseType { get; set; }

        [DisplayName("Glavna oblast")]
        public int? CourseParentID { get; set; }
        public virtual CourseSection CourseParent { get; set; }

    }
}
