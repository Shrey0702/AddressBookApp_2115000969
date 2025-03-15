using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Middleware.SMTP
{
    public class EmailServices: IEmailServices
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
            var enableSSL = Convert.ToBoolean(_configuration["SMTP:EnableSSL"]);
            string resetPasswordUrl = _configuration["App:ResetPasswordUrl"]; // Get the URL from appsettings.json

            

            if (string.IsNullOrWhiteSpace(fromEmail))
            {
                throw new Exception("SMTP sender email is missing in configuration.");
            }

            try
            {
                var fromAddress = new MailAddress(fromEmail, "AddressBOOK API");
                var toAddress = new MailAddress(email);

                using var smtp = new SmtpClient
                {
                    Host = smtpHost,
                    Port = int.Parse(smtpPort ?? "587"), // Default to 587 if null
                    EnableSsl = enableSSL,
                    Credentials = new NetworkCredential(fromEmail, fromPassword)
                };

                string resetLink = $"{resetPasswordUrl}?token={token}";// Use the configured reset link

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
    }
}
