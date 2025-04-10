using DVLD.Api;
using DVLD.Api.Middlewares;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;

namespace DVLD
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();


            #region myConfigs
            //Add configuration from the secret.json file
            builder.Configuration.AddJsonFile("Secret.json", optional: false, reloadOnChange: true);
            builder.Services.AddServices(builder.Configuration);

            //serilog;
            builder.Host.UseSerilog((context, config) =>
            {                              // read configs from appsettigns
                config.ReadFrom.Configuration(context.Configuration);
            });




            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseStaticFiles();   
            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseCors("AllowAll"); // Apply CORS before Authorization
            //app.UseCustomCors(); // or using cutom middleware
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            // it cause problem of more than dbContext was found
            //health check
            app.MapHealthChecks("/health", new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
            //health check with dashboard
            app.MapHealthChecksUI(opts =>
            {
                opts.UIPath = "/health-ui";
            });
            app.Run();
        }
    }
}
