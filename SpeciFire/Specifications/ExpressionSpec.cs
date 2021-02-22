using System;
using System.Linq.Expressions;

namespace SpeciFire.Specifications
{
    public class ExpressionSpec<TContext> : Spec<TContext>
    {
        private readonly Expression<Func<TContext, bool>> expression;

        public ExpressionSpec(Expression<Func<TContext, bool>> expression) => this.expression = expression;

        public override Expression<Func<TContext, bool>> AsExpression() => this.expression;
    }
}
