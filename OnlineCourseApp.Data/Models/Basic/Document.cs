using OnlineCourse.Model.Models.BaseEntity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace OnlineCourseApp.Data.Models.Basic
{
   public class Document:BaseEntity
    {
        public string FileName { get; set; }
        public string FileExtension { get; set; }
        public string FilePath { get; set; }
        public DateTime UploadDate { get; set; }
        public string ContentType { get; set; }
        [ForeignKey(nameof(DocumentOwner))]
        public int DocumentOwnerID { get; set; }
        public virtual AppUser DocumentOwner { get; set; }
        public string FileOldName { get; set; }
    }
}
