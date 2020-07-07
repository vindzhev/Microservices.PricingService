namespace PricingService.Infrastrucutre.Services
{
    using System.Reflection;
    
    using Marten;
    using Marten.Services;
    
    using Newtonsoft.Json;
    using Newtonsoft.Json.Serialization;
    
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

    public class ProtectedSettersContractResolver : DefaultContractResolver
    {
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);

            if (!prop.Writable)
            {
                var property = member as PropertyInfo;
                if (property != null)
                    prop.Writable = property.GetSetMethod(true) != null;
            }

            return prop;
        }
    }
}
