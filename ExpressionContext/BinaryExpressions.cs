using System.Linq.Expressions;

namespace MyExpressions.ExpressionContext;

public abstract class BinaryExpression(IExpression left, IExpression right) : IExpression
{
    protected IExpression Left { get; } = left;
    protected IExpression Right { get; } = right;
    public abstract string Name { get; }

    public abstract Expression Expression { get; }

    public abstract double Interpret(ExpressionContext.IExpressionContext context);
}

public class AddExpression(IExpression left, IExpression right) : BinaryExpression(left, right)
{
    public override string Name { get; } = $"{left.Name} + {right.Name}";
    public override Expression Expression => Expression.Add(left.Expression, right.Expression);

    public override double Interpret(ExpressionContext.IExpressionContext context) =>
        left.Interpret(context) + right.Interpret(context);

    public override string ToString() => Name;
}

public class SubtractExpression(IExpression left, IExpression right) : BinaryExpression(left, right)
{
    public override string Name { get; } = $"{left.Name} - {right.Name}";
    public override Expression Expression => Expression.Subtract(left.Expression, right.Expression);

    public override double Interpret(ExpressionContext.IExpressionContext context) =>
        left.Interpret(context) - right.Interpret(context);

    public override string ToString() => Name;
}

public class MultiplyExpression(IExpression left, IExpression right) : BinaryExpression(left, right)
{
    public override string Name { get; } = $"{left.Name} * {right.Name}";
    public override Expression Expression => Expression.Multiply(left.Expression, right.Expression);

    public override double Interpret(ExpressionContext.IExpressionContext context) =>
        left.Interpret(context) * right.Interpret(context);

    public override string ToString() => Name;
}

public class DivideExpression(IExpression left, IExpression right) : BinaryExpression(left, right)
{
    public override string Name { get; } = $"{left.Name} / {right.Name}";
    public override Expression Expression => Expression.Divide(left.Expression, right.Expression);

    public override double Interpret(ExpressionContext.IExpressionContext context) =>
        left.Interpret(context) / right.Interpret(context);

    public override string ToString() => Name;
}