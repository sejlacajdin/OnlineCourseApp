using OnlineCourse.Model.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models.Basic
{
    public class CourseGroupStudent : BaseEntity
    {
        [ForeignKey(nameof(CourseGroup))]
        public int GroupID { get; set; }
        public virtual CourseGroup Group { get; set; }

        [ForeignKey(nameof(CourseParticipants))]
        public int CourseParticipantID { get; set; }
        public virtual CourseParticipants CourseParticipant { get; set; }
    }
}
