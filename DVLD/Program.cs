using DVLD.Api;
using DVLD.Api.Middlewares;
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
            builder.Services.AddSwaggerGen();

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
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
