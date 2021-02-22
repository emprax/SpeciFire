using System;
using System.Linq;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules
{
    internal sealed class TypeRuleCompleteFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
    {
        private readonly Type ruleType;

        internal TypeRuleCompleteFactory(Type ruleType) => this.ruleType = ruleType;

        public Rule<TInput, TContext> Create(IServiceProvider provider) => Resolve<Rule<TInput, TContext>>(this.ruleType, provider);

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
