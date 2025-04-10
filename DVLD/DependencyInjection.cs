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
using Microsoft.OpenApi.Models;
using System.Text;

namespace DVLD.Api;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration) {

        /*
                                                                           نصيحة ترتيب:
    لو هتخلي كل الـ configurations في method واحدة (زي AddServices)، يبقى:

                                             الكاش والتسجيل بتاعه.
                       تسجيل كل الـ services اللي ممكن تعتمد عليه.
                                                           EF Core.
                                                          Identity.
                                                               JWT.
                               إعدادات خارجية زي SendGrid, SMTP, إل
         */

        // Add CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });

        // health check
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);


        // it cause problem of more than dbContext was found
        ////health check with Dashboard
        //services.AddHealthChecksUI(setup =>
        //{
        //    setup.AddHealthCheckEndpoint("DVLD", "/health");
        //}).AddInMemoryStorage();

        // in-memory caching
        services.AddScoped<ICachingService, CachingService>();
        services.AddMemoryCache(opts =>
        {
            opts.SizeLimit = 1024;
        });

        //validators
        services.AddScoped<IValidator<UserRegisterDTO>, UserRegisterDTOValidator>();
        services.AddScoped<IValidator<ApplicantDTO>, ApplicantDTOValidator>();
        services.AddScoped<IValidator<UpdateApplicationDTO>, UpdateApplicationDTOValidator>();
        services.AddScoped<IValidator<DetainedLicenseDTO>, DetainedLicenseDTOValidator>();
        services.AddScoped<IValidator<AddLicenseDTO>, AddLicenseDTOValidator>();
        services.AddScoped<IValidator<AddInternationalLicenseDTO>, AddInternationalLicenseDTOValidator>();
        services.AddScoped<IValidator<RenewLicenseApplicationDTO>, RenewLicenseApplicationDTOValidator>();

        // u can do DepenencyInjection Class for Dal and one for Core layer :()
        services.AddScoped<IUOW, UOW>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IRolesService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IApplicantService, ApplicantService>();
        services.AddScoped<IApplicationService, ApplicationService>();
        services.AddScoped<IDriverServices, DriverService>();
        services.AddScoped<ILicenseService, LicenseService>();
        services.AddScoped<ITestService, TestService>();
        services.AddScoped<IFileService, FileService>();
        services.AddScoped<IMailingService, MainlingService>();



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


        // swagger configs to add JWT
        // click Authorize button
        // then put in value => Beared {token}
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

            // Add JWT Authentication support to Swagger
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
            });
        });


        return services;


    }
}
