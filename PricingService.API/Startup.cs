namespace PricingService.API
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using Newtonsoft.Json.Serialization;

    using PricingService.Application;
    using PricingService.Infrastrucutre;
    using PricingService.API.Extensions;

    using MicroservicesPOC.Shared.Extensions;
    using MicroservicesPOC.Shared.API.ServiceDiscovery;
    using PricingService.Application.Common.Interfaces;
    using System;
    using System.Threading.Tasks;

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.RegisterConsulServices(this.Configuration.GetServiceConfig());

            services
                .AddApplication()
                .AddInfrastructure(this.Configuration)
                .AddApiComponents();

            services.AddHealthChecks();

            services
                .AddControllers(setupAction => setupAction.ReturnHttpNotAcceptable = true)
                .AddNewtonsoftJson(setupAction =>
                {
                    //setupAction.SerializerSettings.TypeNameHandling = Newtonsoft.Json.TypeNameHandling.All;
                    setupAction.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    setupAction.SerializerSettings.Converters.Add(new QuestionAnswerConverter());
                })
                .AddXmlDataContractSerializerFormatters()
                .ConfigureApiBehaviorOptions(setupAction => setupAction.UseCustomInvalidModelUnprocessableEntityResponse());

            //Register custom 422 api behavior
            services.Configure<ApiBehaviorOptions>(options => options.SuppressModelStateInvalidFilter = true);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                UpdateDatabase(app);
                app.UseDeveloperExceptionPage();
            }
            else app.UseHsts();

            //app.UseCustomExceptionHandler();
            app.UseHealthChecks("/health");
            //app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        public async Task UpdateDatabase(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
            var dataStore = serviceScope.ServiceProvider.GetService<IDataStore>();

            if (!await dataStore.Tariffs.Exists("HSI"))
            {
                var house = new Domain.Entities.Tariff("HSI");

                house.BasePremiumRules.AddRule("C1", "TYP == \"APT\"", "AREA * 1.25M");
                house.BasePremiumRules.AddRule("C1", "TYP == \"HOUSE\"", "AREA * 1.50M");

                house.BasePremiumRules.AddRule("C2", "TYP == \"APT\"", "AREA * 0.25M");
                house.BasePremiumRules.AddRule("C2", "TYP == \"HOUSE\"", "AREA * 0.45M");

                house.BasePremiumRules.AddRule("C3", null, "30M");
                house.BasePremiumRules.AddRule("C4", null, "50M");

                house.DiscountMarkupRules.AddRule("FLOOD == \"YES\"", 1.50M);
                house.DiscountMarkupRules.AddRule("NUM_OF_CLAIM > 1 ", 1.25M);

                dataStore.Tariffs.Add(house);
            }

            if (!await dataStore.Tariffs.Exists("TRI"))
            {
                var travel = new Domain.Entities.Tariff("TRI");
                travel.BasePremiumRules.AddRule("C1", null, "(NUM_OF_ADULTS) * (DESTINATION == \"EUR\" ? 26.00M : 34.00M)");
                travel.BasePremiumRules.AddRule("C2", null, "(NUM_OF_ADULTS + NUM_OF_CHILDREN) * 26.00M");
                travel.BasePremiumRules.AddRule("C3", null, "(NUM_OF_ADULTS + NUM_OF_CHILDREN) * 10.00M");

                travel.DiscountMarkupRules.AddRule("DESTINATION == \"WORLD\"", 1.5M);

                dataStore.Tariffs.Add(travel);
            }

            if (!await dataStore.Tariffs.Exists("FAI"))
            {
                var farm = new Domain.Entities.Tariff("FAI");

                farm.BasePremiumRules.AddRule("C1", null, "10M");
                farm.BasePremiumRules.AddRule("C2", null, "20M");
                farm.BasePremiumRules.AddRule("C3", null, "30M");
                farm.BasePremiumRules.AddRule("C4", null, "40M");

                //farm.DiscountMarkupRules.AddPercentMarkup("FLOOD == \"YES\"", 1.50M);
                farm.DiscountMarkupRules.AddRule("NUM_OF_CLAIM > 2", 2.00M);

                dataStore.Tariffs.Add(farm);
            }

            if (!await dataStore.Tariffs.Exists("CAR"))
            {
                var car = new Domain.Entities.Tariff("CAR");

                car.BasePremiumRules.AddRule("C1", null, "100M");
                car.DiscountMarkupRules.AddRule("NUM_OF_CLAIM > 2", 1.50M);

                dataStore.Tariffs.Add(car);
            }

            dataStore.CommitChanges();
        }
    }
}