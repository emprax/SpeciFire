using System;
using System.Linq;
using SpeciFire.Rules;

namespace SpeciFire.DependencyInjection.Rules.Factories;

internal sealed class TypeRuleFactory<TInput, TContext> : IRuleFactory<TInput, TContext> where TContext : class
{
    private readonly Type predicate;
    private readonly Type operation;

    internal TypeRuleFactory(Type predicate, Type operation)
    {
        this.predicate = predicate;
        this.operation = operation;
    }

    public Rule<TInput, TContext> Create(IServiceProvider provider)
    {
        var predicate = Resolve<ISpec<TInput>>(this.predicate, provider);
        var operation = Resolve<IRuleOperation<TInput, TContext>>(this.operation, provider);

        return new Rule<TInput, TContext>(predicate, operation);
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
