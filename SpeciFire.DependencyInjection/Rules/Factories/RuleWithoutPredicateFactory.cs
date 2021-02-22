using System;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules
{
    internal sealed class RuleWithoutPredicateFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
    {
        private readonly IRuleOperation<TInput, TContext> operation;

        internal RuleWithoutPredicateFactory(IRuleOperation<TInput, TContext> rule) => this.operation = rule;

        public Rule<TInput, TContext> Create(IServiceProvider provider) => new Rule<TInput, TContext>(null, this.operation);
    }
}
