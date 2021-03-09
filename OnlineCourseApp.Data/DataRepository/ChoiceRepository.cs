using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCourseApp.Data.DataRepository.OnlineCourseApp.Data.DataRepository;
using OnlineCourseApp.Data.EF;
using OnlineCourseApp.Data.Models;
using OnlineCourseApp.Data.Models.Account;
using OnlineCourseApp.Data.RepositoryInterfaces;
using OnlineCourseApp.Data.ViewModels;
using OnlineCourseApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace OnlineCourseApp.Data.DataRepository
{
    public class ChoiceRepository : BaseRepository<Choice>, IChoiceRepository
    {
        public ChoiceRepository(MyDBContext db) : base(db) { }

        public ChoiceEditVM EditChoice(int choiceID, int examID)
        {
            Choice choice = db.Choice.Find(choiceID);

            if (choice == null)
                return null;

            else return new ChoiceEditVM
            {
                ChoiceID = choice.ID,
                QuestionID = choice.QuestionID,
                ExamID = examID,
                Points = choice.Points,
                Text = choice.Text,
                IsCorrect = choice.IsCorrect,
                correctList = new List<SelectListItem>()

            };
        }
    }
}

