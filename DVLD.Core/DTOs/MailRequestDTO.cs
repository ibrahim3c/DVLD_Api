﻿using Microsoft.AspNetCore.Http;

namespace DVLD.Core.DTOs
{
    public class MailRequestDTO
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public List<IFormFile>? Attachments { get; set; } = default;
    }
}
