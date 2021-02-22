using System;
using System.Linq.Expressions;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules
{
    public interface IRuleEngineBuilder<TInput, TContext> where TContext : class
    {
        IRuleEngineBuilder<TInput, TContext> AddRule<TPredicate, TOperation>()
            where TPredicate : class, ISpec<TInput>
            where TOperation : class, IRuleOperation<TInput, TContext>;

        IRuleEngineBuilder<TInput, TContext> AddRule<TOperation>(Expression<Func<TInput, bool>> predicate)
            where TOperation : class, IRuleOperation<TInput, TContext>;

        IRuleEngineBuilder<TInput, TContext> AddRule(ISpec<TInput> predicate, IRuleOperation<TInput, TContext> operation);

        IRuleEngineBuilder<TInput, TContext> AddRule(Expression<Func<TInput, bool>> predicate, IRuleOperation<TInput, TContext> operation);

        IRuleEngineBuilder<TInput, TContext> AddRule(Type predicateType, Type operationType);

        IRuleEngineBuilder<TInput, TContext> AddRule(Expression<Func<TInput, bool>> predicate, Type operationType);

        IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate<TOperation>() where TOperation : class, IRuleOperation<TInput, TContext>;

        IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate(IRuleOperation<TInput, TContext> operation);

        IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate(Type type);

        IRuleEngineBuilder<TInput, TContext> AddRule<TRule>() where TRule : Rule<TInput, TContext>;

        IRuleEngineBuilder<TInput, TContext> AddRule(Rule<TInput, TContext> rule);

        IRuleEngineBuilder<TInput, TContext> AddRule(Type type);
    }
}
