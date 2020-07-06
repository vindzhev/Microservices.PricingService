namespace PricingService.Application.Common.Mappings
{
    using System.Linq;
    
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Common.Models;
    
    using PricingService.Domain.Entities;
    using PricingService.Application.Tariffs.Commands;

    public class CalculationProfile : Profile
    {
        public CalculationProfile()
        {
            this.CreateMap<CalculatePriceCommand, Calculation>()
                .ForMember(x => x.Subject, opt => opt.MapFrom(src => src.Answers.ToDictionary(x => x.QuestionCode, x => x.GetAnswer())))
                .ForMember(x => x.Covers, opt => opt.MapFrom(src => src.SelectedCovers.ToDictionary(x => x, x => new Cover(x, 0))));

            this.CreateMap<Calculation, CalculatePriceResult>()
                .ForMember(x => x.TotalPrice, opt => opt.MapFrom(src => src.TotalPremium))
                .ForMember(x => x.CoverPrices, opt => opt.MapFrom(src => src.Covers.ToDictionary(x => x.Key, x => x.Value.Price)));
        }
    }
}
