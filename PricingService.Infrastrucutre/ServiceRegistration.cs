namespace PricingService.Infrastrucutre
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using MicroservicesPOC.Shared.Common;

    using PricingService.Infrastrucutre.Services;
    using PricingService.Application.Common.Interfaces;
    using PricingService.Infrastrucutre.Persistance;

    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(MartenDocumentStore.CreateDocumentStore(configuration.GetConnectionString("PricingServiceConnection")));
            services.AddScoped<IDataStore, DocumentDbStore>();

            services.AddConventionalServices(typeof(ServiceRegistration).Assembly);

            return services;
        }
    }
}
