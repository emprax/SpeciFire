using System;
using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator;

public class AggregationType<TContext>
{
    public static readonly AggregationType<TContext> All = new AggregationType<TContext>(1, (input, specs) => 
    { 
        var errors = new List<string>();

        specs.Where(spec => !spec.IsSatisfiedBy(input))
             .ToList()
             .ForEach(spec => errors.Add(spec.GetType().Name));

        return new ValidationResult(errors);
    });
    
    public static readonly AggregationType<TContext> Any = new AggregationType<TContext>(2, (input, specs) =>
    { 
        var errors = new List<string>();

        specs.Where(spec => !spec.IsSatisfiedBy(input))
             .ToList()
             .ForEach(spec => errors.Add(spec.GetType().Name));

        return new ValidationResult(errors, !specs.Any() || specs.Count != errors.Count);
    });
    
    public static readonly Func<uint, AggregationType<TContext>> AtLeast = new Func<uint, AggregationType<TContext>>(x => 
        new AggregationType<TContext>(3, (input, specs) =>
        {
            var errors = new List<string>();
            var result = specs
                .Where(spec =>
                {
                    if (!spec.IsSatisfiedBy(input))
                    {
                        errors.Add(spec.GetType().Name);
                        return false;
                    }

                    return true;
                })
                .Count() >= x;

            return new ValidationResult(errors, result);
        }));

    public static readonly Func<uint, AggregationType<TContext>> AtMost = new Func<uint, AggregationType<TContext>>(x => 
        new AggregationType<TContext>(4, (input, specs) =>
        {
            var errors = new List<string>();
            var result = specs
                .Select(spec =>
                {
                    if (!spec.IsSatisfiedBy(input))
                    {
                        errors.Add(spec.GetType().Name);
                        return false;
                    }

                    return true;
                })
                .Count() <= x;

            return new ValidationResult(errors, result);
        }));

    private AggregationType(uint id, Func<TContext, IReadOnlyList<ISpec<TContext>>, ValidationResult> operation)
    {
        this.Id = id;
        this.Operation = operation;
    }

    public uint Id { get; }

    public Func<TContext, IReadOnlyList<ISpec<TContext>>, ValidationResult> Operation { get; }
}
