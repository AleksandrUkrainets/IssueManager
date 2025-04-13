using IssueManager.Application.Configuration;
using IssueManager.Application.Interfaces;
using IssueManager.Application.Services;
using IssueManager.Domain.Interfaces;
using IssueManager.Infrastructure.Clients;
using IssueManager.Infrastructure.Factories;
using IssueManager.Infrastructure.Handlers;
using IssueManager.Infrastructure.Providers;
using IssueManager.Infrastructure.Services;
using IssueManager.Persistance;
using IssueManager.Persistance.Repository;
using IssueManager.Persistance.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Refit;
using System.Text.Json;

namespace IssueManager.Presentation.Extentions
{
    public static class ApplicationServicesExtentions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnectionString")));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddScoped<IEncryptionService, AesEncryptionService>();
            services.AddScoped<IUserCredentialRepository, UserCredentialRepository>();
            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIssueProviderFactory, IssueProviderFactory>();
            services.AddScoped<IIssueService, IssueService>();
            services.AddScoped<ITokenGenerator, TokenGenerator>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<GitHubOAuthProvider>();
            services.AddScoped<GitLabOAuthProvider>();
            services.AddScoped<IOAuthProviderFactory, OAuthProviderFactory>();
            services.AddScoped<RefitLoggingHandler>();

            services.AddOpenApi();

            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower;
                options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower;
            });
            services.AddSingleton(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
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


            var serializerOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower,
                PropertyNameCaseInsensitive = true
            };
            var refitSettings = new RefitSettings
            {
                ContentSerializer = new SystemTextJsonContentSerializer(serializerOptions)
            };


            services.AddRefitClient<IGitHubOAuthClient>(refitSettings)
                .ConfigureHttpClient((sp, c) =>
                {
                    var settings = sp.GetRequiredService<IOptions<OAuthSettings>>().Value;
                    var githubConfig = settings.Providers["github"];
                    c.BaseAddress = new Uri(githubConfig.BaseUrl);
                    c.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");
                })
                .AddHttpMessageHandler<RefitLoggingHandler>();

            services.AddRefitClient<IGitHubApiClient>(refitSettings)
                .ConfigureHttpClient((sp, c) =>
                {
                    var settings = sp.GetRequiredService<IOptions<OAuthSettings>>().Value;
                    var githubConfig = settings.Providers["github"];
                    c.BaseAddress = new Uri(githubConfig.ApiBaseUrl);
                    c.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");
                })
                .AddHttpMessageHandler<RefitLoggingHandler>();

            services.AddRefitClient<IGitLabApiClient>(refitSettings)
                .ConfigureHttpClient((sp, c) =>
                {
                    var settings = sp.GetRequiredService<IOptions<OAuthSettings>>().Value;
                    var gitlabConfig = settings.Providers["gitlab"];
                    c.BaseAddress = new Uri(gitlabConfig.ApiBaseUrl);
                    c.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");
                })
                .AddHttpMessageHandler<RefitLoggingHandler>();

            services.AddRefitClient<IGitLabOAuthClient>(refitSettings)
                .ConfigureHttpClient((sp, c) =>
                {
                    var settings = sp.GetRequiredService<IOptions<OAuthSettings>>().Value;
                    var gitlabConfig = settings.Providers["gitlab"];
                    c.BaseAddress = new Uri(gitlabConfig.BaseUrl);
                    c.DefaultRequestHeaders.UserAgent.ParseAdd("IssueManagerApp");
                })
                .AddHttpMessageHandler<RefitLoggingHandler>();

            return services;
        }
    }
}
