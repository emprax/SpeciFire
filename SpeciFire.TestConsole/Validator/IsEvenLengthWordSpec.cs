using System;
using System.Linq.Expressions;

namespace SpeciFire.TestConsole.Validator
{
    public class IsEvenLengthWordSpec : Spec<string>
    {
        public override Expression<Func<string, bool>> AsExpression() => x => x.Length % 2 == 0;
    }
}
