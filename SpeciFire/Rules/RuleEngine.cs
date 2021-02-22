using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SpeciFire.Rules
{
    public class RuleEngine<TInput, TContext> : IRuleEngine<TInput, TContext> where TContext : class
    {
        private readonly IReadOnlyList<Func<Rule<TInput, TContext>>> rules;

        public RuleEngine(IReadOnlyList<Func<Rule<TInput, TContext>>> rules) => this.rules = rules;

        public TContext Context { get; set; }

        public async Task Execute(TInput input)
        {
            foreach (var ruleFactory in this.rules)
            {
                var rule = ruleFactory?.Invoke();
                if (rule?.Predicate?.IsSatisfiedBy(input) ?? true)
                {
                    await rule?.Operation?.Execute(input, this.Context);
                }
            }
        }
    }
}
