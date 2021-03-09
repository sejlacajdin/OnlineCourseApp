using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using OnlineCourseApp.Data.DataRepository.IDataRepository;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Announcement;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.Enums;
using OnlineCourseApp.SignalR.Hubs;

namespace OnlineCourseApp.Controllers
{
    public class AnnouncementController : BaseController
    {
        private IAnnouncementRepository _announcementRepository;
        private readonly UserManager<AppUser> _userManager;
        private IHubContext<NotificationHub> _hubContext;

        public AnnouncementController(IAnnouncementRepository announcementRepository,
                                       UserManager<AppUser> userManager,
                                        IHubContext<NotificationHub> hubContext)
        {
            _announcementRepository = announcementRepository;
            _userManager = userManager;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            Ref = "Index";

            if (Permission == "Profesor")
            {
                return RedirectToAction("AnnouncementsForProfessors");
            }
            else if (Permission == "Student")
            {
                return RedirectToAction("AnnouncementsForStudents");

            }
            List<AnnouncementsVM> announcements = _announcementRepository.GetAllAnnouncements();
            return View(announcements);
        }

        public async Task<IActionResult> New()
        {
            AnnouncementNewVM model = new AnnouncementNewVM()
            {
                AnnouncementFilter = new List<SelectListItem>()
            };
            model.AnnouncementFilter.Add(new SelectListItem { Text = "Svi korisnici", Value = ((int)AnnouncementFilterType.All).ToString() });
            model.AnnouncementFilter.Add(new SelectListItem { Text = "Svi korisnici i websajt", Value = ((int)AnnouncementFilterType.AllWithWebsite).ToString() });
            model.AnnouncementFilter.Add(new SelectListItem { Text = "Svi profesori", Value = ((int)AnnouncementFilterType.AllProfessors).ToString() });
            model.AnnouncementFilter.Add(new SelectListItem { Text = "Svi studenti", Value = ((int)AnnouncementFilterType.AllStudents).ToString() });
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewBag.Name = user.FirstName + " " + user.LastName;

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> New(AnnouncementNewVM vm)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Announcements announcement = new Announcements
            {
                Title = vm.Title,
                Description = vm.Description,
                ShortDescription = vm.ShortDescription,
                FilterType = (AnnouncementFilterType)vm.AnnouncementFilterID,
                RecordCreated = DateTime.Now,
                PostOwner = user.FirstName + " " + user.LastName,
                AnnouncementOwnerID = user.Id,
            };
            _announcementRepository.Add(announcement);
            SendMessage("changed", announcement.FilterType);

            return RedirectToAction("Index");
        }

        public IActionResult Details(int announcementID)
        {
            Announcements x = _announcementRepository.GetById(announcementID);
            AnnouncementsVM model = new AnnouncementsVM
            {
                ID = x.ID,
                AnnouncementOwnerID = x.AnnouncementOwnerID,
                AnnouncementFilter = x.FilterType,
                PostOwner = x.PostOwner,
                RecordCreated = x.RecordCreated,
                Description = x.Description,
                ShortDescription = x.ShortDescription,
                Title = x.Title,
            };
            return View(model);
        }

        public IActionResult Delete(int announcementID)
        {
            _announcementRepository.Delete(announcementID);
            return RedirectToAction("Index");
        }

        public IActionResult AnnouncementsForStudents()
        {
            Ref = "AnnouncementsForStudents";
            List<AnnouncementsVM> announcements = _announcementRepository.GetAllAnnouncements(AnnouncementFilterType.AllStudents);
            return View(announcements);
        }

        public async Task<IActionResult> AnnouncementsForProfessors()
        {
            Ref = "AnnouncementsForProfessors";
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<AnnouncementsVM> announcements = _announcementRepository.GetAllAnnouncementsForProfessor(user.Id);
            return View(announcements);
        }


        private void SendMessage(string message, AnnouncementFilterType type)
        {
            if (type == AnnouncementFilterType.All)
            {           
                    _hubContext.Clients.Group("PublicGroup").SendAsync("ReceiveNotification", message);   
            }
            else if(type == AnnouncementFilterType.AllProfessors)
            {
                _hubContext.Clients.Group("Profesor").SendAsync("ReceiveNotification", message);
            }
            else if (type == AnnouncementFilterType.AllStudents)
            {
                _hubContext.Clients.Group("Student").SendAsync("ReceiveNotification", message);

            }

        }
    }
}