using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis.Binding
{
    internal class BoundFunctionDeclarationExpression : BoundExpression
    {
        public BoundFunctionDeclarationExpression(SyntaxToken functionName, List<VariableSymbol> parameters)
        {
            FunctionName = functionName;
            Parameters = parameters;
        }

        public SyntaxToken FunctionName { get; }
        public List<VariableSymbol> Parameters { get; }
        public override Type Type => typeof(void);

        public override BoundNodeKind Kind => BoundNodeKind.FunctionDeclarationExpression;
    }
}