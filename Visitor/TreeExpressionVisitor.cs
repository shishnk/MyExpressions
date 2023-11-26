using System.Linq.Expressions;
using MyExpressions.ExpressionContext;
using BinaryExpression = System.Linq.Expressions.BinaryExpression;

namespace MyExpressions.Visitor;

public class TreeExpressionVisitor : ExpressionVisitor
{
    private IExpression _collectedExpression = null!;

    public IExpression DisassembleExpression(Expression expression)
    {
        Visit(expression);
        return _collectedExpression;
    }

    protected override Expression VisitBinary(BinaryExpression node)
    {
        // ReSharper disable once SwitchExpressionHandlesSomeKnownEnumValuesWithExceptionInDefault
        var expr = new Expr(node.NodeType switch
        {
            ExpressionType.Add => new AddExpression(InnerVisit(node.Left), InnerVisit(node.Right)),
            ExpressionType.Subtract => new SubtractExpression(InnerVisit(node.Left), InnerVisit(node.Right)),
            ExpressionType.Multiply => new MultiplyExpression(InnerVisit(node.Left), InnerVisit(node.Right)),
            ExpressionType.Divide => new DivideExpression(InnerVisit(node.Left), InnerVisit(node.Right)),
            _ => throw new NotImplementedException($"Node not processed yet: {node.NodeType}"),
        });

        _collectedExpression = new Expr(expr);

        return node;
    }

    protected override Expression VisitConstant(ConstantExpression node)
    {
        _collectedExpression = new ConstantaExpression(node.Value as double? ?? 0.0);
        return node;
    }

    protected override Expression VisitParameter(ParameterExpression node)
    {
        _collectedExpression = new VariableExpression(node.Name ?? string.Empty);
        return node;
    }

    private IExpression InnerVisit(Expression node)
    {
        return new Expr(node switch
        {
            ConstantExpression constantExpression =>
                new ConstantaExpression(constantExpression.Value as double? ?? 0.0),
            ParameterExpression parameterExpression => new VariableExpression(parameterExpression.Name ?? string.Empty),
            BinaryExpression binaryExpression => DisassembleExpression(binaryExpression),
            _ => throw new NotImplementedException($"Node not processed yet: {node.NodeType}"),
        });
    }
}