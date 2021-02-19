namespace SpeciFire.Rules
{
    public interface IRuleOperation<TContext, TResult>
    {
        TResult Execute(TContext context);
    }
}
