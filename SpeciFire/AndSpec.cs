using System.Linq.Expressions;

namespace SpeciFire
{
    public sealed class AndSpec<TContext> : BinarySpec<TContext>
    {
        public AndSpec(ISpec<TContext> left, ISpec<TContext> right) : base(left, right) { }

        protected override BinaryExpression AsBinary(Expression left, Expression right) => Expression.AndAlso(left, right);
    }
}
