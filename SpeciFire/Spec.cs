using System;
using System.Linq.Expressions;

namespace SpeciFire
{
    public abstract class Spec<TContext> : ISpec<TContext>
    {
        public abstract Expression<Func<TContext, bool>> AsExpression();

        public bool IsSatisfiedBy(TContext context) => this
            .AsExpression()
            .Compile()
            .Invoke(context);
    }
}
