using System;
using System.Linq.Expressions;

namespace SpeciFire.TestConsole.Rules;

public class HasAccountDiscountSpecification : Spec<Customer>
{
    public override Expression<Func<Customer, bool>> AsExpression() => c => c.HasAccount;
}
