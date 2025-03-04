using DVLD.Core.Constants;
using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DVLD.Core.Services.Implementations
{
    public sealed class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<AppRole> roleManager;
        private readonly IUOW uow;
        private readonly IOptionsMonitor<JWT> JWTConfigs;
        private readonly IMailingService mailingService;

        public AuthService(UserManager<AppUser> userManager
            , RoleManager<AppRole> roleManager
            , IUOW uow
            , IOptionsMonitor<JWT> JWTConfigs
            ,IMailingService mailingService
)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.uow = uow;
            this.JWTConfigs = JWTConfigs;
            this.mailingService = mailingService;
        }

        // JWT Token
        public async Task<AuthResultDTO> RegisterAsync(UserRegisterDTO userRegisterDTO)
        {

            if (await userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                return new AuthResultDTO()
                {
                    Success = false,
                    Messages = new List<string> { "Email is already Registered!" }
                };

            //create user and create applicant and assign it countryid and userid 

            //  Create a new user
            var user = new AppUser
            {
                UserName = userRegisterDTO.Email, // Assuming username is email
                Email = userRegisterDTO.Email,
                IsActive = true // Set default active status
            };

            var result = await userManager.CreateAsync(user, userRegisterDTO.Password);
            if (!result.Succeeded)
                return new AuthResultDTO()
                {
                    Success = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };

            // add role to user
            await userManager.AddToRoleAsync(user, Roles.UserRole);

            //Create an Applicant record linked to the user
            var applicant = new Applicant
            {
                NationalNo = userRegisterDTO.NationalNo,
                Fname = userRegisterDTO.Fname,
                Sname = userRegisterDTO.Sname,
                Tname = userRegisterDTO.Tname,
                Lname = userRegisterDTO.Lname,
                Gender = userRegisterDTO.Gender,
                BirthDate = userRegisterDTO.BirthDate,
                Address = userRegisterDTO.Address,
                CountryId = userRegisterDTO.CountryId, // Assign country ID
                UserId = user.Id // Link the applicant to the user
            };

            // TODO:Save applicant using applicant Repo

            // generate token
            var token = await GenerateJwtTokenAsync(user);
            return new AuthResultDTO()
            {
                Success = true,
                ExpiresOn = token.ValidTo,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

        }
        public async Task<AuthResultDTO> LoginAsync(UserLoginDTO UserDTO)
        {
            var user = await userManager.FindByEmailAsync(UserDTO.Email);
            if (user == null)
                return new AuthResultDTO
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var result = await userManager.CheckPasswordAsync(user, UserDTO.Password);
            if (!result)
                return new AuthResultDTO
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var token = await GenerateJwtTokenAsync(user);
            return new AuthResultDTO()
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo
            };


        }
        private async Task<JwtSecurityToken> GenerateJwtTokenAsync(AppUser appUser)
        {

            var userClaims = await userManager.GetClaimsAsync(appUser);
            var roles = await userManager.GetRolesAsync(appUser);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, appUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
                new Claim("uid", appUser.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTConfigs.CurrentValue.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: JWTConfigs.CurrentValue.Issuer,
                audience: JWTConfigs.CurrentValue.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(JWTConfigs.CurrentValue.ExpireAfterInMinute),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;


        }

        // JWT with RefreshToken
        private RefreshToken GenereteRefreshToken()
        {
            var randomNum = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNum);

            return new RefreshToken
            {
                // he needs to refresh token  or login each 15 days 
                ExpiresOn = DateTime.UtcNow.AddDays(15),
                Token = Convert.ToBase64String(randomNum),
                CreatedOn = DateTime.UtcNow
            };
        }

        public async Task<AuthResultDTOForRefresh> RegisterWithRefreshTokenAsync(UserRegisterDTO userRegisterDTO)
        {

            if (await userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                return new AuthResultDTOForRefresh()
                {
                    Success = false,
                    Messages = new List<string> { "Email is already Registered!" }
                };

            // Create a new user
            var user = new AppUser
            {
                UserName = userRegisterDTO.Email, // Assuming username is email
                Email = userRegisterDTO.Email,
                IsActive = true // Set default active status
            };

            var result = await userManager.CreateAsync(user, userRegisterDTO.Password);
            if (!result.Succeeded)
                return new AuthResultDTOForRefresh()
                {
                    Success = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };

            // add role to user
            await userManager.AddToRoleAsync(user, Roles.UserRole);

            //Create an Applicant record linked to the user
            var applicant = new Applicant
            {
                NationalNo = userRegisterDTO.NationalNo,
                Fname = userRegisterDTO.Fname,
                Sname = userRegisterDTO.Sname,
                Tname = userRegisterDTO.Tname,
                Lname = userRegisterDTO.Lname,
                Gender = userRegisterDTO.Gender,
                BirthDate = userRegisterDTO.BirthDate,
                Address = userRegisterDTO.Address,
                CountryId = userRegisterDTO.CountryId, // Assign country ID
                UserId = user.Id // Link the applicant to the user
            };

            //TODO:Save applicant using applicant repo

            // generate token
            var token = await GenerateJwtTokenAsync(user);
            // generate refreshToken
            var refreshToken = GenereteRefreshToken();


            // then save it in db
            user.RefreshTokens.Add(refreshToken);
            await userManager.UpdateAsync(user);
            return new AuthResultDTOForRefresh()
            {
                Success = true,
                RefreshTokenExpiresOn = refreshToken.ExpiresOn,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken = refreshToken.Token
            };

        }

        public async Task<AuthResultDTOForRefresh> LoginWithRefreshTokenAsync(UserLoginDTO UserDTO)
        {
            // to include the RefreshTokens 
            var user = await userManager.Users
                                    .Include(u => u.RefreshTokens)
                                    .FirstOrDefaultAsync(u => u.Email == UserDTO.Email);
            if (user == null)
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var result = await userManager.CheckPasswordAsync(user, UserDTO.Password);
            if (!result)
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var token = await GenerateJwtTokenAsync(user);



            var authResult = new AuthResultDTOForRefresh()
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            // check if user has already active refresh token 
            // so no need to give him new refresh token
            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                // TODO: check this 
                var UserRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
                authResult.RefreshToken = UserRefreshToken.Token;
                authResult.RefreshTokenExpiresOn = UserRefreshToken.ExpiresOn;
            }

            // if he does not
            // generate new refreshToken
            else
            {
                var refreshToken = GenereteRefreshToken();
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiresOn = refreshToken.ExpiresOn;

                // then save it in db
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }

            return authResult;


        }

        public async Task<AuthResultDTOForRefresh> RefreshTokenAsync(string refreshToken)
        {
            // ensure there is user has this refresh token
            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
            {
                return new AuthResultDTOForRefresh
                {
                    // u can don't add false=> cuz it's the default value 
                    Success = false,
                    Messages = ["InValid Token"]
                };
            }
            // ensure this token is active
            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = ["InValid Token"]
                };
            // if all things well
            //revoke old refresh token
            oldRefreshToken.RevokedOn = DateTime.UtcNow;

            // generate new refresh token and add it to db
            var newRefreshToken = GenereteRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            await userManager.UpdateAsync(user);

            // generate new JWT Token
            var jwtToken = await GenerateJwtTokenAsync(user);

            return new AuthResultDTOForRefresh
            {
                Success = true,
                Messages = ["Refresh Token Successfully"],
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpiresOn = newRefreshToken.ExpiresOn,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtToken)
            };

        }

        public async Task<bool> RevokeTokenAsync(string refreshToken)
        {
            if (string.IsNullOrEmpty(refreshToken))
                return false;

            var user = await userManager.Users.SingleOrDefaultAsync(u => u.RefreshTokens.Any(r => r.Token == refreshToken));
            if (user == null)
                return false;

            var oldRefreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == refreshToken);
            if (!oldRefreshToken.IsActive)
                return false;


            oldRefreshToken.RevokedOn = DateTime.UtcNow;


            await userManager.UpdateAsync(user);


            return true;

        }


        // With Email Verification
        public async Task<ResultDTO<string>> RegisterWithEmailVerification(UserRegisterDTO userRegisterDTO, string scheme, string host)
        {

            if (await userManager.FindByEmailAsync(userRegisterDTO.Email) is not null)
                return new ResultDTO<string>()
                {
                    Success = false,
                    Messages = new List<string> { "Email is already Registered!" }
                };

            //  Create a new user
            var user = new AppUser
            {
                UserName = userRegisterDTO.Email, // Assuming username is email
                Email = userRegisterDTO.Email,
                IsActive = true, // Set default active status
                EmailConfirmed=false
            };

            var result = await userManager.CreateAsync(user, userRegisterDTO.Password);
            if (!result.Succeeded)
                return new ResultDTO<string>()
                {
                    Success = false,
                    Messages = result.Errors.Select(e => e.Description).ToList()
                };


            //Create an Applicant record linked to the user
            var applicant = new Applicant
            {
                NationalNo = userRegisterDTO.NationalNo,
                Fname = userRegisterDTO.Fname,
                Sname = userRegisterDTO.Sname,
                Tname = userRegisterDTO.Tname,
                Lname = userRegisterDTO.Lname,
                Gender = userRegisterDTO.Gender,
                BirthDate = userRegisterDTO.BirthDate,
                Address = userRegisterDTO.Address,
                CountryId = userRegisterDTO.CountryId, // Assign country ID
                UserId = user.Id // Link the applicant to the user
            };

            // TODO: add applicant using applicant repository

            // add role to user
            await userManager.AddToRoleAsync(user, Roles.UserRole);


            // send confirmation token to user 
            await SendConfirmationEmailAsync(user, scheme, host);

            return new ResultDTO<string>()
            {
                Success = true,
                Messages = ["Please verify your email, through the verification email we have just send"]
            };

        }

        public async Task<ResultDTO<string>> VerifyEmailAsync(string userId, string code)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(code))
            {
                return ResultDTO<string>.Failure(["UserId and code are required"]);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ResultDTO<string>.Failure(["User not found"]);
            }

            // Decode the token before using it
            var decodedCode = Uri.UnescapeDataString(code);

            var result = await userManager.ConfirmEmailAsync(user, decodedCode);
            if (result.Succeeded)
            {
                return ResultDTO<string>.SuccessFully(["Email confirmed successfully"], null);
            }
            else
            {
                return ResultDTO<string>.Failure(["Email confirmation failed"]);
            }
        }

        public async Task<AuthResultDTOForRefresh> LoginWithEmailVerificationWithRefreshTokenAsync(UserLoginDTO UserDTO)
        {
            //var user = await userManager.FindByEmailAsync(UserDTO.Email);

            // to include the RefreshTokens 
            var user = await userManager.Users
                                    .Include(u => u.RefreshTokens)
                                    .FirstOrDefaultAsync(u => u.Email == UserDTO.Email);
            if (user == null)
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            // verify if he confirmed
            if (!user.EmailConfirmed)
            {
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = new List<string> { "Email needs to be Confirmed" }
                };
            }

            var result = await userManager.CheckPasswordAsync(user, UserDTO.Password);
            if (!result)
                return new AuthResultDTOForRefresh
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var token = await GenerateJwtTokenAsync(user);



            var authResult = new AuthResultDTOForRefresh()
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
            };

            // check if user has already active refresh token 
            // so no need to give him new refresh token
            if (user.RefreshTokens.Any(r => r.IsActive))
            {
                // TODO: check this 
                var UserRefreshToken = user.RefreshTokens.FirstOrDefault(r => r.IsActive);
                authResult.RefreshToken = UserRefreshToken.Token;
                authResult.RefreshTokenExpiresOn = UserRefreshToken.ExpiresOn;
            }

            // if he does not
            // generate new refreshToken
            else
            {
                var refreshToken = GenereteRefreshToken();
                authResult.RefreshToken = refreshToken.Token;
                authResult.RefreshTokenExpiresOn = refreshToken.ExpiresOn;

                // then save it in db
                user.RefreshTokens.Add(refreshToken);
                await userManager.UpdateAsync(user);
            }

            return authResult;


        }
        private async Task SendConfirmationEmailAsync(AppUser user, string scheme, string host)
        {
            var code = await userManager.GenerateEmailConfirmationTokenAsync(user);

            // Generate the URL =>https://localhost:8080/api/Account/VerifyEmail?userId=dkl&code=ioerw
            var callbackUrl = $"{scheme}://{host}/api/Account/VerifyEmail?userId={user.Id}&code={Uri.EscapeDataString(code)}";

            var emailBody = $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>Confirm Email</a>";

            // send Email
            await mailingService.SendMailBySendGridAsync(user.Email!, "Email Confirmation", emailBody);

        }

        public async Task<AuthResultDTO> LoginWithEmailVerificationAsync(UserLoginDTO UserDTO)
        {
            var user = await userManager.FindByEmailAsync(UserDTO.Email);
            if (user == null)
                return new AuthResultDTO
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };


            // verify if he confirmed
            if (!user.EmailConfirmed)
            {
                return new AuthResultDTO
                {
                    Success = false,
                    Messages = new List<string> { "Email needs to be Confirmed" }
                };
            }

            var result = await userManager.CheckPasswordAsync(user, UserDTO.Password);
            if (!result)
                return new AuthResultDTO
                {
                    Success = false,
                    Messages = new List<string> { "Email or Password is incorrect" }
                };

            var token = await GenerateJwtTokenAsync(user);
            return new AuthResultDTO()
            {
                Success = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresOn = token.ValidTo
            };


        }


        //Forgot Password
        public async Task<ResultDTO<string>> ForgotPasswordAsync(ForgotPasswordDTO forgotPasswordDTO, string scheme, string host)
        {
            var user = await userManager.FindByEmailAsync(forgotPasswordDTO.Email);
            if (user == null)
                return new ResultDTO<string>
                {
                    Success = false,
                    Messages = new List<string> { "Email is incorrect!" }
                };

            // generete token and  send it to user
            await SendPasswordResetEmailAsync(user, scheme, host);

            return new ResultDTO<string>
            {
                Success = true,
                Messages = new List<string> { "Please go to your email and reset your password" }
            };

            // after that user click on link and go to frontend page that
            //1-capture userId, code
            //2-make form for user to reset new password
            // then user send data to reset password endpoint


        }
        private async Task SendPasswordResetEmailAsync(AppUser user, string scheme, string host)
        {
            // Generate the password reset token
            var code = await userManager.GeneratePasswordResetTokenAsync(user);

            // Construct the reset link
            var callbackUrl = $"{scheme}://{host}/api/Account/ResetPassword?userId={user.Id}&code={Uri.EscapeDataString(code)}";
            // Send email with the reset link
            await mailingService.SendMailBySendGridAsync(user.Email, "Reset Your Password",
                $"Please reset your password by clicking this link: <a href='{callbackUrl}'>Reset Password</a>");
        }

        public async Task<ResultDTO<string>> ResetPasswordAsync(ResetPasswordDTO resetPasswordDto)
        {
            if (string.IsNullOrWhiteSpace(resetPasswordDto.UserId) || string.IsNullOrWhiteSpace(resetPasswordDto.code))
            {
                return ResultDTO<string>.Failure(["UserId and code are required"]);
            }

            var user = await userManager.FindByIdAsync(resetPasswordDto.UserId);
            if (user == null)
            {
                return ResultDTO<string>.Failure(["User not found"]);
            }

            // Decode the token before using it
            var decodedCode = Uri.UnescapeDataString(resetPasswordDto.code);

            var result = await userManager.ResetPasswordAsync(user, decodedCode, resetPasswordDto.NewPassword);
            if (result.Succeeded)
            {
                return ResultDTO<string>.SuccessFully(["Password Reset successfully"], null);
            }
            else
            {
                return ResultDTO<string>.Failure(["Error resetting password."]);
            }
        }



        /*
         
         Flow Between Frontend and Backend
            User Requests a Password Reset
                Frontend:
                    User enters their email in a "Forgot Password" form.
                    Frontend sends a POST request to ForgotPasswordAsync endpoint with the email.
                Backend (ForgotPasswordAsync)
                    Checks if the email exists.
                    Generates a password reset token.
                    Sends an email with a reset link (SendPasswordResetEmailAsync).
                    Returns a success message: "Please go to your email and reset your password".
            User Clicks the Reset Link in Email
                Frontend:
                    Extracts userId and code from the URL.
                    Displays a password reset form for the user to enter a new password.
            User Submits the New Password
                Frontend:
                    Sends a POST request to ResetPasswordAsync with userId, code, and newPassword.
                Backend (ResetPasswordAsync)
                    Validates input.
                    Finds the user.
                    Decodes the reset token.
                    Resets the password using userManager.ResetPasswordAsync().
                    Returns a success message: "Password Reset successfully".
         
         */

    }

}
