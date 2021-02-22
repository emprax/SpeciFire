using System;
using System.Linq.Expressions;

namespace SpeciFire.Validator
{
    public interface IValidatorBuilder<TContext>
    {
        IValidatorBuilder<TContext> With<TSpec>() where TSpec : class, ISpec<TContext>;

        IValidatorBuilder<TContext> With(ISpec<TContext> spec);

        IValidatorBuilder<TContext> With(Expression<Func<TContext, bool>> predicate);
    }
}
