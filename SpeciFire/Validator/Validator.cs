using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator
{
    public class Validator<TContext> : IValidator<TContext>
    {
        private readonly IReadOnlyList<ISpec<TContext>> rules;

        public Validator(IReadOnlyList<ISpec<TContext>> rules) => this.rules = rules;

        public bool Validate(TContext context)
        {
            return this.rules?.All(rule => rule.IsSatisfiedBy(context)) ?? true;
        }
    }
}
