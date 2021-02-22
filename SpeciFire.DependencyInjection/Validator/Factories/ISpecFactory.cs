using System;

namespace SpeciFire.Validator
{
    internal interface ISpecFactory<TContext>
    {
        ISpec<TContext> Create(IServiceProvider provider);
    }
}
