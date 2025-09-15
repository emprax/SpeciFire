using System;
using Microsoft.Extensions.DependencyInjection;
using SpeciFire.DependencyInjection.Rules;
using SpeciFire.DependencyInjection.Validator;
using SpeciFire.Rules;
using SpeciFire.Validator;

namespace SpeciFire.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRuleEngine<TInput, TContext>(this IServiceCollection services, Action<IRuleEngineBuilder<TInput, TContext>, IServiceProvider> configuration)
        where TContext : class
    {
        return services
            .AddTransient<IRuleEngine<TInput, TContext>>(provider => new RuleEngine<TInput, TContext>(provider.GetRequiredService<RuleEngineCore<TInput, TContext>>().Rules))
            .AddSingleton(provider => 
            {
                var builder = new RuleEngineBuilder<TInput, TContext>();
                configuration.Invoke(builder, provider);
                return new RuleEngineCore<TInput, TContext>(builder.Build(provider));
            });
    }

    public static IServiceCollection AddSpecValidator<TContext>(this IServiceCollection services, Action<IValidatorBuilder<TContext>, IServiceProvider> configuration)
    {
        return services
            .AddTransient<ISpecValidator<TContext>>(provider => new SpecValidator<TContext>(provider.GetRequiredService<ValidatorCore<TContext>>().Specs))
            .AddSingleton(provider => 
            {
                var builder = new ValidatorBuilder<TContext>();
                configuration.Invoke(builder, provider);
                return new ValidatorCore<TContext>(builder.Build(provider));
            });
    }
}
