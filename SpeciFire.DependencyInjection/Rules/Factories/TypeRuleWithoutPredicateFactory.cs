using System;
using System.Linq;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules
{
    internal sealed class TypeRuleWithoutPredicateFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
    {
        private readonly Type type;

        internal TypeRuleWithoutPredicateFactory(Type type) => this.type = type;

        public Rule<TInput, TContext> Create(IServiceProvider provider)
        {
            var operation = Resolve<IRuleOperation<TInput, TContext>>(this.type, provider);
            return new Rule<TInput, TContext>(null, operation);
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
