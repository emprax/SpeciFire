using System.Threading.Tasks;

namespace SpeciFire.Rules
{
    public abstract class SyncRuleOperation<TInput, TContext> : IRuleOperation<TInput, TContext> where TContext : class
    {
        protected abstract void Apply(TInput input, TContext context);

        public Task Execute(TInput input, TContext context)
        {
            this.Apply(input, context);
            return Task.CompletedTask;
        }
    }
}
