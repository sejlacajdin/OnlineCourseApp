using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace OnlineCourseApp.Controllers
{
    public class NotificationsController : Controller
    {
        protected string SuccessNotification
        {
            get { return TempData["SuccessNotification"] as string; }
            set { TempData["SuccessNotification"] = value; }
        }

        protected string ErrorNotification
        {
            get { return TempData["ErrorNotification"] as string; }
            set { TempData["ErrorNotification"] = value; }
        }

    }
}