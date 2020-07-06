namespace PricingService.Domain.Rules
{
    using System.Linq;
    using System.Collections.Generic;

    using PricingService.Domain.Entities;

    public class BasePremiumCalculationRuleList
    {
        private readonly ICollection<BasePremiumCalculationRule> _rules;

        public BasePremiumCalculationRuleList(ICollection<BasePremiumCalculationRule> rules) => this._rules = rules;

        public void AddRule(string coverCode, string applyIfFormula, string basePriceFormula) =>
            this._rules.Add(new BasePremiumCalculationRule(coverCode, applyIfFormula, basePriceFormula));

        public decimal CalculateBasePrice(Cover cover, Calculation calculation) =>
            this._rules.Where(x => x.Applies(cover, calculation))
                .Select(x => x.CalculateBasePrice(calculation))
                .FirstOrDefault();
    }
}
