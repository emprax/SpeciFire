# <img src=".\docs\SpeciFire(logo).png" width="120" height="134" /> SpeciFire

![Nuget](https://img.shields.io/nuget/v/SpeciFire?color=green&style=plastic)

NuGet package pages:

- [SpeciFire](https://www.nuget.org/packages/SpeciFire/)
- [SpeciFire.DependencyInjection](https://www.nuget.org/packages/SpeciFire.DependencyInjection/)

## Introduction & Background

SpeciFire is a specification pattern library, defined like already established approaches, but extended with other fitting functionalities. A few sources, which have been directly applied, or served as inspiration are:

- Specification pattern:
  -  https://en.wikipedia.org/wiki/Specification_pattern
  - https://enterprisecraftsmanship.com/posts/specification-pattern-c-implementation/
  - https://ademcatamak.medium.com/specification-design-pattern-c814649be0ef
  - https://www.martinfowler.com/apsupp/spec.pdf
  - https://www.pluralsight.com/courses/csharp-specification-pattern
- Rules pattern:
  - https://www.michael-whelan.net/rules-design-pattern/
  - https://tenmilesquare.com/basic-rules-engine-design-pattern/
  - https://yiniski.medium.com/rule-engine-pattern-8a3f0e0c2d81
  - https://www.pluralsight.com/courses/c-sharp-design-patterns-rules-pattern

This 3-in-1 patterns library provides the following three patterns:

- **Specification pattern:** Defines specifications as business-rules or queryable-rules which can be chained together using boolean logic. It provides highly reusable specifications that can also be transformed to expressions. This helps to define the predicates for queries utilized when using ORM technologies, such as Entity Framework, for querying a database.
- **Rules pattern:** Defines a set of business-rules that are stored together in a rule-engine, by the means of a list. This collection of rules forms a cohesive determination for a certain assignment. An individual rule consists of 2 parts: an operation that can be applied, and a specification to determine whether such an operation should occur. Note that the specification is not mandatory.
- **Validation pattern:** This is not a specifically defined pattern, but it is nonetheless one of the structures provided by this library. It is a simpler form of the rules pattern as it only contains a list of specifications. Fire a validation-operation will run through all specifications and determine whether the input conforms to the provided specifications.

## Coding Examples

The following code samples will provide and insight in how to utilize and extend certain functionality that is made available by the library.

### Specifications

The specifications can be written in different styles and can be combined/chained to extend the expression logic. The comments provide some specific details that can answer some fundamental questions.

```c#
// Note that the Spec<T> abstraction is used instead of the ISpec<T> interface, because in most cases the IsSatisfiedBy() method will just simply compile and execute the expression retrieved from the AsExpression() method.
public class CrossedThresholdSpecification : Spec<float>
{
    private readonly float threshold;
    
    public CrossedThresholdSpecification(float threshold) => this.threshold = threshold;
    
    // Note: (value => value > this.threshold) is an expression case, so the notation below might seem a bit of. 
    // Howeverthe the short-hand version of a one-line method is combined with the expression notation.
    public override Expression<Func<float, bool>> AsExpression() => value => value > this.threshold;
}


// Notice that here only the CrossedThresholdSpecification<T> specification, defined above, is only being used, though, it depends on the developer to add more expressions. The And(ISpec<T>, ISpec<T>), Or(ISpec<T>, ISpec<T>) and Not(ISpec<T>) helpers (there are also the same extension-methods for ISpec<T> itself) are there to help chaining the specifications together to form a larging specification. These helper methods are implemented as AndSpec<T>, OrSpec<T> and NotSpec<T> respectively and are implementations of the so-called BinarySpec<T> and UnarySpec<T> specifications (These are also available in the library to create new binary and unary specitications).
var chain = Spec.And(
    new CrossedThresholdSpecification<float>(4.0f), 
    Spec.Or(
        new CrossedThresholdSpecification<float>(8.0f),
    	new CrossedThresholdSpecification<float>(5.0f)));

if (chain.IsSatisfiedBy(6.0f))
{
    Console.WriteLine("Success!!");
}
```

Of course, it is also one of the best choices for queries in repositories.

```c#
// It is a very good substitude for queries fired at a database or other datastores.
var entityRepository = new EfRepository<User>(dbContext);   // implementation of IRepository<User>
var cosmosRepository = new CosmosRepository<User>(collection);   // implementation of IRepository<User>

// Can be used for both cases.
var query = new UserByNameSpec<User>("John");

entityRepository.ListAsync(query);
cosmosRepository.ListAsync(query);
```

### Rules

The rules-engines can be defined by a specific dependency injection extension. This extension utilizes a builder-factory construction to define the chain links which represent the rules themselves.

```c#
// Note that the context always needs to be a reference type because of reference behavior used in this library.
public class CrossedThresholdOperation : IRuleOperation<float, Record>
{
    private readonly Logger logger;
    
    public CrossedThresholdOperation(Logger logger) => this.logger = logger;
    
    public Task Execute(float input, Record context) 
    {
        context.Total += input;
        
        this.logger.Info(
            "Total amount has been increased by verified value of '{input}'. New total: {total}.",
        	input,
        	context.Total);
        
        return Task.CompletedTask; 
        // It is also possible to use the synchronous SyncRuleOperation<TInput, TContext> and the lambda version of it.
    }
}


// The provisioner is available as internal IServiceProvider to resolve dependencies. The builder has to be provided with the rule(-component)s. Building the objects is handled internally.
var provider = new ServiceCollection()
    .AddRuleEngine<float, Record>((builder, provisioner) =>
    {
        builder
            // By type arguments. Note: DI now has to know the threshold. Possible solution: IOptions<T> or object/provider.
            .AddRule<CrossedThresholdSpecification, CrossedThresholdOperation>()
            // By type argument for operation and expression for specification. Expression will be stored in ExpressionSpec<T>.
            .AddRule<CrossedThresholdOperation>(value => value > 10.0f)
            // By type arguments. Note: DI now has to know the threshold. Possible solution: IOptions<T> or object/provider.
            .AddRule(typeof(CrossedThresholdSpecification), typeof(CrossedThresholdOperation))
	    // By fully implemented operation + specification.            
            .AddRule(new CrossedThresholdSpecification(10.0f), new CrossedThresholdOperation(new Logger()))
	    // By implementation of operation and expression for specification. Expression will be stored in ExpressionSpec<T>.
            .AddRule(value => value > 10.0f, new CrossedThresholdOperation(new Logger()))
	    // By type value for operation and expression for specification. Expression will be stored in ExpressionSpec<T>.
            .AddRule(value => value > 10.0f, typeof(CrossedThresholdOperation))
            // As above, however, these rules will not implement a specification and will therefore be executed nonetheless.
            .AddRuleWithoutPredicate<CrossedThresholdOperation>()
            .AddRuleWithoutPredicate(typeof(CrossedThresholdOperation))
            .AddRuleWithoutPredicate(new CrossedThresholdOperation())
	    // As above, however, definitions for full rules. 
            .AddRule<CrossedThresholdRule>()
            .AddRule(typeof(CrossedThresholdRule))
            .AddRule(new Rule<float, Record>(new CrossedThresholdSpecification(10.0f), new CrossedThresholdOperation()));
    })
    .BuildServiceProvider(); 


var ruleEngine = provider.GetRequiredService<IRuleEngine<float, Record>>();
ruleEngine.Context = new Record();

await ruleEngine.Execute(15.0f);
Console.WriteLine("Total: {total}.", ruleEngine.Context.Total);
```

### Validator

The validator does apply similar approach as the rule-engine. However, the amount of different methods to register is considerably less in comparison to the rule-engine approach. Nevertheless, there is the possibility to state which aggregation strategy the validator has to apply. By default this is the All-type, which ensures that all the given specifications has to pass to determine the validation to be a success.

```c#
// The provisioner is available as internal IServiceProvider to resolve dependencies. The builder has to be provided with the specifications. Building the objects is handled internally.
var provider = new ServiceCollection()
    .AddSpecValidator<float>((builder, provisioner) =>
    {
        builder
            // By type argument. Note: DI now has to know the threshold. Possible solution: IOptions<T> or object/provider.
            .With<CrossedThresholdSpecification>()
            // By object initialization.
            .With(new CrossedThresholdSpecification(10.0f))
            // By type value. Note: DI now has to know the threshold. Possible solution: IOptions<T> or object/provider.
            .With(typeof(CrossedThresholdSpecification));
    })
    .BuildServiceProvider();



var validator = provider.GetRequiredService<ISpecValidator<float>>();

// The default validation aggregation is the All-type.
var result1 = validator.Validate(6.0f);

// This is how to define a different validation aggregation-type for this validator.
// This is the Any-type aggregation which determines that there is at least one specification that passes the validation successfully.
var result2 = validator
    .WithAggregationType(AggregationType<float>.Any)
    .Validate(12.0f);

// This is the AtLeast-type aggregation which determines that there must be at least a certain number of specifications that should be pass the validation successfully. However, the twist here is that a number should be provided to determine the minimal amount of specification that should be passed successfully.
var result3 = validator
    .WithAggregationType(AggregationType<float>.AtLeast(3))
    .Validate(12.0f);

// This is the AtMost-type aggregation which determines that there can only at most be a certain number of specifications that should be pass the validation successfully. However, the twist here is that a number should be provided to determine the maximum amount of specification that should be passed successfully.
var result3 = validator
    .WithAggregationType(AggregationType<float>.AtMost(3))
    .Validate(12.0f);



// The results each provide a ValidationResult.
// Outputs:  'Validation: Invalid. Errors found in (CrossedThresholdSpecification).'
// When more specs fail:  'Validation: Invalid. Errors found in (CrossedThresholdSpecification, OtherSpec, YetAnotherSpec).'
Console.Writeline($"Validation: '{result1}''");  

// On success:
Console.Writeline($"Validation: '{result2}''");  // Outputs:  'Validation: Valid.'
```



