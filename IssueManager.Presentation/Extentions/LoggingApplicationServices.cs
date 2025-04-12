using Serilog;

namespace IssueManager.Presentation.Extentions
{
    public static class LoggingApplicationServices
    {
        public static IServiceCollection AddLoggingApplicationServices(this IServiceCollection services)
        {
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog();
            });

            return services;
        }
    }
}
