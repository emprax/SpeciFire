using System;
using System.Linq.Expressions;

namespace SpeciFire
{
    public interface ISpec<TContext>
    {
        Expression<Func<TContext, bool>> AsExpression();

        bool IsSatisfiedBy(TContext context);
    }
}
