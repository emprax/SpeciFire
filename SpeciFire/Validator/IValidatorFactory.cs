namespace SpeciFire.Validator
{
    public interface IValidatorFactory<TContext>
    {
        IValidatorFactory<TContext> With(ISpec<TContext> spec);

        IValidator<TContext> Build();
    }
}
