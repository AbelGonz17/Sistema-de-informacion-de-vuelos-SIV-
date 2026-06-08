using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SIV.Application.Common.Behaviors;
using System.Reflection;

namespace SIV.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this ServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidacionOperativaBehavior<,>));
            });

            return services;
        }
    }
}