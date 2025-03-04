using Microsoft.AspNetCore.Identity;

namespace DVLD.Core.Models
{
    public class AppUser:IdentityUser
    {
        public bool IsActive {  get; set; }
        public Applicant? Applicant { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
