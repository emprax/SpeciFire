using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SpeciFire.Rules;
using SpeciFire.Specifications;

namespace SpeciFire.DependencyInjection.Rules
{
    internal sealed class RuleEngineBuilder<TInput, TContext> : IRuleEngineBuilder<TInput, TContext> where TContext : class
    {
        private readonly IList<IRuleFactory<TInput, TContext>> rules;

        internal RuleEngineBuilder() => this.rules = new List<IRuleFactory<TInput, TContext>>();

        public IRuleEngineBuilder<TInput, TContext> AddRule<TPredicate, TOperation>()
            where TPredicate : class, ISpec<TInput>
            where TOperation : class, IRuleOperation<TInput, TContext>
        {
            this.rules.Add(new TypeRuleFactory<TInput, TContext>(typeof(TPredicate), typeof(TOperation)));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(ISpec<TInput> predicate, IRuleOperation<TInput, TContext> operation)
        {
            this.rules.Add(new RuleFactory<TInput, TContext>(predicate, operation));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(Type predicateType, Type operationType)
        {
            if (!(predicateType?.IsAssignableFrom(typeof(ISpec<TInput>)) ?? false))
            {
                throw new InvalidOperationException($"Type '{predicateType?.Name}' is not assignable from ISpec<{nameof(TInput)}>.");
            }

            if (!(operationType?.IsAssignableFrom(typeof(IRuleOperation<TInput, TContext>)) ?? false))
            {
                throw new InvalidOperationException($"Type '{operationType?.Name}' is not assignable from IRuleOperation<{nameof(TInput)}, {nameof(TContext)}>.");
            }

            this.rules.Add(new TypeRuleFactory<TInput, TContext>(predicateType, operationType));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate<TOperation>() where TOperation : class, IRuleOperation<TInput, TContext>
        {
            this.rules.Add(new TypeRuleWithoutPredicateFactory<TInput, TContext>(typeof(TOperation)));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate(IRuleOperation<TInput, TContext> operation)
        {
            this.rules.Add(new RuleWithoutPredicateFactory<TInput, TContext>(operation));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRuleWithoutPredicate(Type type)
        {
            if (!(type?.IsAssignableFrom(typeof(IRuleOperation<TInput, TContext>)) ?? false))
            {
                throw new InvalidOperationException($"Type '{type?.Name}' is not assignable from IRuleOperation<{nameof(TInput)}, {nameof(TContext)}>.");
            }

            this.rules.Add(new TypeRuleWithoutPredicateFactory<TInput, TContext>(type));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule<TRule>() where TRule : Rule<TInput, TContext>
        {
            this.rules.Add(new TypeRuleCompleteFactory<TInput, TContext>(typeof(TRule)));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(Rule<TInput, TContext> rule)
        {
            this.rules.Add(new RuleCompleteFactory<TInput, TContext>(rule));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(Type type)
        {
            if (!(type?.IsAssignableFrom(typeof(Rule<TInput, TContext>)) ?? false))
            {
                throw new InvalidOperationException($"Type '{type?.Name}' is not assignable from Rule<{nameof(TInput)}, {nameof(TContext)}>.");
            }

            this.rules.Add(new TypeRuleCompleteFactory<TInput, TContext>(type));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule<TOperation>(Expression<Func<TInput, bool>> predicate)
            where TOperation : class, IRuleOperation<TInput, TContext>
        {
            this.rules.Add(new TypeRuleWithExpressionFactory<TInput, TContext>(predicate, typeof(TOperation)));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(Expression<Func<TInput, bool>> predicate, IRuleOperation<TInput, TContext> operation)
        {
            this.rules.Add(new RuleFactory<TInput, TContext>(new ExpressionSpec<TInput>(predicate), operation));
            return this;
        }

        public IRuleEngineBuilder<TInput, TContext> AddRule(Expression<Func<TInput, bool>> predicate, Type type)
        {
            if (!(type?.IsAssignableFrom(typeof(IRuleOperation<TInput, TContext>)) ?? false))
            {
                throw new InvalidOperationException($"Type '{type?.Name}' is not assignable from IRuleOperation<{nameof(TInput)}, {nameof(TContext)}>.");
            }

            this.rules.Add(new TypeRuleWithExpressionFactory<TInput, TContext>(predicate, type));
            return this;
        }

        internal IReadOnlyList<Func<Rule<TInput, TContext>>> Build(IServiceProvider provider)
        {
            return this.rules
                .Select(factory => new Func<Rule<TInput, TContext>>(() => factory.Create(provider)))
                .ToList();
        }
    }
}
