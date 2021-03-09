using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineCourseApp.SignalR.Hubs
{
    public class NotificationHub: Hub
    {
        public override async Task OnConnectedAsync()
        {
            // 1. Add the use to the public group
            await Groups.AddToGroupAsync(Context.ConnectionId, "PublicGroup");

            // 2. Add user to the private channel, single person
            if (Context.User.Identity.Name != null)
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.User.Identity.Name);

            if (Context.User.IsInRole("Admin"))
            {
                // 3. Add the user to the Admin group
                await Groups.AddToGroupAsync(Context.ConnectionId, "Admin");
            }

            if (Context.User.IsInRole("Profesor"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Profesor");
            }

            if (Context.User.IsInRole("Student"))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, "Student");
            }
            await base.OnConnectedAsync();
        }
    }
}
