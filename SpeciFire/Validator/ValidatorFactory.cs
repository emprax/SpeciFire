using System.Collections.Generic;

namespace SpeciFire.Validator
{
    public class ValidatorFactory<TContext> : IValidatorFactory<TContext>
    {
        private List<ISpec<TContext>> specifications;

        public ValidatorFactory() => this.specifications = new List<ISpec<TContext>>();

        public IValidator<TContext> Build()
        {
            var validator = new Validator<TContext>(this.specifications.AsReadOnly());
            this.specifications = new List<ISpec<TContext>>();

            return validator;
        }

        public IValidatorFactory<TContext> With(ISpec<TContext> spec)
        {
            this.specifications.Add(spec);
            return this;
        }
    }
}
