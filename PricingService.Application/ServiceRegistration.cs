namespace PricingService.Application
{
    using System.Reflection;
    
    using MediatR;
    using AutoMapper;

    using Microsoft.Extensions.DependencyInjection;
    
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services) =>
            services.AddAutoMapper(Assembly.GetExecutingAssembly())
                    .AddMediatR(Assembly.GetExecutingAssembly());
    }
}
