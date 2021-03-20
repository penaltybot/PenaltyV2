using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace PenaltyV2.Areas.Identity.Pages.Account
{
    public class sendMail : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var fromAddress = Environment.GetEnvironmentVariable("FROM_EMAIL");
            string emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress, emailPassword),
            };

            var mailMessage = new MailMessage(fromAddress, email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };

            smtp.Send(mailMessage);
            return Task.CompletedTask;
        }
    }
}
