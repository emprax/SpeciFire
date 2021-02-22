using System;
using System.Linq.Expressions;
using SpeciFire.Specifications;

namespace SpeciFire.Validator
{
    internal class PredicateSpecFactory<TContext> : ISpecFactory<TContext>
    {
        private readonly Expression<Func<TContext, bool>> predicate;

        internal PredicateSpecFactory(Expression<Func<TContext, bool>> predicate) => this.predicate = predicate;

        public ISpec<TContext> Create(IServiceProvider provider) => new ExpressionSpec<TContext>(this.predicate);
    }
}
