﻿using Microsoft.AspNetCore.Identity.UI.Services;

namespace BlogProject.Services.Interfaces
{
    public class IEmailService : IEmailSender

    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            throw new NotImplementedException();
        }
    }
}
