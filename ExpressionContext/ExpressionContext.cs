namespace MyExpressions.ExpressionContext;

public abstract class ExpressionContext
{
    private static readonly InnerExpressionContext s_innerExpressionContext = new();

    public static IExpressionContext Current => s_innerExpressionContext;

    public interface IExpressionContext
    {
        double this[string name] { get; set; }

        IEnumerable<string> Variables { get; }

        void AddVariable(string parameter);
    }

    private class InnerExpressionContext : IExpressionContext
    {
        private readonly Dictionary<string, double> _variables = new();
        private readonly List<string> _variablesNames = new();

        public IEnumerable<string> Variables => _variablesNames.Select(v => v.Trim()).Distinct();

        public double this[string name]
        {
            get
            {
                if (_variables.TryGetValue(name, out var item)) return item;
                return double.TryParse(name, out var value) ? value : _variables[name];
            }
            set
            {
                if (_variables.TryGetValue(name, out var variable))
                {
                    if (Math.Abs(variable - value) < double.Epsilon)
                        return;
                }

                _variablesNames.Add(name);
                _variables.Remove(name);
                _variables.TryAdd(name, value);
            }
        }

        public void AddVariable(string parameter) => _variablesNames.Add(parameter);
    }
}