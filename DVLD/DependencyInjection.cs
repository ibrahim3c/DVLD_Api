using DVLD.Core.DTOs;
using DVLD.Core.Helpers;
using DVLD.Core.IRepositories;
using DVLD.Core.Models;
using DVLD.Core.Services.Implementations;
using DVLD.Core.Services.Interfaces;
using DVLD.Core.Validators;
using DVLD.Dal.Data;
using DVLD.Dal.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DVLD.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) {


        // u can do DepenencyInjection Class for Dal and one for Core layer :()
        services.AddScoped<IUOW, UOW>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IMailingService, MainlingService>();
        services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();


        //sendGrid
        services.Configure<SendGridSettings>(configuration.GetSection("SendGridSettings"));



        #region EfCoreConfigs
        var connectionString = configuration.GetConnectionString("DefaultConnection")
            ?? throw new ArgumentNullException(nameof(configuration));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(connectionString);
        });
        #endregion

        #region JWTConfigs
        //(1)
        // identity ===> i spend one day to find out that you are the problem => it should be above JWTConfigs :(
        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();


        // JWTHelper (2)
        services.Configure<JWT>(configuration.GetSection("JWT"));

        // (3)
        // to use jwt token to check authantication =>[authorize]
        services.AddAuthentication(options =>
        {
            // to change default authantication to jwt 
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;

            //  if u are unauthanticated it will redirect you to login form
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            // if there other schemas make is default of jwt
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;


            // these configs to check if has token only but i want to check if he has right claims
        }).AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;

            // check if token have specific data
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidIssuer = configuration["JWT:Issuer"],
                ValidAudience = configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:SecretKey"])),

                // if u want when the token expire he does not give me مهله بعض الوقت 
                ClockSkew = TimeSpan.Zero

            };
        }

                     );
        #endregion
        return services;


    }
}
