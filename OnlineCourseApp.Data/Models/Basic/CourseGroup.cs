using OnlineCourse.Model.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models.Basic
{
    public class CourseGroup : BaseEntity
    {
        public string Name { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
        public virtual List<CourseGroupStudent> GroupStudents { get; set; }
    }
}
