using DVLD.Core.DTOs;

namespace DVLD.Core.Services.Interfaces;

public interface IAuthService
{
    // JWT
    Task<AuthResultDTO> RegisterAsync(UserRegisterDTO userRegisterDTO);
    Task<AuthResultDTO> LoginAsync(UserLoginDTO UserDTO);


    //JWT RefreshToken
    Task<AuthResultDTOForRefresh> RegisterWithRefreshTokenAsync(UserRegisterDTO userRegisterDTO);
    Task<AuthResultDTOForRefresh> LoginWithRefreshTokenAsync(UserLoginDTO UserDTO);
    Task<AuthResultDTOForRefresh> RefreshTokenAsync(string refreshToken);
    Task<bool> RevokeTokenAsync(string refreshToken);


    //With Email Verification
    Task<ResultDTO<string>> RegisterWithEmailVerification(UserRegisterDTO userRegisterDTO, string scheme, string host);
    Task<ResultDTO<string>> VerifyEmailAsync(string userId, string code);
    Task<AuthResultDTO> LoginWithEmailVerificationAsync(UserLoginDTO UserDTO);
    Task<AuthResultDTOForRefresh> LoginWithEmailVerificationWithRefreshTokenAsync(UserLoginDTO UserDTO);


    // Forgot Password
    Task<ResultDTO<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO, string scheme, string host);
    Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto);


}
