using System.Linq.Expressions;

namespace SpeciFire
{
    public sealed class NotSpec<TContext> : UnarySpec<TContext>
    {
        public NotSpec(ISpec<TContext> specification) : base(specification) { }

        protected override UnaryExpression AsUnary(Expression expression) => Expression.Not(expression);
    }
}
