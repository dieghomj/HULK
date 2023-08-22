namespace HULK
{
    sealed class NegativeNumberExpressionSyntax : ExpressionSyntax
    {
        public NegativeNumberExpressionSyntax(SyntaxToken operatorToken, ExpressionSyntax expression)
        {
            OperatorToken = operatorToken;
            Expression = expression;
            // NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NegativeNumberExpression;

        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Expression { get; }
        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return OperatorToken ;
            yield return Expression ;
        }
    }
}