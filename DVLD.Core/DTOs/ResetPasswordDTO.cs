namespace DVLD.Core.DTOs
{
    public class ResetPasswordDTO
    {
        public string UserId { get; set; }
        public string code { get; set; }
        public string NewPassword { get; set; }
    }

}
