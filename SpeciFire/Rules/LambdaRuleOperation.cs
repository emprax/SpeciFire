using System;
using System.Threading.Tasks;

namespace SpeciFire.Rules;

public class LambdaRuleOperation<TInput, TContext> : IRuleOperation<TInput, TContext> where TContext : class
{
    private readonly Func<TInput, TContext, Task> lambda;

    public LambdaRuleOperation(Func<TInput, TContext, Task> lambda) => this.lambda = lambda;

    public Task Execute(TInput input, TContext context) => this.lambda.Invoke(input, context);
}
