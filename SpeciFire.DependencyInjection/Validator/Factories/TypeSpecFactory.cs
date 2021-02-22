using System;
using System.Linq;

namespace SpeciFire.Validator
{
    internal class TypeSpecFactory<TContext> : ISpecFactory<TContext>
    {
        private readonly Type type;

        internal TypeSpecFactory(Type type) => this.type = type;

        public ISpec<TContext> Create(IServiceProvider provider)
        {
            var constructor = type
                .GetConstructors()
                .FirstOrDefault();

            var parameters = constructor
                .GetParameters()
                .Select(p => provider.GetService(p.ParameterType))
                .ToArray();

            return constructor.Invoke(parameters) as ISpec<TContext>;
        }
    }
}
