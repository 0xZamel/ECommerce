using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace ECommerce.Utility
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        public EmailSender(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string email,string subject,string message)
        {
            var smtpClient = new SmtpClient( _configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(
                    _configuration["EmailSettings:Port"]!),

                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:Username"],
                    _configuration["EmailSettings:Password"]),

                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(
                    _configuration["EmailSettings:Username"]!),

                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
