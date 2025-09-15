using System.Threading.Tasks;

namespace SpeciFire.Rules;

public interface IRuleOperation<TInput, TContext> where TContext : class
{
    Task Execute(TInput input, TContext context);
}
