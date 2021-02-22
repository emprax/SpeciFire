using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator
{
    public class Validator<TContext> : ISpecValidator<TContext>
    {
        private readonly IReadOnlyList<Func<ISpec<TContext>>> rules;
        private AggregationType<TContext> aggregationType;

        public Validator(IReadOnlyList<Func<ISpec<TContext>>> rules)
        {
            this.rules = rules;
            this.aggregationType = AggregationType<TContext>.All;
        }

        public ISpecValidator<TContext> WithAggregationType(AggregationType<TContext> type)
        {
            this.aggregationType = type;
            return this;
        }

        public bool Validate(TContext context)
        {
            var rules = this.rules.Select(r => r.Invoke()).ToList();
            return this.aggregationType.Operation.Invoke(context, rules);
        }
    }
}
