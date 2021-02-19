namespace SpeciFire.Rules
{
    public interface ISpecEngineFactoryProvider
    {
        IRuleEngineFactory<TContext> GetFactory<TContext>();
    }
}
