namespace HULK.CodeAnalysis.Syntax
{
    internal class PredefinedFunctionExpressionSyntax : ExpressionSyntax
    {
        public PredefinedFunctionExpressionSyntax(SyntaxToken function, SyntaxToken openParenthesisToken, List<ExpressionSyntax> arguments, SyntaxToken closedParenthesisToken)
        {
            Function = function;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
            ClosedParenthesisToken = closedParenthesisToken;
        }


        public SyntaxToken Function { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public List<ExpressionSyntax> Arguments { get; }
        public SyntaxToken ClosedParenthesisToken { get; }

        public override SyntaxKind Kind => SyntaxKind.PredefinedFunctionExpression;
        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Function;
            yield return OpenParenthesisToken;
            foreach (var a in Arguments)
                yield return a;
            yield return ClosedParenthesisToken;
        
        }
    }
}

