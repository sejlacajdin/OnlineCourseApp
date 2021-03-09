using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;

namespace OnlineCourseApp.Controllers
{
    public class ExamController : NotificationsController
    {
        private ICourseRepository courseRepository;
        private IExamRepository examRepository;
        private IQuestionRepository questionRepository;
        private readonly UserManager<AppUser> _userManager;
        private IStudentRepository _studentRepository;
        private IExamAnsweredQuestionRepository _examAnsweredQuestionRepository;

        public ExamController(UserManager<AppUser> userManager, ICourseRepository courseRepository, IExamRepository examRepository, IQuestionRepository questionRepository,
                                IStudentRepository studentRepository, IExamAnsweredQuestionRepository examAnsweredQuestionRepository)
        {
            _userManager = userManager;
            this.courseRepository = courseRepository;
            this.examRepository = examRepository;
            this.questionRepository = questionRepository;
            _studentRepository = studentRepository;
            _examAnsweredQuestionRepository = examAnsweredQuestionRepository;
        }

        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> Index()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["exams"] = examRepository.GetAllExamsByProfessor(user.Id);
            return View();
        }
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentIndex()
        {
             AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            int studentID = _studentRepository.GetCurrentStudentId(user.Id);
            List<ExamPreviewVM> exams = examRepository.GetAllExamsByAppliedCourses(studentID);
            List<ExamPreviewVM> _exams = exams.Where(e => _examAnsweredQuestionRepository.ExamTaken(studentID, e.ExamID) == false).ToList();
            ViewData["exams"] = _exams;
            return View();
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex()
        {
            List<ExamPreviewVM> _exams = examRepository.GetAllExams();
            ViewData["exams"] = _exams;

            return View();
        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> DodajForma()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            ViewData["courses"] = courseRepository.GetCourseListByOwner(user.Id);
            ExamAddVM Model = new ExamAddVM();
            return PartialView(Model);

        }

        [Authorize(Roles = "Profesor")]
        public async Task<IActionResult> Dodaj(ExamAddVM m)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            Exam exam = new Exam
            {
                Title = m.Title,
                Instructions = m.Instructions,
                RandomizeQuestions = false,
                ActivationDate = m.ActivationDate,
                DeactivationDate = m.DeactivationDate,
                CourseID = m.CourseID,
                ExamOwnerID = user.Id,
                IsActive = true,
                TimeLimit = new TimeSpan(0, m.TimeLimit, 0)

        };
            examRepository.Add(exam);
            SuccessNotification = "Uspješno ste dodali ispit.";

            return RedirectToAction("Index", "Exam");
        }

        [Authorize(Roles = "Profesor,Admin")]
        public IActionResult Edit(int examID)
        {
            ViewData["courses"] = courseRepository.GetCourseList();

            ViewBag.ExamActive = new List<SelectListItem>
            {
                new SelectListItem {Value = "True" , Text="Aktivan" },
                new SelectListItem {Value = "False" , Text="Neaktivan" }
            };
            var exam = examRepository.EditExam(examID);

            if (exam == null)
            {
                ViewBag.ErrorMessage = "Ispit nije pronađen.";
                return View("_NotFound");
            }

            return PartialView(exam);
        }

        [Authorize(Roles = "Profesor,Admin")]
        public async Task<IActionResult> SpremiIzmjene(ExamEditVM m)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            var exam = examRepository.GetById(m.ExamID);
            if (exam == null)
            {
                ViewBag.ErrorMessage = "Ispit nije pronađen.";
                return View("_NotFound");
            }
            else
            {
                exam.Title = m.Title;
                exam.Instructions = m.Instructions;
                exam.RandomizeQuestions = false;
                exam.ActivationDate = m.ActivationDate;
                exam.DeactivationDate = m.DeactivationDate;
                exam.CourseID = m.CourseID;
                exam.IsActive = m.Active;
                exam.ExamOwnerID = user.Id;
                exam.TimeLimit = new TimeSpan(0, m.TimeLimit, 0);

                 examRepository.Update(exam);
                SuccessNotification = "Uspješno ste ažurirali ispit.";
            }

            return RedirectToAction("Index", "Exam");
        }

        [Authorize(Roles = "Profesor,Admin")]
        public IActionResult Delete(int examID)
        {
            examRepository.Delete(examID);
            SuccessNotification = "Uspješno ste obrisali ispit.";
            return RedirectToAction("Index", "Exam");
        }

        [Authorize(Roles = "Profesor,Admin")]
        public IActionResult Details(int examID)
        {
            var exam = examRepository.GetExamById(examID);
            ViewBag.Questions = questionRepository.GetQuestionByExam(examID);

            if (exam == null)
            {
                ViewBag.ErrorMessage = "Ispit nije pronađen.";
                return View("_NotFound");
            }
            return View(exam);
        }

        [Authorize(Roles = "Student")]
        public IActionResult DetailsStudent(int examID)
        {
            var exam = examRepository.GetExamById(examID);
            

            if (exam == null)
            {
                ViewBag.ErrorMessage = "Ispit nije pronađen.";
                return View("_NotFound");
            }
            return View(exam);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> EvalPage(int? qno, int examID)
        {
            if (qno.GetValueOrDefault() < 1)
                qno = 1;

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            int studentID = _studentRepository.GetCurrentStudentId(user.Id);

            QuestionDisplayVM model = questionRepository.GetQuestionByNumber(qno, examID);
            if (model == null)
                return RedirectToAction("StudentIndex","Exam");

            var savedAnswers = _examAnsweredQuestionRepository.GetAnswers(model.QuestionID, studentID);

            ViewBag.TimeExpire = examRepository.GetExamById(examID).TimeLimit;
            //TimeSpan ts = new TimeSpan(0, 0, 30);
            ViewBag.Time = examRepository.GetExamById(examID).TimeLimit.ToString();

            foreach (var item in savedAnswers)
            {
                model.Options.Where(x => x.ChoiceID == item.ChoiceID).FirstOrDefault().Answer = item.Answer;
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Student")]
        public async Task<IActionResult> PostAnswer(Answers choices)
        {
            var examQuestionInfo = questionRepository.GetQuestionInfoByChoice(choices.ExamID, choices.QuestionID);
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            int studentID = _studentRepository.GetCurrentStudentId(user.Id);

            if (examQuestionInfo != null)
            {
                List<ExamAnsweredQuestion> previousSavedAnswers = _examAnsweredQuestionRepository.GetAnswers(choices.QuestionID, studentID);

                if(previousSavedAnswers.Count > 0)
                {
                    foreach (var item in previousSavedAnswers)
                    {
                        _examAnsweredQuestionRepository.Delete(item.ID);
                    }
                }

                if (choices.UserChoices.Count > 1)
                {
                    var allPointValueOfChoices = questionRepository.GetPointsByChoices(examQuestionInfo, choices, studentID);
                    _examAnsweredQuestionRepository.AddRange(allPointValueOfChoices);
                }
                else
                {
                    var examAnswers = new ExamAnsweredQuestion
                    {
                        StudentID = studentID,
                        QuestionID = examQuestionInfo.QuestionID,
                        ChoiceID = choices.UserChoices.FirstOrDefault().ChoiceID,
                        Answer = choices.Answer,
                        MarkScored = 1
                      };
                    _examAnsweredQuestionRepository.Add(examAnswers);
                }
            }

            var nextQuestionNumber = 1;
            if(choices.Direction.Equals("forward", StringComparison.CurrentCultureIgnoreCase))
            {
                nextQuestionNumber = questionRepository.GetNextQuestionNumber(choices.ExamID, choices.QuestionID);
            }
            else
            {
                nextQuestionNumber = questionRepository.GetPreviousQuestionNumber(choices.ExamID, choices.QuestionID);
            }

            if (nextQuestionNumber < 1)
                nextQuestionNumber = 1;
            var id = questionRepository.GetById(examQuestionInfo.QuestionID).QuestionNumber;

            if (id == questionRepository.GetLastQuestionNumber(choices.ExamID))
            return RedirectToAction("ExamFinished", "Results", new { examID = choices.ExamID });
            else
            return RedirectToAction("EvalPage", new { qno = nextQuestionNumber , examID = choices.ExamID});
        }

    }
}