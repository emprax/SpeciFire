namespace SpeciFire.Rules
{
    public interface IRuleEngineFactoryBuilder<TContext>
    {
        IRuleEngineFactoryBuilder<TContext> With<TResult>(ISpec<TContext> spec, IRuleOperation<TContext, TResult> operation);

        IRuleEngineFactoryBuilder<TContext> With<TResult>(Rule<TContext, TResult> rule);

        IRuleEngineFactory<TContext> Build();
    }
}
