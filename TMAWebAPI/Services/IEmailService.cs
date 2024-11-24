 using System.Threading.Tasks;

    namespace TMAWebAPI.Services
    {
        public interface IEmailService
        {
            /// <summary>
            /// Fetches the email address of a user by their user ID.
            /// </summary>
            /// <param name="userId">The ID of the user.</param>
            /// <returns>The user's email address.</returns>
            Task<string> GetUserEmailByIdAsync(int Id);

            /// <summary>
            /// Sends an email with the specified details.
            /// </summary>
            /// <param name="toEmail">The recipient's email address.</param>
            /// <param name="subject">The subject of the email.</param>
            /// <param name="body">The body content of the email.</param>
            /// <param name="fromEmail">Optional: The sender's email address. Defaults to a configured email if not provided.</param>
            Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null);
        }
    }



