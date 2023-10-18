using HULK.CodeAnalysis.Binding;
using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis
{
    public sealed class Compilation
    {
        public Compilation(SyntaxTree syntax)
        {
            Syntax = syntax;
        }

        public SyntaxTree Syntax { get; private set; }
        public EvaluationResult Evaluate(Dictionary<VariableSymbol, object> variables, Dictionary<FunctionSymbol, object> functions)
        {
            var binder = new Binder(variables, functions);
            var boundExpression = binder.BindExpression(Syntax.Root.Expression);
            
            var diagnostics = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if(diagnostics.Any())
                return new EvaluationResult(diagnostics,null);

            var evaluator = new Evaluator(boundExpression, functions);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<Diagnostic>(), value);
        }
    }
}

