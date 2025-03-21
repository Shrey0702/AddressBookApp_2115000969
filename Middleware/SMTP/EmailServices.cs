using Microsoft.Extensions.Configuration;
using System;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using MimeKit;
using MailKit.Net.Smtp;
using SmtpClient = System.Net.Mail.SmtpClient;

namespace Middleware.SMTP
{
    public class EmailServices : IEmailServices
    {
        private readonly IConfiguration _configuration;

        public EmailServices(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendResetPasswordEmailAsync(string email, string token)
        {
            var fromEmail = _configuration["SMTP:SenderEmail"];
            var fromPassword = _configuration["SMTP:Password"];
            var smtpHost = _configuration["SMTP:Host"];
            var smtpPort = _configuration["SMTP:Port"];
            var enableSSL = bool.TryParse(_configuration["SMTP:EnableSSL"], out bool sslEnabled) && sslEnabled;
            var resetPasswordUrl = _configuration["App:ResetPasswordUrl"];

            if (string.IsNullOrWhiteSpace(fromEmail) || string.IsNullOrWhiteSpace(fromPassword))
            {
                throw new Exception("SMTP sender email or password is missing in configuration.");
            }

            try
            {
                var fromAddress = new MailAddress(fromEmail, "AddressBOOK API");
                var toAddress = new MailAddress(email);
                string resetLink = $"{resetPasswordUrl}?token={token}";

                using var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = int.TryParse(smtpPort, out int port) ? port : 587, // Default to 587
                    EnableSsl = enableSSL,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                using var message = new MailMessage(fromAddress, toAddress)
                {
                    Subject = "Reset Your Password",
                    Body = $"Click the link to reset your password: <a href='{resetLink}'>{resetLink}</a>",
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                throw new Exception("Email sending failed", ex);
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var fromEmail = _configuration["SMTP:SenderEmail"];
            var fromPassword = _configuration["SMTP:Password"];
            var smtpHost = _configuration["SMTP:Host"];
            var smtpPort = _configuration["SMTP:Port"];

            try
            {
                var emailMessage = new MimeMessage();
                emailMessage.From.Add(new MailboxAddress("AddressBook", fromEmail));
                emailMessage.To.Add(new MailboxAddress("Recipient", to));
                emailMessage.Subject = subject;
                emailMessage.Body = new TextPart("html") { Text = body };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(smtpHost, int.TryParse(smtpPort, out int port) ? port : 587, MailKit.Security.SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(fromEmail, fromPassword);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);

                Console.WriteLine($"[x] Email successfully sent to {to}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[!] Email sending failed: {ex.Message}");
                throw;
            }
        }
    }
}
