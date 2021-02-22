using SpeciFire.Rules;

namespace SpeciFire.TestConsole.Rules
{
    public class AccountDiscountOperation : SyncRuleOperation<Customer, Result>
    {
        protected override void Apply(Customer input, Result context) => context.Price -= 5.0m;
    }
}
