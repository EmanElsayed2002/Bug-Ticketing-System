
using BugTracker.Application;
using BugTracker.Application.Settings;
using BugTracker.Data.models.Identity;
using BugTracker.Infrastructure;
using BugTracker.Infrastructure.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using School.Application.SeedRoles;
using System.Text;

namespace BugTracker.API
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
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplicationModule(builder.Configuration).AddInfraModule(builder.Configuration);
            builder.Services.AddIdentity<User, Role>()
               .AddEntityFrameworkStores<AppDbContext>()
               .AddDefaultTokenProviders();
            builder.Services.Configure<JwtSetting>(builder.Configuration.GetSection(nameof(JwtSetting)));
            builder.Services.AddOptions<JwtSetting>().BindConfiguration(nameof(JwtSetting)).ValidateDataAnnotations();
            var settings = builder.Configuration.GetSection(nameof(JwtSetting)).Get<JwtSetting>();

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.Key)),
                    ValidIssuer = settings.Issuer,
                    ValidAudience = settings.Audience,
                };
            });
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection(nameof(EmailSettings)));
            builder.Services.AddHttpContextAccessor();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
                await DefaultRoles.SeedRolesAsync(roleManager);
                await DefaultUser.SeedAdminUserAsync(userManager);
            }
            app.UseAuthorization();


            app.MapControllers();


            app.Run();
        }
    }
}
