namespace PricingService.Application.Tariffs.Commands
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Collections.Generic;
    
    using MediatR;
    using AutoMapper;
    
    using MicroservicesPOC.Shared.Common.Models;

    using PricingService.Domain.Entities;
    using PricingService.Application.Common.Interfaces;

    public class CalculatePriceCommand : IRequest<CalculatePriceResult>
    {
        public CalculatePriceCommand(string productCode, DateTimeOffset policyFrom, DateTimeOffset policyTo, ICollection<string> selectedCovers, ICollection<QuestionAnswerDTO> answers)
        {
            this.ProductCode = productCode;
            this.PolicyFrom = policyFrom;
            this.PolicyTo = policyTo;
            this.SelectedCovers = selectedCovers;
            this.Answers = answers;
        }

        public string ProductCode { get; set; }

        public DateTimeOffset PolicyFrom { get; set; }

        public DateTimeOffset PolicyTo { get; set; }

        public ICollection<string> SelectedCovers { get; set; }

        public ICollection<QuestionAnswerDTO> Answers { get; set; }

        public class CalculatePriceCommandHandler : IRequestHandler<CalculatePriceCommand, CalculatePriceResult>
        {
            private readonly IMapper _mapper;
            private readonly IDataStore _data;
            //TODO: private readonly CalculatePriceCommandValidator _commandValidator = new CalculatePriceCommandValidator();

            public CalculatePriceCommandHandler(IMapper mapper, IDataStore dataStore)
            {
                this._mapper = mapper;
                this._data = dataStore;
            }

            public async Task<CalculatePriceResult> Handle(CalculatePriceCommand request, CancellationToken cancellationToken)
            {
                //TODO: Move to behaviour pipeline
                //this._commandValidator.ValidateAndThrow(request);

                Tariff tarrif = await this._data.Tariffs.WithCode(request.ProductCode);
                
                return this._mapper.Map<CalculatePriceResult>(
                    tarrif.CalculatePrice(this._mapper.Map<Calculation>(request)));
            }
        }
    }
}
