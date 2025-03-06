using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController:ControllerBase
    {
        private readonly IAuthService authService;

        public AccountController(IAuthService authService)
        {
            this.authService = authService;
        }


        // JWT
        [HttpPost("Register")]
        public async Task<ActionResult<AuthResultDTO>> RegisterAsync(UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.RegisterAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);

        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthResultDTO>> LoginAsync(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.LoginAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);

        }


        //JWT with RefreshToken

        [HttpPost("RegisterWithRefreshToken")]
        public async Task<ActionResult<AuthResultDTO>> RegisterWithRefreshTokenAsync(UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.RegisterWithRefreshTokenAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);

            //set refresh token in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);
            return Ok(result);

        }

        [HttpPost("LoginWithRefreshToken")]
        public async Task<ActionResult<AuthResultDTO>> LoginWithRefreshTokenAsync(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.LoginWithRefreshTokenAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);

            //set refresh token in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);
            return Ok(result);

        }

        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefershTokenAsync()
        {
            var refreshToken = HttpContext.Request.Cookies[GeneralConsts.RefreshTokenKey];

            var result = await authService.RefreshTokenAsync(refreshToken);
            if (!result.Success)
                return BadRequest(result);

            // set new refreshToken in response cookie
            setRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiresOn);

            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        public async Task<IActionResult> RevokeTokenAsync([FromBody] string? refreshToken)
        {
            string token = refreshToken ?? HttpContext.Request.Cookies[GeneralConsts.RefreshTokenKey];
            var result = await authService.RevokeTokenAsync(token);
            if (result)
                return Ok();
            return BadRequest("InValid Token");
        }
        private void setRefreshTokenInCookie(string refreshToken, DateTime refreshTokenExpiresOn)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = refreshTokenExpiresOn.ToLocalTime()
            };

            HttpContext.Response.Cookies.Append(GeneralConsts.RefreshTokenKey, refreshToken, cookieOptions);
        }


        //With Email Verification
        [HttpPost("RegisterWithEmailConfirmation")]
        public async Task<IActionResult> RegisterWithEmailConfirmationAsync(UserRegisterDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }

            var scheme = Request.Scheme; // e.g., "https"
            var host = Request.Host.Value; // e.g., "localhost:5000"
            var result = await authService.RegisterWithEmailVerification(userDTO, scheme, host);

            if (!result.Success)
                return BadRequest(result);


            return Ok(result.Messages);
        }

        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail(string userId, string code)
        {
            var result = await authService.VerifyEmailAsync(userId, code);
            if (result.Success)
                return Ok(result);
            return BadRequest(result.Messages);
        }

        [HttpPost("LoginWithEmailConfirmation")]
        public async Task<IActionResult> LoginWithEmailConfirmationAsync(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.LoginWithEmailVerificationAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);

        }




        [HttpPost("LoginWithEmailConfirmationWithRefreshToken")]
        public async Task<IActionResult> LoginWithEmailConfirmationWithRefreshTokenAsync(UserLoginDTO userDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }
            var result = await authService.LoginWithEmailVerificationWithRefreshTokenAsync(userDTO);
            if (!result.Success)
                return BadRequest(result);
            return Ok(result);

        }

        [HttpPost("ForgotPassword")]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }

            var scheme = Request.Scheme; // e.g., "https"
            var host = Request.Host.Value; // e.g., "localhost:5000"
            var result = await authService.ForgotPasswordAsync(forgotPasswordDTO, scheme, host);

            if (!result.Success)
                return BadRequest(result);


            return Ok(result.Messages);

            // after that user click on link and go to frontend page that
            //1-capture userId, code
            //2-make form for user to reset new password
            // then user send data to reset password endpoint
        }

        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassordAsync([FromBody] ResetPasswordDTO resetPasswordDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthResultDTO
                {
                    Success = false,
                    Messages = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList()
                });
            }

            var result = await authService.ResetPasswordAsync(resetPasswordDto);
            if (result.Success)
                return Ok(result);
            return BadRequest(result);
        }

    }
}
