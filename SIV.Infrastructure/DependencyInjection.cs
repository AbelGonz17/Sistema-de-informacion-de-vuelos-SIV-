using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SIV.Domain.Interfaces;
using SIV.Infrastructure.Persistence;
using SIV.Infrastructure.Persistence.Repositories;
using SIV.Infrastructure.RealTime;
using SIV.Infrastructure.Security;
using SIV.Application.Common.Interfaces;
using SIV.Infrastructure.Services;

namespace SIV.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(
                    configuration.GetConnectionString("DefaultConnection"),
                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            #region Repositories, Services, UnitOfWork
            services.AddScoped<IVueloRepository, VueloRepository>();
            services.AddScoped<IAuditoriaRepository, AuditoriaRepository>();
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();
            services.AddScoped<INotificacionRepository, NotificacionRepository>();
            services.AddScoped<IAerolineaRepository, AerolineaRepository>();
            services.AddScoped<IAeropuertoRepository, AeropuertoRepository>();
            services.AddScoped<IReportesRepository, ReportesRepository>();

            services.AddScoped<INotificacionService, NotificacionService>();
            services.AddScoped<ISeguridadService, SeguridadService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IPasswordHasher, PasswordHasher>();
            
            services.AddHttpContextAccessor();
            services.AddSignalR();
            services.AddHttpClient(); // Requerido por ResendEmailService

            if (configuration.GetValue<bool>("EmailSettings:UseMock", true))
            {
                services.AddScoped<IEmailService, MockEmailService>();
            }
            else
            {
                services.AddScoped<IEmailService, ResendEmailService>();
            }

            return services;
        }
    }
}