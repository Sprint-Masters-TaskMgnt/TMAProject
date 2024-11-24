using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using TMAWebAPI.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace TMAWebAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Replace with your SMTP server
        private readonly int _smtpPort = 587; // Replace with your SMTP port
        private readonly string _emailFrom = "samalaabhinaya2606@gmail.com"; // Replace with your sender email
        private readonly string _emailPassword = "nozq zggr yklc opre"; // Replace with your app-specific password

        private readonly TMADbContext _context;
        private readonly ILogger<EmailService> _logger;

        public EmailService(TMADbContext tMADbContext, IConfiguration configuration, ILogger<EmailService> logger)
        {
            _context = tMADbContext;
            _logger = logger;

            //_smtpServer = configuration["Smtp:Server"] ?? throw new ArgumentNullException("SMTP server is not configured.");
            //_smtpPort = int.Parse(configuration["Smtp:Port"] ?? "587");
            //_emailFrom = configuration["Smtp:EmailFrom"] ?? throw new ArgumentNullException("SMTP sender email is not configured.");
            //_emailPassword = configuration["Smtp:Password"] ?? throw new ArgumentNullException("SMTP password is not configured.");
        }

        public async Task<string> GetUserEmailByIdAsync(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user == null)
            {
                _logger.LogError($"User with ID {Id} not found.");
                throw new InvalidOperationException($"User with ID {Id} does not exist.");
            }
            return user.Email;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Task Management System", fromEmail ?? _emailFrom));
            email.To.Add(new MailboxAddress("Recipient", toEmail));
            email.Subject = subject;
            email.Body = new TextPart("plain") { Text = body };

            using var smtp = new SmtpClient();
            try
            {
                _logger.LogInformation("Connecting to SMTP server...");
                await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailFrom, _emailPassword);
                await smtp.SendAsync(email);
                _logger.LogInformation("Email sent successfully to {Recipient}.", toEmail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {Recipient}.", toEmail);
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
                _logger.LogInformation("Disconnected from SMTP server.");
            }
        }
    }
}
