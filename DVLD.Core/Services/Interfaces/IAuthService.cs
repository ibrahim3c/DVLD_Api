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

}
