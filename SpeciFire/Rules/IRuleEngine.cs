using System.Threading.Tasks;

namespace SpeciFire.Rules
{
    public interface IRuleEngine<TInput, TContext> where TContext : class
    {
        TContext Context { get; set; }

        Task Execute(TInput input);
    }
}
