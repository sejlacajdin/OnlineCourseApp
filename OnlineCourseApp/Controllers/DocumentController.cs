using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Basic;

namespace OnlineCourseApp.Controllers
{
    [Authorize]
    public class DocumentController : NotificationsController
    {
        private IDocumentRepository documentRepository;
        private IDocumentShareRepository documentShareRepository;
        private readonly UserManager<AppUser> _userManager;

        public DocumentController(UserManager<AppUser> userManager, IDocumentRepository documentRepository, IDocumentShareRepository documentShareRepository)
        {
            _userManager = userManager;
            this.documentRepository = documentRepository;
            this.documentShareRepository = documentShareRepository;
        }
        public async Task<IActionResult> Upload(IFormFile file, int courseID)
        {
            if (file != null)
            {
                if (file.Length > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    var fileExtension = Path.GetExtension(fileName);
                    var contentType = file.ContentType;
                    var newFileName = String.Concat(Convert.ToString(Guid.NewGuid()), fileExtension);

                    string folder = "wwwroot/uploads/";
                    bool exists = Directory.Exists(folder);
                    if (!exists)
                        Directory.CreateDirectory(folder);

                    var path = Path.Combine( Directory.GetCurrentDirectory(), folder, newFileName);

                    AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                    Document doc = new Document
                    {
                        FileOldName = fileName,
                        FileName = newFileName,
                        FileExtension = fileExtension,
                        FilePath = newFileName,
                        UploadDate =DateTime.Now,
                        ContentType =contentType,
                        DocumentOwnerID = user.Id
                    };
               

                    using (var stream = new FileStream(folder + newFileName, FileMode.Create))
                    {
                        file.CopyTo(stream);
                        stream.Close();
                    }

                    documentRepository.Add(doc);
                  
                    DocumentShare docShare = new DocumentShare
                    {
                        CourseID = courseID,
                        DocumentID = documentRepository.GetLastUploaded().ID
                    };
                    documentShareRepository.Add(docShare);
                    SuccessNotification = "Uspješno ste dodali dokument.";

                }
            }
                                   
            return RedirectToAction("Detalji", "Course", new { courseID = courseID });
        }

        public IActionResult Delete(int documentID, int courseID)
        {
            Document doc = documentRepository.GetById(documentID);
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/uploads/" + doc.FileName);

            //System.IO.File.Create(fullPath).Close();

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
                
                documentRepository.Delete(documentID);
               SuccessNotification = "Uspješno ste obrisali dokument.";
            }
            else
            {
                ErrorNotification = "Greška prilikom brisanja dokumenta.";
            }
            return RedirectToAction("Detalji", "Course", new { courseID = courseID });
        }

        public async Task<IActionResult> DownloadFile(int documentID)
        {
            Document doc = documentRepository.GetById(documentID);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/", doc.FileName);

            var memory = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memory);
                stream.Close();
            }
            memory.Position = 0;
            return File(memory, "application/octet-stream", doc.FileName);
        }
    }
}