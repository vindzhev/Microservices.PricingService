namespace PricingService.Application.Common.Interfaces
{
    using System.Threading.Tasks;

    using PricingService.Domain.Entities;
    
    public interface ITariffRepository
    {
        Task<Tariff> WithCode(string code);

        void Add(Tariff tariff);

        Task<bool> Exists(string code);
    }
}
