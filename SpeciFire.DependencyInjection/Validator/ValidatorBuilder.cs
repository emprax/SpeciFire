using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SpeciFire.Validator
{
    internal sealed class ValidatorBuilder<TContext> : IValidatorBuilder<TContext>
    {
        private IList<ISpecFactory<TContext>> specifications;

        internal ValidatorBuilder() => this.specifications = new List<ISpecFactory<TContext>>();

        public IValidatorBuilder<TContext> With<TSpec>() where TSpec : class, ISpec<TContext>
        {
            this.specifications.Add(new TypeSpecFactory<TContext>(typeof(TSpec)));
            return this;
        }

        public IValidatorBuilder<TContext> With(Type type)
        {
            if (!(type?.IsAssignableFrom(typeof(ISpec<TContext>)) ?? false))
            {
                throw new InvalidOperationException($"Type provided type '{type?.Name}' is not assignable from ISpec<{nameof(TContext)}>.");
            }

            this.specifications.Add(new TypeSpecFactory<TContext>(type));
            return this;
        }

        public IValidatorBuilder<TContext> With(Expression<Func<TContext, bool>> predicate)
        {
            this.specifications.Add(new PredicateSpecFactory<TContext>(predicate));
            return this;
        }

        public IValidatorBuilder<TContext> With(ISpec<TContext> spec)
        {
            this.specifications.Add(new SpecFactory<TContext>(spec));
            return this;
        }

        internal IReadOnlyList<Func<ISpec<TContext>>> Build(IServiceProvider provider)
        {
            return this.specifications
                .Select(spec => new Func<ISpec<TContext>>(() => spec.Create(provider)))
                .ToList();
        }
    }
}
