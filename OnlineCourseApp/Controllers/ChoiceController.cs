using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;

namespace OnlineCourseApp.Controllers
{
    [Authorize]
    public class ChoiceController : NotificationsController
    {
        private IChoiceRepository choiceRepository;

        public ChoiceController(IChoiceRepository choiceRepository)
        {
            this.choiceRepository = choiceRepository;
        }

        public IActionResult Delete(int choiceID, int questionID, int examID)
        {
            choiceRepository.Delete(choiceID);
            SuccessNotification = "Uspješno ste obrisali odgovor.";

            return RedirectToAction("Details", "Question", new {  questionID,  examID });
        }

        public IActionResult Edit(int choiceID, int examID)
        {
            var choice = choiceRepository.EditChoice(choiceID, examID);

            if (choice == null)
            {
                ViewBag.ErrorMessage = "Odgovor nije pronađen.";
                return View("_NotFound");
            }

            choice.correctList.Add(new SelectListItem { Text = "Tačno", Value = "True" });
            choice.correctList.Add(new SelectListItem { Text = "Netačno", Value = "False" });

            return PartialView(choice);
        }

        public IActionResult SpremiIzmjene(ChoiceEditVM m)
        {
            var choice = choiceRepository.GetById(m.ChoiceID);
            if (choice == null)
            {
                ViewBag.ErrorMessage = "Odgovor nije pronađen.";
                return View("_NotFound");
            }
            else
            {
                
                choice.Text = m.Text;
                choice.Points = m.Points;
                choice.IsCorrect = m.IsCorrect;

                choiceRepository.Update(choice);
                SuccessNotification = "Uspješno ste ažurirali odgovor.";
            }

            return RedirectToAction("Details", "Question", new { questionID = m.QuestionID, examID = m.ExamID });
        }

    }
}