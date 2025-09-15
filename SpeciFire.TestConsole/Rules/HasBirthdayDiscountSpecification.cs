using System;
using System.Linq.Expressions;

namespace SpeciFire.TestConsole.Rules;

public class HasBirthdayDiscountSpecification : Spec<Customer>
{
    public override Expression<Func<Customer, bool>> AsExpression() => c => c.Birthday == DateTime.Today;
}
