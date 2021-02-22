using System;
using System.Threading.Tasks;

namespace SpeciFire.Rules
{
    public class SyncLambdaRuleOperation<TInput, TContext> : IRuleOperation<TInput, TContext> where TContext : class
    {
        private readonly Action<TInput, TContext> lambda;

        public SyncLambdaRuleOperation(Action<TInput, TContext> lambda) => this.lambda = lambda;

        public Task Execute(TInput input, TContext context) 
        { 
            this.lambda.Invoke(input, context);
            return Task.CompletedTask;
        }
    }
}
