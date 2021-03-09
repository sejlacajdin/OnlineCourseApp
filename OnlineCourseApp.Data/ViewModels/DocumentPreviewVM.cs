using Microsoft.AspNetCore.Http;
using OnlineCourseApp.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace OnlineCourseApp.Data.ViewModels
{
   public class DocumentPreviewVM
    {
        public int DocumentID { get; set; }
        public string FileName { get; set; }
    }
}
