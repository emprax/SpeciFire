using System;
using System.Collections.Generic;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules;

internal sealed class RuleEngineCore<TInput, TContext> where TContext : class
{
    internal RuleEngineCore(IReadOnlyList<Func<Rule<TInput, TContext>>> rules) => this.Rules = rules;

    internal IReadOnlyList<Func<Rule<TInput, TContext>>> Rules { get; }
}
