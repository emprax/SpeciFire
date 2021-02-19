using System;

namespace SpeciFire.Rules
{
    public interface IRuleEngineFactory<TContext>
    {
        IRuleEngineFactory<TContext> With(Action<IRuleEngineFactoryBuilder<TContext>> builder);

        IRuleEngine<TContext, TResult> Create<TResult>(Func<TContext, TResult> commonOperation);
    }
}
