using SpeciFire.Rules;

namespace SpeciFire.TestConsole.Rules
{
    public class BirthDayDiscountOperation : SyncRuleOperation<Customer, Result>
    {
        protected override void Apply(Customer input, Result context) => context.Price -= 2.5m;
    }
}
