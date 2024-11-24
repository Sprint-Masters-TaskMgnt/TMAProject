
using System.Threading.Tasks;
namespace TMAWebAPI.Services
{

    public interface IEmailService
    {
        Task<string> GetUserEmailByIdAsync(int Id);
        Task SendEmailAsync(string toEmail, string subject, string body, string fromEmail = null);
    }

}

