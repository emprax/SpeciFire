# <img src=".\docs\SpeciFire(logo).png" style="zoom:13%;" /> SpeciFire

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

