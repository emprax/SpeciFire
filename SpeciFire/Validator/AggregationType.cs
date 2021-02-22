using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator
{
    public class AggregationType<TContext>
    {
        public static readonly AggregationType<TContext> All = new AggregationType<TContext>(1, (input, specs) => specs.All(spec => spec.IsSatisfiedBy(input)));
        public static readonly AggregationType<TContext> Any = new AggregationType<TContext>(2, (input, specs) => specs.Any(spec => spec.IsSatisfiedBy(input)));
        public static readonly Func<uint, AggregationType<TContext>> AtLeast = new Func<uint, AggregationType<TContext>>(x => 
            new AggregationType<TContext>(3, (input, specs) => specs
                .Select(spec => spec.IsSatisfiedBy(input))
                .Count() >= x));

        public static readonly Func<uint, AggregationType<TContext>> Max = new Func<uint, AggregationType<TContext>>(x => 
            new AggregationType<TContext>(3, (input, specs) => specs
                .Select(spec => spec.IsSatisfiedBy(input))
                .Count() <= x));

        private AggregationType(uint id, Func<TContext, IReadOnlyList<ISpec<TContext>>, bool> operation)
        {
            this.Id = id;
            this.Operation = operation;
        }

        public uint Id { get; }

        public Func<TContext, IReadOnlyList<ISpec<TContext>>, bool> Operation { get; }
    }
}
