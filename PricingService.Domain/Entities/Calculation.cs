namespace PricingService.Domain.Entities
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    
    using DynamicExpresso;
    
    using MicroservicesPOC.Shared.Extensions;

    public class Calculation
    {
        public Calculation() { }

        public Calculation(string productCode, DateTimeOffset policyFrom, DateTimeOffset policyTo, IEnumerable<string> selectedCovers, IDictionary<string, object> subject)
        {
            this.ProductCode = productCode;

            this.PolicyFrom = policyFrom;
            this.PolicyTo = policyTo;

            this.Subject = subject;

            selectedCovers.ForEach(ZeroPrice);
        }

        public string ProductCode { get; private set; }

        public DateTimeOffset PolicyFrom { get; private set; }

        public DateTimeOffset PolicyTo { get; private set; }

        public decimal TotalPremium { get; private set; }

        public IDictionary<string, Cover> Covers { get; private set; } = new Dictionary<string, Cover>();

        public IDictionary<string, object> Subject { get; private set; } = new Dictionary<string, object>();

        private void ZeroPrice(string coverCode) => this.Covers.Add(coverCode, new Cover(coverCode, 0));

        public void UpdateTotals() => this.TotalPremium = this.Covers.Values.Sum(x => x.Price);

        public (ICollection<Parameter>, ICollection<object>) ToCalculationParams()
        {
            ICollection<object> values = new List<object>();
            ICollection<Parameter> parameters = new List<Parameter>();

            parameters.Add(new Parameter("policyFrom", typeof(DateTimeOffset)));
            values.Add(this.PolicyFrom);

            parameters.Add(new Parameter("policyTo", typeof(DateTimeOffset)));
            values.Add(this.PolicyTo);

            foreach (KeyValuePair<string, Cover> cover in this.Covers)
            {
                parameters.Add(new Parameter(cover.Key, typeof(Cover)));
                values.Add(cover.Value);
            }

            foreach (KeyValuePair<string, object> question in this.Subject)
            {
                parameters.Add(new Parameter(question.Key, question.Value.GetType()));
                values.Add(question.Value);
            }

            return (parameters, values);
        }
    }
}
