namespace SpeciFire.Validator
{
    public interface ISpecValidator<TContext>
    {
        bool Validate(TContext context);
    }
}
