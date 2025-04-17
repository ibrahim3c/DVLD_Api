using DVLD.Api;
using DVLD.Api.Middlewares;
using DVLD.Core.Helpers;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Serilog;
using System.Threading.RateLimiting;

namespace DVLD
{
    public class Program
    {
        public static async Task Main(string[] args)
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

            //RateLimiter
            builder.Services.AddRateLimiter(options =>
            {
                options.AddPolicy("Token", context =>
        RateLimitPartition.GetTokenBucketLimiter(
        partitionKey: context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: _ => new TokenBucketRateLimiterOptions
            {
                TokenLimit = 10, 
                TokensPerPeriod = 1,
                                    
                ReplenishmentPeriod = TimeSpan.FromSeconds(1), 
                QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                QueueLimit = 0
            }));
                options.RejectionStatusCode = 429;
            });





            #endregion

            var app = builder.Build();

            ////seed admin user
            //using (var scope = app.Services.CreateScope())
            //{
            //    var services = scope.ServiceProvider;
            //    await Initializer.SeedDataAsync(services); // make sure this method is async
            //}

            // Configure the HTTP request pipeline.
            // if (app.Environment.IsDevelopment())
            // {
            app.UseSwagger();
            app.UseSwaggerUI();
            //}

            app.UseStaticFiles();   
            app.UseHttpsRedirection();
            app.UseMiddleware<GlobalExceptionHandler>();
            app.UseCors("AllowAll"); // Apply CORS before Authorization
            //app.UseCustomCors(); // or using cutom middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRateLimiter();

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
