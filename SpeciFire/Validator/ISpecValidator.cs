namespace SpeciFire.Validator;

public interface ISpecValidator<TContext>
{
    ISpecValidator<TContext> WithAggregationType(AggregationType<TContext> type);

    ValidationResult Validate(TContext context);
}
