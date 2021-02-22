using System;
using System.Linq;
using System.Linq.Expressions;
using SpeciFire.Rules;
using SpeciFire.Specifications;

namespace SpeciFire.DependencyInjection.Rules
{
    internal sealed class TypeRuleWithExpressionFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
    {
        private readonly Expression<Func<TInput, bool>> predicate;
        private readonly Type operation;

        internal TypeRuleWithExpressionFactory(Expression<Func<TInput, bool>> predicate, Type operation)
        {
            this.predicate = predicate;
            this.operation = operation;
        }

        public Rule<TInput, TContext> Create(IServiceProvider provider)
        {
            var operation = Resolve<IRuleOperation<TInput, TContext>>(this.operation, provider);
            return new Rule<TInput, TContext>(new ExpressionSpec<TInput>(this.predicate), operation);
        }

        private static TObject Resolve<TObject>(Type type, IServiceProvider provider) where TObject : class
        {
            var constructor = type
                .GetConstructors()
                .FirstOrDefault();

            var parameters = constructor
                .GetParameters()
                .Select(p => provider.GetService(p.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters) as TObject;
        }
    }
}
