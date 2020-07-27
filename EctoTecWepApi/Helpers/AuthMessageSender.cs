using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using JWTNetCore.Model;

namespace JWTNetCore.Helpers
{
    public class AuthMessageSender: IEmailSender
    {
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public EmailSettings _emailSettings { get; }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            Execute(email, subject, message).Wait();
            return Task.FromResult(0);
        }

        public async Task Execute(string email, string subject, string message)
        {
            try
            {
                string toEmail = string.IsNullOrEmpty(email)
                                 ? _emailSettings.ToEmail
                                 : email;
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(_emailSettings.UsernameEmail, "Antonio Alcantar")
                };
                //mail.To.Add(new MailAddress(toEmail));
                //mail.To.Add(new MailAddress(email));
                //mail.CC.Add(new MailAddress(_emailSettings.CcEmail));

                mail.IsBodyHtml = true;
                mail.From = new MailAddress(_emailSettings.UsernameEmail, "EctoTec");
                mail.To.Add(new MailAddress(email));
                mail.Subject = subject;  // "Personal Management System - " + subject;
                mail.Body = message;



                // mail.Priority = MailPriority.High;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Port = _emailSettings.SMPT_PORT;
                    smtp.Host = _emailSettings.PrimaryDomain;
                    smtp.UseDefaultCredentials = false;
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.Credentials = new NetworkCredential(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);
                    smtp.EnableSsl = true;
                    smtp.Timeout = 500000;

                    await smtp.SendMailAsync(mail);

                }

                mail.Dispose();
            }
            catch (Exception)
            {
                //do something here
            }
        }

    }
}
