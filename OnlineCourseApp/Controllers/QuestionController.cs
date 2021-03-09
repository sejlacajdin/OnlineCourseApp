using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Enums;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace OnlineCourseApp.Controllers
{
    public class QuestionController : NotificationsController
    {
        private IQuestionRepository questionRepository;
        private IQuestionCategoryRepository _questionCategoryRepository;
        private IChoiceRepository choiceRepository;

        public QuestionController( IQuestionRepository questionRepository, IChoiceRepository choiceRepository, IQuestionCategoryRepository questionCategoryRepository)
        {
            this.questionRepository = questionRepository;
            this.choiceRepository = choiceRepository;
            _questionCategoryRepository = questionCategoryRepository;
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult DodajForma(int examID)
        {
            ViewBag.ExamID = examID;
            QuestionAddVM model = new QuestionAddVM() { 
                QuestionCategory = new List<SelectListItem>(),
                QuestionType = new List<SelectListItem>()
            };
            model.QuestionCategory = questionRepository.GetQuestionCategories();

            model.QuestionType.Add(new SelectListItem { Text = "Više ponuđenih više tačnih", Value = ((int)QuestionType.MultipleChoice).ToString() });
            model.QuestionType.Add(new SelectListItem { Text = "Više ponuđenih jedno tačno", Value = ((int)QuestionType.Matching).ToString() });
            model.QuestionType.Add(new SelectListItem { Text = "Esej", Value = ((int)QuestionType.Essay).ToString() });

            return View(model);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult Dodaj(QuestionAddVM model, List<string> Answer, IFormCollection collection, List<string> Percentage)
        {

            if (!ModelState.IsValid)
            {
                return Redirect("/Question/DodajForma?examID=" + model.ExamID);
            }


            if (Answer.Count > 0 && Answer.Contains(null))
            {
                //error message
                ErrorNotification = "Unesite odgovore.";
                return Redirect("/Question/DodajForma?examID=" + model.ExamID);

            };

            var results = collection["checkboxes"];

            if (Answer.Count > 0 && results.Count == 0)
            {
                //error message
                ErrorNotification = "Označite tačne odgovore.";
                return Redirect("/Question/DodajForma?examID=" + model.ExamID);
            };

            Question question = new Question
            {
                QuestionCategoryID = model.QuestionCategoryID,
                QuestionType = (QuestionType)model.QuestionTypeID,
                ExamID = model.ExamID,
                QuestionNumber = questionRepository.GetLastQuestionNumber(model.ExamID) +1,
                Points = model.Points,
                IsActive =true,
                Text = model.Text
            };

            questionRepository.Add(question);

            if (Answer.Count > 0)
            {
        
                for(var i = 0; i < Answer.Count; i++)
                {
                    Choice choice = new Choice
                    {
                        QuestionID = questionRepository.GetLastQuestion().ID,
                        Text = Answer[i],
                        IsCorrect = results.Contains(i.ToString()) ? true: false,
                        Points = model.QuestionTypeID == 1 ? Convert.ToDouble(Percentage[i]) : (results.Contains(i.ToString()) ? 100 : 0)
                    };
                    choiceRepository.Add(choice);
                }
            }
            else
            {
                Choice choice = new Choice
                {
                    QuestionID = questionRepository.GetLastQuestion().ID,
                    Text = "",
                    IsCorrect =  true,
                    Points = model.Points
                };
                choiceRepository.Add(choice);
            };


            SuccessNotification = "Uspješno ste dodali pitanje.";

            return Redirect("/Question/DodajForma?examID=" + model.ExamID);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult Delete(int questionID, int examID)
        {
            var questions = questionRepository.GetAll();
            var deletedQuestion = questionRepository.GetById(questionID);

            foreach (var item in questions)
            {
                if (item.ExamID == examID && item.QuestionNumber > deletedQuestion.QuestionNumber)
                    item.QuestionNumber -= 1;
            }

            questionRepository.Delete(questionID);
            SuccessNotification = "Uspješno ste obrisali pitanje.";
            return RedirectToAction("Details", "Exam", new { examID = examID });
        }

        [Authorize(Roles = "Admin, Profesor, Student")]
        public IActionResult Details(int questionID, int examID)
        {
            var question = questionRepository.GetQuestionDetails(questionID);
            ViewBag.ExamID = examID;

            if (question == null)
            {
                ViewBag.ErrorMessage = "Pitanje nije pronađen.";
                return View("_NotFound");
            }
            return View(question);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult Edit(int questionID, int examID)
        {
            var question = questionRepository.EditQuestion(questionID);

            if (question == null)
            {
                ViewBag.ErrorMessage = "Pitanje nije pronađeno.";
                return View("_NotFound");
            }

            question.QuestionCategoryList = questionRepository.GetQuestionCategories();

            question.QuestionTypeList.Add(new SelectListItem { Text = "Više ponuđenih više tačnih", Value = ((int)QuestionType.MultipleChoice).ToString() });
            question.QuestionTypeList.Add(new SelectListItem { Text = "Više ponuđenih jedno tačno", Value = ((int)QuestionType.Matching).ToString() });
            question.QuestionTypeList.Add(new SelectListItem { Text = "Esej", Value = ((int)QuestionType.Essay).ToString() });

            return PartialView(question);
        }

        [Authorize(Roles = "Admin, Profesor")]
        public IActionResult SpremiIzmjene(QuestionEditVM m)
        {
            var question = questionRepository.GetById(m.QuestionID);
            if (question == null)
            {
                ViewBag.ErrorMessage = "Pitanje nije pronađeno.";
                return View("_NotFound");
            }
            else
            {
                question.QuestionCategoryID = m.QuestionCategoryID;
                question.QuestionType = m.QuestionType;
                question.Text = m.Text;
                question.Points = m.Points;
      
                questionRepository.Update(question);
                SuccessNotification = "Uspješno ste ažurirali pitanje.";
            }

            return RedirectToAction("Details", "Exam", new { examID = m.ExamID });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddQuestionCategoryForm()
        {
            return PartialView();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AddQuestionCategory(string categoryName)
        {
            QuestionCategory category = new QuestionCategory { CategoryName = categoryName };
            _questionCategoryRepository.Add(category);
            SuccessNotification = "Uspješno ste dodali kategoriju.";

            return RedirectToAction("AdminIndex","Exam");
        }

    }
}