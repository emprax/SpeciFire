using SpeciFire.Specifications;

namespace SpeciFire
{
    public static class SpecExtensions
    {
        public static ISpec<TContext> And<TContext>(this ISpec<TContext> left, ISpec<TContext> right) => new AndSpec<TContext>(left, right);

        public static ISpec<TContext> Or<TContext>(this ISpec<TContext> left, ISpec<TContext> right) => new OrSpec<TContext>(left, right);

        public static ISpec<TContext> Not<TContext>(this ISpec<TContext> spec) => new NotSpec<TContext>(spec);
    }
}
