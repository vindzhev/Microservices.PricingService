namespace PricingService.Domain.Rules
{
    using System;
    using System.Collections.Generic;

    using PricingService.Domain.Entities;

    public class PercentMarkupRule : DiscountMarkupRule
    {
        public PercentMarkupRule(string applyIfFormula, decimal parameterValue)
        {
            this.ApplyIfFormula = applyIfFormula;
            this.ParameterValue = parameterValue;
        }

        public override Calculation Apply(Calculation calculation)
        {
            foreach (KeyValuePair<string, Cover> cover in calculation.Covers)
                cover.Value.SetPrice(Math.Round(cover.Value.Price * this.ParameterValue, 2, MidpointRounding.AwayFromZero));

            return calculation;
        }
    }
}
