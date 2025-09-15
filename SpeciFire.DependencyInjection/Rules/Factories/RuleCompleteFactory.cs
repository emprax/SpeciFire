using System;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules.Factories;

internal sealed class RuleCompleteFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
{
    private readonly Rule<TInput, TContext> rule;

    internal RuleCompleteFactory(Rule<TInput, TContext> rule) => this.rule = rule;

    public Rule<TInput, TContext> Create(IServiceProvider provider) => this.rule;
}
