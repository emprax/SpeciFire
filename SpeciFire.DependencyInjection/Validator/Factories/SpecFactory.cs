using System;

namespace SpeciFire.Validator
{
    internal class SpecFactory<TContext> : ISpecFactory<TContext>
    {
        private readonly ISpec<TContext> spec;

        internal SpecFactory(ISpec<TContext> spec) => this.spec = spec;

        public ISpec<TContext> Create(IServiceProvider provider) => this.spec;
    }
}
