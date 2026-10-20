using Laba.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Laba.Services.Interfaces;

namespace Laba.Services.Implementations
{
    public class EmailSender : IEmailSender
    {
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _fromEmail;

        public EmailSender(
            string host,
            int port,
            string username,
            string password,
            string fromEmail)
        {
            _host = host;
            _port = port;
            _username = username;
            _password = password;
            _fromEmail = fromEmail;
        }

        public async Task SendEmailAsync(
            string email,
            string subject,
            string message)
        {
            var mail = new MimeMessage();

            mail.From.Add(
                new MailboxAddress("Web Service", _fromEmail));

            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = subject;
            mail.Body = new TextPart("plain")
            {
                Text = message
            };

            using var client = new SmtpClient();

            await client.ConnectAsync(
                _host,
                _port,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                _username,
                _password);

            await client.SendAsync(mail);

            await client.DisconnectAsync(true);
        }
    }
}
