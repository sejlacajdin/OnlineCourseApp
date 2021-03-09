using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;

namespace OnlineCourseApp.Controllers
{
    public class RegionController : NotificationsController
    {
        private IRegionRepository regionRepository;
        public RegionController(IRegionRepository regionRepository)
        { this.regionRepository = regionRepository; }

        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult DodajForma()
        {
            return PartialView();

        }

        [Authorize(Roles = "Admin")]
        public IActionResult Dodaj(string description)
        {
            var region = new Region
            {
                Description = description
            };
            regionRepository.Add(region);
            SuccessNotification = "Uspješno ste dodali regiju.";
            return Redirect("/Region/Prikaz");
        }

        [Authorize(Roles = "Admin,Profesor,Student")]
        public IActionResult Prikaz()
        {
            var regije = regionRepository.GetAllRegions();
            return View(regije);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Obrisi(int regionID)
        {
            regionRepository.Delete(regionID);
            SuccessNotification = "Uspješno ste obrisali regiju.";
            return Redirect(url: "/Region/Prikaz");
        }
    }
}