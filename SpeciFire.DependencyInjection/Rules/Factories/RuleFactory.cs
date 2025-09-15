using System;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules.Factories;

internal sealed class RuleFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
{
    private readonly ISpec<TInput> predicate;
    private readonly IRuleOperation<TInput, TContext> operation;

    internal RuleFactory(ISpec<TInput> predicate, IRuleOperation<TInput, TContext> operation)
    {
        this.predicate = predicate;
        this.operation = operation;
    }

    public Rule<TInput, TContext> Create(IServiceProvider provider) => new Rule<TInput, TContext>(this.predicate, this.operation);
}
