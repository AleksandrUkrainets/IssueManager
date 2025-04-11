using IssueManager.Application.Interfaces;
using IssueManager.Application.Services;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Factories;
using IssueManager.Infrastructure.Services;
using IssueManager.Persistance.Repository;
using IssueManager.Persistance.Security;
using IssueManager.Persistance;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace IssueManager.Presentation.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString")));

            services.AddScoped<IEncryptionService, AesEncryptionService>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIssueProviderFactory, IssueProviderFactory>();
            services.AddScoped<IIssueService, IssueService>();

            services.AddOpenApi();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            });

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Issue Manager API", Version = "v1" });

                opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Write JWT token from /oauth/callback"
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
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
}
