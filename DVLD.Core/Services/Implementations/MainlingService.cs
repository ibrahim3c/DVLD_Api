using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace DVLD.Core.Services.Implementations
{
    public class MainlingService : IMailingService
    {
        private readonly IOptionsMonitor<SendGridSettings> sendGridSettings;

        public MainlingService(IOptionsMonitor<SendGridSettings> sendGridSettings)
        {
            this.sendGridSettings = sendGridSettings;
        }
        public async Task SendMailBySendGridAsync(MailRequestDTO mailRequest)
        {
            var client = new SendGridClient(sendGridSettings.CurrentValue.ApiKey);
            var from = new EmailAddress(sendGridSettings.CurrentValue.SenderMail, sendGridSettings.CurrentValue.SenderName);
            var to = new EmailAddress(mailRequest.ToEmail);

            var msg = MailHelper.CreateSingleEmail(from, to, mailRequest.Subject, mailRequest.Body, mailRequest.Body);

            // Attach files if any
            if (mailRequest.Attachments != null && mailRequest.Attachments.Count > 0)
            {
                foreach (var attachment in mailRequest.Attachments)
                {
                    using (var ms = new MemoryStream())
                    {
                        await attachment.CopyToAsync(ms);
                        var fileBytes = ms.ToArray();
                        msg.AddAttachment(attachment.FileName, Convert.ToBase64String(fileBytes));
                    }
                }
            }

            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to send email: {response.StatusCode}");
            }

        }

        public async Task SendMailBySendGridAsync(string mailTo, string subject, string body, IList<IFormFile> files = null)
        {
            var client = new SendGridClient(sendGridSettings.CurrentValue.ApiKey);
            var from = new EmailAddress(sendGridSettings.CurrentValue.SenderMail, sendGridSettings.CurrentValue.SenderName);
            var to = new EmailAddress(mailTo);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);

            // Attach files if any
            if (files != null && files.Count > 0)
            {
                foreach (var file in files)
                {
                    using var ms = new MemoryStream();
                    await file.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    msg.AddAttachment(file.FileName, Convert.ToBase64String(fileBytes));
                }
            }

            // Send email and handle response
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var errorMessage = await response.Body.ReadAsStringAsync();
                throw new Exception($"Failed to send email: {response.StatusCode} - {errorMessage}");
            }

        }
    }
}
