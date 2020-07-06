namespace PricingService.Infrastrucutre.Persistance.Repositories
{
    using System.Threading.Tasks;

    using Marten;

    using PricingService.Domain.Entities;
    using PricingService.Application.Common.Interfaces;

    public class TariffRepository : ITariffRepository
    {
        private readonly IDocumentSession _session;

        public TariffRepository(IDocumentSession session) => this._session = session;

        public void Add(Tariff tariff) => this._session.Insert(tariff);

        public async Task<bool> Exists(string code) => await this._session.Query<Tariff>().AnyAsync(x => x.Code == code);

        public async Task<Tariff> WithCode(string code) => await this._session.Query<Tariff>().FirstOrDefaultAsync(x => x.Code == code);

    }
}
