using OnlineCourse.Model.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models.Basic
{
    public class DocumentShare : BaseEntity
    {
        [ForeignKey(nameof(Document))]
        public int DocumentID { get; set; }
        public virtual Document Document { get; set; }

        [ForeignKey(nameof(Course))]
        public int CourseID { get; set; }
        public virtual Course Course { get; set; }
    }
}
