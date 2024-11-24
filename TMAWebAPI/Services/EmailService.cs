
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System;
using System.Net.Mail;
using System.Threading.Tasks;
using TMAWebAPI.Models;
using TMAWebAPI.Services;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace TMADbAPI.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer = "smtp.gmail.com"; // Replace with your SMTP server
        private readonly int _smtpPort = 587; // Replace with your SMTP port
        private readonly string _emailFrom = "samalaabhinaya2606@gmail.com"; // Replace with your sender email
        private readonly string _emailPassword = "nozq zggr yklc opre"; // Replace with your app-specific password

        private readonly TMADbContext _context;
        public EmailService(TMADbContext TMADbContext)
        {
            _context = TMADbContext;
        }
        public async Task<string> GetUserEmailByIdAsync(int Id)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == Id);
            return user?.Email ?? throw new InvalidOperationException("User email not found.");
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
                await smtp.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_emailFrom, _emailPassword);
                await smtp.SendAsync(email);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to send email: {ex.Message}");
                throw;
            }
            finally
            {
                await smtp.DisconnectAsync(true);
            }
        }
    }
}