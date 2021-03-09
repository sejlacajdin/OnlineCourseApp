using Microsoft.EntityFrameworkCore;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models.Announcement;
using OnlineCourseApp.Data.Models.Basic;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class DocumentRepository : BaseRepository<Document>, IDocumentRepository
    {
        public DocumentRepository(MyDBContext db) : base(db)
        {
            
        }

        public Document GetLastUploaded()
        {
            int max = db.Document.Max(p => p.ID);

            return db.Document.Find(max);
        }



    }
}
