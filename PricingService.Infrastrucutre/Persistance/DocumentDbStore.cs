namespace PricingService.Infrastrucutre.Persistance
{
    using System;
    using System.Threading.Tasks;

    using Marten;

    using PricingService.Application.Common.Interfaces;
    using PricingService.Infrastrucutre.Persistance.Repositories;

    public class DocumentDbStore : IDataStore
    {
        private readonly IDocumentSession _session;
        private readonly ITariffRepository _tariffRepository;

        public DocumentDbStore(IDocumentStore documentStore)
        {
            this._session = documentStore.LightweightSession();
            this._tariffRepository = new TariffRepository(this._session);
        }

        public ITariffRepository Tariffs => this._tariffRepository;

        public async Task CommitChanges() => await this._session.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                this._session.Dispose();
        }
    }
}
