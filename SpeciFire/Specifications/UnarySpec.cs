using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpeciFire.Specifications
{
    public abstract class UnarySpec<TContext> : Spec<TContext>
    {
        private readonly ISpec<TContext> specification;

        public UnarySpec(ISpec<TContext> specification) => this.specification = specification;

        protected abstract UnaryExpression AsUnary(Expression expression);

        public override Expression<Func<TContext, bool>> AsExpression()
        {
            var expression = this.specification.AsExpression();
            var unary = this.AsUnary(expression.Body);

            return Expression.Lambda<Func<TContext, bool>>(unary, expression.Parameters.Single());
        }
    }
}
