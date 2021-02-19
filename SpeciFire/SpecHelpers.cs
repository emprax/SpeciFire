using SpeciFire.Specifications;

namespace SpeciFire
{
    public static class Spec
    {
        public static ISpec<TContext> And<TContext>(ISpec<TContext> left, ISpec<TContext> right) => new AndSpec<TContext>(left, right);

        public static ISpec<TContext> Or<TContext>(ISpec<TContext> left, ISpec<TContext> right) => new OrSpec<TContext>(left, right);

        public static ISpec<TContext> Not<TContext>(ISpec<TContext> spec) => new NotSpec<TContext>(spec);
    }
}
