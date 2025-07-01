using System.Linq.Expressions;
using System.Reflection;

namespace FinanceControl.Application.Utils
{
    public static class ExpressionHelper
    {
        public static object ConvertToType(Type targetType, string value)
        {
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            return underlyingType switch
            {
                { } when underlyingType == typeof(bool) => bool.Parse(value),
                { } when underlyingType == typeof(DateTime) => DateTime.Parse(value),
                { } when underlyingType == typeof(Guid) => Guid.Parse(value),
                { } when underlyingType.IsEnum => Enum.Parse(underlyingType, value),
                _ => Convert.ChangeType(value, underlyingType!)
            };
        }

        public static Expression CreateComparisonExpression(Expression left, Type propertyType, string filterValue)
        {
            var targetType = Nullable.GetUnderlyingType(propertyType) ?? propertyType;

            if (targetType == typeof(string))
            {
                var toLowerMethod = typeof(string).GetMethod("ToLower", Type.EmptyTypes);
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });

                var leftToLower = Expression.Call(left, toLowerMethod!);
                var rightToLower = Expression.Constant(filterValue.ToLower(), typeof(string));

                return Expression.Call(leftToLower, containsMethod!, rightToLower);
            }

            object parsedValue = ConvertToType(targetType, filterValue);
            Expression right = Expression.Constant(parsedValue, targetType);

            return Expression.Equal(left, Expression.Convert(right, left.Type));
        }

        public static Expression CreateNestedComparisonExpression(ParameterExpression parameter, string propertyPath, string filterValue)
        {
            var properties = propertyPath.Split('.', StringSplitOptions.RemoveEmptyEntries);

            Expression expression = parameter;

            foreach (var prop in properties)
            {
                var propertyInfo = expression.Type.GetProperty(prop, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    throw new InvalidOperationException($"Property '{prop}' not found on type '{expression.Type.Name}'");
                }

                expression = Expression.Property(expression, propertyInfo);
            }

            var propertyType = ((MemberExpression)expression).Type;
            return CreateComparisonExpression(expression, propertyType, filterValue);
        }
    }
}
