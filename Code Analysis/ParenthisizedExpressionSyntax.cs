namespace HULK
{
    public sealed class ParenthisizedExpressionSyntax : ExpressionSyntax
    {
        public ParenthisizedExpressionSyntax( SyntaxToken openParenthisisToken, ExpressionSyntax expression, SyntaxToken closedParenthisisToken)
        {
            OpenParenthisisToken = openParenthisisToken;
            Expression = expression;
            ClosedParenthisisToken = closedParenthisisToken;
        }

        public override SyntaxKind Kind => SyntaxKind.ParenthisizedExpression;

        public SyntaxToken OpenParenthisisToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken ClosedParenthisisToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OpenParenthisisToken;
            yield return Expression;
            yield return ClosedParenthisisToken;
        }
    }
}