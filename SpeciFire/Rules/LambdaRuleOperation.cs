using System;

namespace SpeciFire.Rules
{
    public class LambdaRuleOperation<TContext, TResult> : IRuleOperation<TContext, TResult>
    {
        private readonly Func<TContext, TResult> lambda;

        public LambdaRuleOperation(Func<TContext, TResult> lambda) => this.lambda = lambda;

        public TResult Execute(TContext context) => this.lambda.Invoke(context);
    }
}
