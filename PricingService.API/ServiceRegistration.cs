namespace PricingService.API
{
    using MicroservicesPOC.Shared;

    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceRegistration
    {
        public static IServiceCollection AddApiComponents(this IServiceCollection services) =>
            services.AddHttpContextAccessor()
                .AddConventionalServices(typeof(ServiceRegistration).Assembly);
    }
}
