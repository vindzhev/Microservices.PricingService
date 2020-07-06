namespace PricingService.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    
    using PricingService.Domain.Rules;
    
    using MicroservicesPOC.Shared.Common.Entities;
    using Newtonsoft.Json;

    public class Tariff : Entity<Guid>
    {
        [JsonProperty]
        private readonly List<DiscountMarkupRule> _discountMarkupRules;

        [JsonProperty]
        private readonly List<BasePremiumCalculationRule> _basePremiumRules;

        public Tariff(string code)
        {
            this.Code = code;

            this._discountMarkupRules = new List<DiscountMarkupRule>();
            this._basePremiumRules = new List<BasePremiumCalculationRule>();
        }

        public string Code { get; set; }

        [JsonIgnore]
        public DiscountMarkupRuleList DiscountMarkupRules => new DiscountMarkupRuleList(this._discountMarkupRules);

        [JsonIgnore]
        public BasePremiumCalculationRuleList BasePremiumRules => new BasePremiumCalculationRuleList(this._basePremiumRules);

        private void CalculateBasePrice(Calculation calculation)
        {
            foreach (Cover cover in calculation.Covers.Values)
                cover.SetPrice(BasePremiumRules.CalculateBasePrice(cover, calculation));
        }

        private void ApplyDiscounts(Calculation calculation) => this.DiscountMarkupRules.Apply(calculation);

        private void UpdateTotals(Calculation calculation) => calculation.UpdateTotals();

        public Calculation CalculatePrice(Calculation calculation)
        {
            CalculateBasePrice(calculation);
            ApplyDiscounts(calculation);
            UpdateTotals(calculation);

            return calculation;
        }
    }
}
