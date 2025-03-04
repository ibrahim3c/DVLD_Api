using DVLD.Core.DTOs;
using Microsoft.AspNetCore.Http;

namespace DVLD.Core.Services.Interfaces
{
    public interface IMailingService
    {
        Task SendMailBySendGridAsync(MailRequestDTO mailRequest);
        Task SendMailBySendGridAsync(string mailTo, string subject, string body, IList<IFormFile> files = null);

    }
}
