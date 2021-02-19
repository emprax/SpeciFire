namespace SpeciFire.Validator
{
    public interface IValidator<TContext>
    {
        bool Validate(TContext context);
    }
}
