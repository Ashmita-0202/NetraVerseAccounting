using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TN.Shared.Domain.IRepository;

namespace TN.Shared.Infrastructure.Repository
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;

        }
        public Task SendEmailAsync(string email, string subject, string body)
        {
            var apiKey = _configuration["SendGridApiKey:EmailApi"];
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("officeproj111@gmail.com", "HamroCommunity Application");
            var to = new EmailAddress(email);
            var plainTextContent = body;
            var htmlContent = $"<p>{body}</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            return client.SendEmailAsync(msg);
        }
    }
}
