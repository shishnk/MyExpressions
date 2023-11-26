using System.Linq.Expressions;

namespace MyExpressions.ExpressionContext;

public interface IExpression
{
    string Name { get; }
    Expression Expression { get; }

    double Interpret(ExpressionContext.IExpressionContext context);
}

public class Expr(IExpression expr) : IExpression
{
    public string Name { get; } = expr.Name;
    public Expression Expression => expr.Expression;

    public double Interpret(ExpressionContext.IExpressionContext context) => expr.Interpret(context);

    public override string ToString() => Name;
}