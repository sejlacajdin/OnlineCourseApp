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
    public class DocumentShareRepository : BaseRepository<DocumentShare>, IDocumentShareRepository
    {
        public DocumentShareRepository(MyDBContext db) : base(db)
        {
           
        }
        public List<DocumentPreviewVM> GetDocumentByUserAndCourse(int userID, int courseID)
        {

            return db.DocumentShare.Include(d => d.Document).Where(d => d.Document.DocumentOwnerID == userID && d.CourseID == courseID).Select(e => new DocumentPreviewVM
            {
                DocumentID = e.DocumentID,
                FileName = e.Document.FileOldName
            }).ToList();
         }

        public List<DocumentPreviewVM> GetDocumentByCourse(int courseID)
        {

            return db.DocumentShare.Include(d => d.Document).Where(d => d.CourseID == courseID).Select(e => new DocumentPreviewVM
            {
                DocumentID = e.DocumentID,
                FileName = e.Document.FileOldName
            }).ToList();
        }

    }
}
