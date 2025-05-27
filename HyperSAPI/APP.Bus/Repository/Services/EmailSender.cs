using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Mailjet.Client;
using Mailjet.Client.Resources;
using Mailjet.Client.TransactionalEmails;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace APP.Bus.Repository.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private MailjetClient _mailjetClient;
        public EmailSender(IConfiguration configuration)
        {
            var mailjetApiKey = "208dd27fca54f61709d9472bcac83c4c";
            var mailjetApiSecret = "1341aee10ba5ae102a7fbfa3dba9caf9";
            _mailjetClient = new MailjetClient(mailjetApiKey, mailjetApiSecret);
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {           
            var request = new TransactionalEmailBuilder()
                .WithFrom(new SendContact("cakhosolo2003@gmail.com", "HyperS"))
                .WithTo(new SendContact(email))
                .WithSubject(subject)
                .WithHtmlPart(htmlMessage)
                .Build();

            var response = await _mailjetClient.SendTransactionalEmailAsync(request);
        }
    }

    public class EmailService
    {
        private SmtpClient mailClient;

        private string sender = null;

        public EmailService(string server, int port, string user, string password, string emailsender)
        {
            mailClient = new SmtpClient(server, port);
            mailClient.Timeout = 15000;
            mailClient.Credentials = new NetworkCredential(user, password);
            mailClient.EnableSsl = true;
            sender = emailsender;
        }

        public string ReadTemplate()
        {
            throw new NotImplementedException();
        }

        public void SendEmail(string subject, string body, List<string> to, List<string> Cc = null, List<string> Bcc = null)
        {
            try
            {
                if (mailClient != null && sender != null)
                {
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(sender);
                    mailMessage.Subject = subject;
                    mailMessage.Body = body;
                    mailMessage.IsBodyHtml = true;
                    mailMessage.To.Add(string.Join(",", to));
                    if (Cc != null)
                    {
                        mailMessage.CC.Add(string.Join(",", Cc));
                    }

                    if (Bcc != null)
                    {
                        mailMessage.Bcc.Add(string.Join(",", Bcc));
                    }

                    mailClient.Send(mailMessage);
                    return;
                }

                throw new Exception("Not config");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
