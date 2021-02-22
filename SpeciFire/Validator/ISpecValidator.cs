namespace SpeciFire.Validator
{
    public interface ISpecValidator<TContext>
    {
        ISpecValidator<TContext> WithAggregationType(AggregationType<TContext> type);

        bool Validate(TContext context);
    }
}
