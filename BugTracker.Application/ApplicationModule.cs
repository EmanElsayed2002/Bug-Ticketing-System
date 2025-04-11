using BugTracker.Application.ExceptionHandlers;
using BugTracker.Application.Services.Abstract.AttachFile;
using BugTracker.Application.Services.Abstract.Auth;
using BugTracker.Application.Services.Abstract.Bugs;
using BugTracker.Application.Services.Abstract.Projects;
using BugTracker.Application.Services.Implementation.Attachments;
using BugTracker.Application.Services.Implementation.Auth;
using BugTracker.Application.Services.Implementation.Bug;
using BugTracker.Application.Services.Implementation.Project;
using FluentValidation;
using FluentValidation.AspNetCore;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Threading.RateLimiting;
namespace BugTracker.Application
{
    public static class ApplicationModule
    {
        public static IServiceCollection AddApplicationModule(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddHangfire(x =>
            x.UseSqlServerStorage(configuration.GetConnectionString("DB")));
            service.AddHangfireServer();
            service.AddScoped<IJwtProvider, JwtProvider>();
            service.AddTransient<IAccountService, AccountService>();
            service.AddTransient<IAuthService, AuthService>();
            service.AddTransient<IRoleService, RoleService>();
            service.AddTransient<IEmailSender, EmailSender>();
            service.AddTransient<IProjectService, ProjectService>();
            service.AddTransient<IUserService, UserService>();
            service.AddTransient<IFileService, AtachmentService>();
            service.AddTransient<IBugService, BugService>();
            service.AddAutoMapper(Assembly.GetExecutingAssembly());
            service.AddExceptionHandler<GlobalExceptionHandler>();
            service.AddFluentValidationAutoValidation()
                  .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            service.AddRateLimiter(RateLimiterOption =>
            {
                RateLimiterOption.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

                RateLimiterOption.AddTokenBucketLimiter("Token", option =>
                {
                    option.TokenLimit = 2;
                    option.QueueLimit = 1;
                    option.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    option.TokensPerPeriod = 2;
                    option.ReplenishmentPeriod = TimeSpan.FromSeconds(30);
                    option.AutoReplenishment = true;
                }
                );
            });


            service.AddCors();
            return service;
        }
    }
}
