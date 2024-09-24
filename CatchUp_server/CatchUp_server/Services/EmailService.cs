using System.Net;
using System.Net.Mail;

namespace CatchUp_server.Services
{
    public class EmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService(string smtpServer, int port, string userName, string password)
        {
            _smtpClient = new SmtpClient(smtpServer)
            {
                Port = port,
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = true,
            };
        }

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("latixdani@gmail.com", "kxwi agbw jrdc jquc"),
                EnableSsl = true,
            };
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("latixdani@gmail.com"),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }

        public async Task SendEmailAsync(string fromEmail, string toEmail, string subject, string body)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromEmail),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(toEmail);

            await _smtpClient.SendMailAsync(mailMessage);
        }
    }
}
