namespace SpeciFire.Rules
{
    public class Rule<TContext, TResult>
    {
        public Rule(ISpec<TContext> predicate, IRuleOperation<TContext, TResult> operation)
        {
            this.Predicate = predicate;
            this.Operation = operation;
        }

        ISpec<TContext> Predicate { get; }

        IRuleOperation<TContext, TResult> Operation { get; }
    }
}
