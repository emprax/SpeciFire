using System;
using System.Linq.Expressions;

namespace SpeciFire.TestConsole.Validator;

public class IsUrlSpec : Spec<string>
{
    public override Expression<Func<string, bool>> AsExpression() => x => Uri.IsWellFormedUriString(x, UriKind.RelativeOrAbsolute);
}
