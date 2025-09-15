using System.Collections.Generic;
using System.Linq;

namespace SpeciFire.Validator;

public class ValidationResult
{
    public ValidationResult(IList<string> errors) : this(errors, !(errors?.Any() ?? true)) {}

    public ValidationResult(IList<string> errors, bool isValid)
    {
        this.Errors = (errors?.ToList() ?? new List<string>()).AsReadOnly();
        this.IsValid = isValid;
    }

    public IReadOnlyList<string> Errors { get; }

    public bool IsValid { get; }

    public override string ToString()
    {
        if (this.IsValid)
        { 
            return "Valid.";
        }
        
        var result = (this.Errors?.Any() ?? false) 
            ? this.Errors.Aggregate((a, b) => $"{a}, {b}")
            : "...";

        return $"Invalid. Errors found in: ({result}).";
    }
}
