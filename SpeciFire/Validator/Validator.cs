using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator
{
    public class Validator<TContext> : ISpecValidator<TContext>
    {
        private readonly IReadOnlyList<Func<ISpec<TContext>>> rules;

        public Validator(IReadOnlyList<Func<ISpec<TContext>>> rules) => this.rules = rules;

        public bool Validate(TContext context)
        {
            return this.rules?.All(rule => rule.Invoke().IsSatisfiedBy(context)) ?? true;
        }
    }
}
