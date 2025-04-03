namespace FinanceControl.API.Extensions
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ValidationContextAttribute : Attribute
    {
        public Type ValidatorType { get; }

        public ValidationContextAttribute(Type validatorType)
        {
            ValidatorType = validatorType ?? throw new ArgumentNullException(nameof(validatorType));
        }
    }
}
