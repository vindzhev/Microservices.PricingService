namespace PricingService.Domain.Entities
{
    using System;
    
    using MicroservicesPOC.Shared.Common.Entities;

    public class Cover
    {
        public Cover(string code, decimal price)
        {
            this.Code = code;
            this.Price = price;
        }

        public string Code { get; private set; }

        public decimal Price { get; private set; }

        public void SetPrice(decimal price) => this.Price = price;
    }
}
