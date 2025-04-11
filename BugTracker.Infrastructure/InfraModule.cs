using BugTracker.Infrastructure.Context;
using BugTracker.Infrastructure.Repos.Abstract;
using BugTracker.Infrastructure.Repos.Implementation;
using BugTracker.Infrastructure.UnitOfWorks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BugTracker.Infrastructure
{
    public static class InfraModule
    {
        public static IServiceCollection AddInfraModule(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("DB"));
            });

            services.AddScoped<IAttachmentRepo, AttachmentRepo>();
            services.AddScoped<IBugRepo, BugRepo>();
            services.AddScoped<IProjectRepo, ProjectRepo>();
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));
            return services;
        }
    }
}
