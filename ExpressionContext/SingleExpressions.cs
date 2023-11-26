using System.Globalization;
using System.Linq.Expressions;

namespace MyExpressions.ExpressionContext;

public class VariableExpression(string name) : IExpression
{
    public string Name => name;
    public Expression Expression => Expression.Parameter(typeof(double), name);
    public double Interpret(ExpressionContext.IExpressionContext context) => context[name];

    public override string ToString() => name;

    public static IExpression operator +(VariableExpression left, VariableExpression right) =>
        new AddExpression(left, right);
}

public class ConstantaExpression(double value) : IExpression
{
    public string Name { get; } = value.ToString(CultureInfo.InvariantCulture);
    public Expression Expression => Expression.Constant(typeof(double));

    public double Interpret(ExpressionContext.IExpressionContext context) =>
        context[value.ToString(CultureInfo.InvariantCulture)];

    public override string ToString() => Name;

    public static implicit operator ConstantaExpression(double value) => new(value);
}

public class UnaryExpression(IExpression expression) : IExpression
{
    public string Name => $"-{expression.Name}";
    public Expression Expression => Expression.Negate(expression.Expression);

    public double Interpret(ExpressionContext.IExpressionContext context) => -expression.Interpret(context);

    public override string ToString() => Name;
}