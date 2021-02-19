using System;
using System.Linq;
using System.Linq.Expressions;

namespace SpeciFire.Specifications
{
    public abstract class BinarySpec<TContext> : Spec<TContext>
    {
        private readonly ISpec<TContext> left;
        private readonly ISpec<TContext> right;

        protected BinarySpec(ISpec<TContext> left, ISpec<TContext> right)
        {
            this.left = left;
            this.right = right;
        }

        protected abstract BinaryExpression AsBinary(Expression left, Expression right);

        public override Expression<Func<TContext, bool>> AsExpression()
        {
            var left = this.left.AsExpression();
            var right = this.right.AsExpression();
            var binaryExpression = this.AsBinary(left.Body, right.Body);

            return Expression.Lambda<Func<TContext, bool>>(binaryExpression, left.Parameters.Single());
        }
    }
}
