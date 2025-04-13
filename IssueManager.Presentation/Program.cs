using IssueManager.Persistance;
using IssueManager.Presentation.Extentions;
using IssueManager.Presentation.Middlewares;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IssueManager.Presentation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                Log.Information("Starting up the app");
                var builder = WebApplication.CreateBuilder(args);

                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(8080);
                });


                builder.Host.UseSerilog((context, configuration) =>
                    configuration.ReadFrom.Configuration(context.Configuration));

                builder.Services.AddApplicationServices(builder.Configuration);
                builder.Services.AddSecurityApplicationServices(builder.Configuration);
                builder.Services.AddLoggingApplicationServices();

                var app = builder.Build();
                using (var scope = app.Services.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    dbContext.Database.Migrate();
                }
                app.UseMiddleware<LoggingMiddleware>();

                if (app.Environment.IsDevelopment())
                {
                    app.MapOpenApi();
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();
                app.UseAuthentication();
                app.UseAuthorization();
                app.MapControllers();
                app.Run();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "App terminated unexpectedly");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}
