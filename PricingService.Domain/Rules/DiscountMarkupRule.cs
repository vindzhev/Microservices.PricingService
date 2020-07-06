namespace PricingService.Domain.Rules
{
    using System.Linq;
    
    using DynamicExpresso;
    
    using PricingService.Domain.Entities;

    public abstract class DiscountMarkupRule
    {
        public string ApplyIfFormula { get; protected set; }

        public decimal ParameterValue { get; protected set; }

        public bool Applies(Calculation calculation)
        {
            if (string.IsNullOrEmpty(this.ApplyIfFormula))
                return true;

            var (parameterDefinitions, values) = calculation.ToCalculationParams();
            return (bool)new Interpreter().Parse(this.ApplyIfFormula, parameterDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

        public abstract Calculation Apply(Calculation calculation);
    }
}
