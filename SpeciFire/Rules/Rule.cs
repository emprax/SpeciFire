namespace SpeciFire.Rules;

public class Rule<TInput, TContext> where TContext : class
{
    public Rule(ISpec<TInput> predicate, IRuleOperation<TInput, TContext> operation)
    {
        this.Predicate = predicate;
        this.Operation = operation;
    }

    public ISpec<TInput> Predicate { get; }

    public IRuleOperation<TInput, TContext> Operation { get; }
}
