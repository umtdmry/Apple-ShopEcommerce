using Microsoft.AspNetCore.Identity.UI.Services;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ETicaret.WebUI.EmailServices
{
    //EmailSender IEmailSender interfacesini kalıtmaktadır.
    //Bu method email servicemiz olan SendGrid'in dökümantasyonundan alınmıştır.
    //apiKey ile onay maili ve şifremi unuttum maili aşağıdaki fonksiyonlar sayesinde gönderilmektedir.
    public class EmailSender:IEmailSender
    {

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var apiKey = "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";

            return Execute(apiKey, subject, htmlMessage, email);
        }

        public Task Execute(string apiKey, string subject, string htmlMessage, string email)
        {
            var client = new SendGridClient(apiKey);

            var msg = new SendGridMessage()
            {
                From = new EmailAddress("croxyfire@gmail.com", "E Ticaret Project"),
                Subject = subject,
                PlainTextContent = htmlMessage,
                HtmlContent = htmlMessage
            };

            msg.AddTo(new EmailAddress(email));
            return client.SendEmailAsync(msg);
        }
    }
}
