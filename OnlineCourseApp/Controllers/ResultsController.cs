using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;

namespace OnlineCourseApp.Controllers
{
    public class ResultsController : NotificationsController
    {
        private readonly UserManager<AppUser> _userManager;
        private IStudentRepository _studentRepository;
        private IExamAnsweredQuestionRepository _examAnsweredQuestionRepository;
        private IExamRepository _examRepository;
        private IQuestionRepository _questionRepository;

        public ResultsController(UserManager<AppUser> userManager, IStudentRepository studentRepository, IExamAnsweredQuestionRepository examAnsweredQuestionRepository,
                                 IExamRepository examRepository, IQuestionRepository questionRepository)
        {
            _userManager = userManager;         
            _studentRepository = studentRepository;
            _examAnsweredQuestionRepository = examAnsweredQuestionRepository;
            _examRepository = examRepository;
            _questionRepository = questionRepository;
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> StudentIndex()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            int studentID = _studentRepository.GetCurrentStudentId(user.Id);
            List<ResultsListVM> results = _examAnsweredQuestionRepository.ResultsForStudent(studentID);


            return View(results);
        }

        [Authorize(Roles = "Student")]
        public async Task<IActionResult> ExamFinished(int examID)
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            int studentID = _studentRepository.GetCurrentStudentId(user.Id);
            ResultsListVM results = _examAnsweredQuestionRepository.ExamResultForStudent(studentID, examID);

            if (results != null)
                return View(results);
            else 
                return RedirectToAction("StudentIndex","Results");
        }

        [Authorize(Roles = "Admin,Profesor")]
        public async Task<IActionResult> ProfessorIndex()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            List<ExamPreviewVM> exams = _examRepository.GetAllExamsByProfessor(user.Id);
            List<ExamPreviewVM> _exams = exams.Where(e => _examAnsweredQuestionRepository.ExamTakenForProfessor( e.ExamID) == true).ToList();

            return View(_exams);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult AdminIndex()
        { 
            List<ExamPreviewVM> exams = _examRepository.GetAllExams();
            return View("ProfessorIndex", exams);
        }

        [Authorize(Roles = "Admin,Profesor")]
        public IActionResult ListOfResults(int examID)
        {
            ExamResultsVM model = _examAnsweredQuestionRepository.ExamResults(examID);

            return View(model);
        }
        [Authorize(Roles = "Admin,Profesor")]
        public IActionResult ResultDetails(int examID, int studentID)
        {
            List<QuestionExamListVM> questions = _questionRepository.GetQuestionByExam(examID);
            List<QuestionDetailsVM> questionDetails = questions.Select(q => _questionRepository.GetQuestionDetails(q.QuestionID)).ToList();
            List<QuestionDisplayVM> model = new List<QuestionDisplayVM>();

            foreach (var item in questionDetails)
            {
                var question = _questionRepository.GetQuestionByNumber(item.QuestionNumber, examID);
                var savedAnswers = _examAnsweredQuestionRepository.GetAnswers(question.QuestionID, studentID);
                foreach (var answer in savedAnswers)
                {
                    question.Options.Where(x => x.ChoiceID == answer.ChoiceID).FirstOrDefault().Answer = answer.Answer;
                }
                model.Add(question);
            }

            ViewBag.QuestionDetails = questionDetails;
            ViewBag.ExamID = examID;
            ViewBag.StudentID = studentID;

            return View(model);
        }
        [Authorize(Roles = "Admin,Profesor")]
        public IActionResult AddMarkedScoreForEssay(int examID, int studentID, int answerID, int score)
        {
            var answer = _examAnsweredQuestionRepository.GetAll().Where(e => e.ChoiceID == answerID).FirstOrDefault();
            if (answer != null)
            {
            answer.MarkScored = score;
            _examAnsweredQuestionRepository.Update(answer);
                SuccessNotification = "Uspješno ste dodali bodove.";
            }
            return RedirectToAction("ResultDetails", "Results", new { examID = examID, studentID = studentID });
        }

    }
}