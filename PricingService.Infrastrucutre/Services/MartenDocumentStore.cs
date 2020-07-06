namespace PricingService.Infrastrucutre.Services
{
    using Marten;
    using Marten.Services;
    
    using MicroservicesPOC.Shared.Extensions;

    using PricingService.Domain.Entities;

    public static class MartenDocumentStore
    {
        private static JsonNetSerializer CustomizeJsonSerializer()
        {
            var serializer = new JsonNetSerializer();
            serializer.Customize(_ => _.ContractResolver = new ProtectedSettersContractResolver());

            return serializer;
        }

        public static IDocumentStore CreateDocumentStore(string connectionString) =>
            DocumentStore.For(_ =>
            {
                _.Connection(connectionString);
                _.DatabaseSchemaName = "policy_service";
                _.Serializer(CustomizeJsonSerializer());

                _.Schema.For<Tariff>().Duplicate(x => x.Code, pgType: "varchar(50)", configure: idx => idx.IsUnique = true);
            });

    }
}
