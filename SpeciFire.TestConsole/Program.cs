using System;
using System.Threading.Tasks;
using SpeciFire.TestConsole.Rules;
using SpeciFire.TestConsole.Validator;

namespace SpeciFire.TestConsole
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await RulesCase.Execute();

            Console.WriteLine();
            ValidatorCase.Execute();
        }
    }
}
