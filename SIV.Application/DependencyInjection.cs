using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SIV.Application.Common.Behaviors;
using System.Reflection;

namespace SIV.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidacionOperativaBehavior<,>));
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidacionBehavior<,>));
            });

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransaccionBehavior<,>));

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}