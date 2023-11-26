using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using MyExpressions.Visitor;

namespace MyExpressions.ExpressionContext;

public static class ExpressionParser
{
    private static readonly TreeExpressionVisitor s_treeExpressionVisitor = new();

    public static Delegate ToDelegate(string expression)
    {
        var parameters =
            expression.Split(new[] { '+', '-', '*', '/', '(', ')' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var parameter in parameters.Where(v => !double.TryParse(v, out _)))
        {
            ExpressionContext.Current.AddVariable(parameter);
        }

        return DynamicExpressionParser.ParseLambda(
            parameters.Select(v => Expression.Parameter(typeof(double), v.Trim())).ToArray(),
            null,
            expression).Compile();
    }

    public static IExpression ToExpression(string expression)
    {
        var parameters =
            expression.Split(new[] { '+', '-', '*', '/', '(', ')' },
                StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        foreach (var parameter in parameters.Where(v => !double.TryParse(v, out _)))
        {
            ExpressionContext.Current.AddVariable(parameter);
        }

        var lambdaExpression = DynamicExpressionParser.ParseLambda(
            ExpressionContext.Current.Variables.Select(v => Expression.Parameter(typeof(double), v.Trim())).ToArray(),
            null,
            expression);

        return s_treeExpressionVisitor.DisassembleExpression(lambdaExpression.Body);
    }
}