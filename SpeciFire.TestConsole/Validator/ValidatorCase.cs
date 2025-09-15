using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using SpeciFire.DependencyInjection;
using SpeciFire.Validator;

namespace SpeciFire.TestConsole.Validator;

public class ValidatorCase
{
    public static void Execute()
    {
        var provider = new ServiceCollection()
            .AddSpecValidator<string>((builder, provider) =>
            { 
                builder
                    .With<IsEvenLengthWordSpec>()
                    .With<IsUrlSpec>();
            })
            .BuildServiceProvider();

        var validator = provider.GetRequiredService<ISpecValidator<string>>();
        foreach (var value in Strings())
        {
            var result = validator
                .WithAggregationType(AggregationType<string>.All)
                .Validate(value);

            Console.WriteLine($"Value '{value}' was '{result}'.");
        }
    }

    private static IEnumerable<string> Strings()
    {
        yield return "http://test.test";
        yield return "http://tests.test";
        yield return " - he)llo( ";
    }
}
