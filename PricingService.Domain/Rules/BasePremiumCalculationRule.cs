namespace PricingService.Domain.Rules
{
    using System.Linq;
    
    using DynamicExpresso;
    
    using PricingService.Domain.Entities;

    public class BasePremiumCalculationRule
    {
        public BasePremiumCalculationRule(string coverCode, string applyIfFormula, string basePriceFormula)
        {
            this.CoverCode = coverCode;
            this.ApplyIfFormula = applyIfFormula;
            this.BasePriceFormula = basePriceFormula;
        }

        public string CoverCode { get; private set; }

        public string ApplyIfFormula { get; private set; }

        public string BasePriceFormula { get; private set; }

        public bool Applies(Cover cover, Calculation calculation)
        {
            if (cover.Code != this.CoverCode)
                return false;

            if (string.IsNullOrEmpty(this.ApplyIfFormula))
                return true;

            var (parameterDefinitions, values) = calculation.ToCalculationParams();

            return (bool)new Interpreter().Parse(this.ApplyIfFormula, parameterDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

        public decimal CalculateBasePrice(Calculation calculation)
        {
            var (parameterDefinitions, values) = calculation.ToCalculationParams();

            return (decimal)new Interpreter().Parse(this.BasePriceFormula, parameterDefinitions.ToArray())
                .Invoke(values.ToArray());
        }

    }
}
