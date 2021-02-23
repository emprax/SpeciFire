using System;

namespace SpeciFire.Rules
{
    public class SyncLambdaRuleOperation<TInput, TContext> : SyncRuleOperation<TInput, TContext> where TContext : class
    {
        private readonly Action<TInput, TContext> lambda;

        public SyncLambdaRuleOperation(Action<TInput, TContext> lambda) => this.lambda = lambda;

        protected override void Apply(TInput input, TContext context) => this.lambda.Invoke(input, context);
    }
}
