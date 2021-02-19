namespace SpeciFire.Rules
{
    public interface IRuleEngine<TContext, TResult>
    {
        TResult Execute(TContext context);
    }
}
