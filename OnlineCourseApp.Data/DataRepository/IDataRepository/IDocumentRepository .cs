using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.Migrations;
using OnlineCourseApp.Data.Models.Announcement;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository.IDataRepository
{
    public interface IDocumentRepository : IBaseRepository<Document>
    {
        public Document GetLastUploaded();
       
    }
}
