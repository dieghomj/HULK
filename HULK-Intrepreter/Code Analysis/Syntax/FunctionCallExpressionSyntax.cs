
namespace HULK.CodeAnalysis.Syntax
{
    internal sealed class FunctionCallExpressionSyntax : ExpressionSyntax
    {
        public FunctionCallExpressionSyntax( SyntaxToken functionName, SyntaxToken openParenthesisToken, List<ExpressionSyntax> arguments, SyntaxToken closeParenthesisToken)
        {
            FunctionName = functionName;
            OpenParenthesisToken = openParenthesisToken;
            Arguments = arguments;
            CloseParenthesisToken = closeParenthesisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.FunctionCallExpression;

        public SyntaxToken FunctionName { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public List<ExpressionSyntax> Arguments { get; }
        public SyntaxToken CloseParenthesisToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return FunctionName;
            yield return OpenParenthesisToken;
            foreach (var argument in Arguments)
                yield return argument;
            yield return CloseParenthesisToken;
        }
    }
}