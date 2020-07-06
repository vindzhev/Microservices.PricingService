namespace PricingService.Application.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;

    public interface IDataStore : IDisposable
    {
        ITariffRepository Tariffs { get; }

        Task CommitChanges();
    }
}
