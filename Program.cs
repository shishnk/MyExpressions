using MyExpressions.ExpressionContext;

var a = new VariableExpression("a");
var b = new VariableExpression("b");

var expr = new MultiplyExpression(new AddExpression(a, b),
    new AddExpression(new UnaryExpression(a), (ConstantaExpression)2.0));

ExpressionContext.Current["a"] = 1.0;
ExpressionContext.Current["b"] = 2.0;
ExpressionContext.Current["c"] = 3.0;

Console.WriteLine(expr);
Console.WriteLine(expr.Interpret(ExpressionContext.Current));

Console.WriteLine();

var t = ExpressionParser.ToDelegate("a + c");
var p = t.DynamicInvoke(2.0, 3.0);
Console.WriteLine(p);

Console.WriteLine();

var reducedToMyExpression = ExpressionParser.ToExpression("(a + c) + (a - b) / a");
Console.WriteLine(reducedToMyExpression);
Console.WriteLine(reducedToMyExpression.Interpret(ExpressionContext.Current));

Console.WriteLine();

var expressionWithConstant = ExpressionParser.ToExpression("2.0");
Console.WriteLine(expressionWithConstant);
Console.WriteLine(expressionWithConstant.Interpret(ExpressionContext.Current));

Console.WriteLine();

var expressionWithVariable = ExpressionParser.ToExpression("a");
Console.WriteLine(expressionWithVariable);
Console.WriteLine(expressionWithVariable.Interpret(ExpressionContext.Current));