using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Configuration;

namespace APP.Bus.Repository.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;

        public EmailSender(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var mailjetApiKey = "208dd27fca54f61709d9472bcac83c4c";
            var mailjetApiSecret = "1341aee10ba5ae102a7fbfa3dba9caf9";
            var mailjetClient = new MailjetClient(mailjetApiKey, mailjetApiSecret);

            var request = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("cakhosolo2003@gmail.com", "HyperS"))
                .WithTo(new SendContact(email))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .Build();

            var response = await mailjetClient.SendTransactionalEmailAsync(request);

            Console.WriteLine(response);
        }
    }
}
