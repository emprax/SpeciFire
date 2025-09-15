using System;

namespace SpeciFire.DependencyInjection.Validator.Factories;

internal interface ISpecFactory<TContext>
{
    ISpec<TContext> Create(IServiceProvider provider);
}
