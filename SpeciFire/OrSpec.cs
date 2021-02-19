using System.Linq.Expressions;

namespace SpeciFire
{
    public sealed class OrSpec<TContext> : BinarySpec<TContext>
    {
        public OrSpec(ISpec<TContext> left, ISpec<TContext> right) : base(left, right) { }

        protected override BinaryExpression AsBinary(Expression left, Expression right) => Expression.OrElse(left, right);
    }
}
