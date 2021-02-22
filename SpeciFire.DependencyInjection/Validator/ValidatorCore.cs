using System;
using System.Collections.Generic;

namespace SpeciFire.Validator
{
    internal class ValidatorCore<TContext>
    {
        internal ValidatorCore(IReadOnlyList<Func<ISpec<TContext>>> specs) => this.Specs = specs;

        internal IReadOnlyList<Func<ISpec<TContext>>> Specs { get; }
    }
}
