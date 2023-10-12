namespace HULK.CodeAnalysis.Syntax
{
    internal sealed class IfElseExpressionSyntax : ExpressionSyntax
    {
        public IfElseExpressionSyntax( SyntaxToken ifToken, SyntaxToken openParenthesisToken, ExpressionSyntax condition, SyntaxToken closeParenthesisToken, ExpressionSyntax trueExpression, SyntaxToken elseToken, ExpressionSyntax falseExpression )
        {
            IfToken = ifToken;
            OpenParenthesisToken = openParenthesisToken;
            Condition = condition;
            CloseParenthesisToken = closeParenthesisToken;
            TrueExpression = trueExpression;
            ElseToken = elseToken;
            FalseExpression = falseExpression;
        }

        public SyntaxToken IfToken { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public ExpressionSyntax Condition { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public ExpressionSyntax TrueExpression { get; }
        public SyntaxToken ElseToken { get; }
        public ExpressionSyntax FalseExpression { get; }

        public override SyntaxKind Kind => SyntaxKind.IfElseExpression;

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return IfToken;
            yield return Condition;
            yield return TrueExpression;
            yield return ElseToken;
            yield return FalseExpression;
        
        }
    }

}