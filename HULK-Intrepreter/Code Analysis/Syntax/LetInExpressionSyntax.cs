namespace HULK.CodeAnalysis.Syntax
{
    internal sealed class LetInExpressionSyntax : ExpressionSyntax
    {
        public LetInExpressionSyntax(SyntaxToken letToken, ExpressionSyntax assignment, SyntaxToken inToken, ExpressionSyntax expression)
        {
            LetToken = letToken;
            Assignment = assignment;
            InToken = inToken;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.LetInExpression;

        public SyntaxToken LetToken { get; }
        public ExpressionSyntax Assignment { get; }
        public SyntaxToken InToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LetToken;
            yield return Assignment;
            yield return InToken;
            yield return Expression;
        }

    }    

}