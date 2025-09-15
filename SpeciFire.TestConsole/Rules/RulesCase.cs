using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SpeciFire.DependencyInjection;
using SpeciFire.Rules;

namespace SpeciFire.TestConsole.Rules;

public class RulesCase
{
    public static async Task Execute()
    {
        var provider = new ServiceCollection()
            .AddRuleEngine<Customer, Result>((builder, provider) =>
            {
                builder
                    .AddRule<HasAccountDiscountSpecification, AccountDiscountOperation>()
                    .AddRule<HasBirthdayDiscountSpecification, BirthDayDiscountOperation>();
            })
            .BuildServiceProvider();

        var engine = provider.GetRequiredService<IRuleEngine<Customer, Result>>();
        foreach (var customer in Customers())
        {
            engine.Context = new Result(10.0m);
            await engine.Execute(customer);

            Console.WriteLine($"{customer.Name} eventually has to pay: {engine.Context.Price}.");
        }
    }

    private static IEnumerable<Customer> Customers()
    {
        yield return new Customer("Henk", DateTime.Today, false);
        yield return new Customer("Richard", DateTime.Today, true);
        yield return new Customer("George", DateTime.Today.AddDays(3).AddMonths(2), true);
    }
}
