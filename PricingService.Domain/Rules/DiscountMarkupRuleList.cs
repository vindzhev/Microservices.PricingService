namespace PricingService.Domain.Rules
{
    using System.Linq;
    using System.Collections.Generic;

    using PricingService.Domain.Entities;

    using MicroservicesPOC.Shared.Extensions;

    public class DiscountMarkupRuleList
    {
        private readonly ICollection<DiscountMarkupRule> _rules;

        public DiscountMarkupRuleList(ICollection<DiscountMarkupRule> rules) => this._rules = rules;

        public void AddRule(string applyIfFormula, decimal value) =>
            this._rules.Add(new PercentMarkupRule(applyIfFormula, value));

        public void Apply(Calculation calculation) =>
            this._rules.Where(x => x.Applies(calculation))
                .ForEach(x => x.Apply(calculation));
    }
}
