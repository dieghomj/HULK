namespace HULK.CodeAnalysis.Syntax
{
    internal sealed class LetInExpressionSyntax : ExpressionSyntax
    {
        public LetInExpressionSyntax(SyntaxToken letToken, List<ExpressionSyntax> assignments, SyntaxToken inToken, ExpressionSyntax expression)
        {
            LetToken = letToken;
            Assignments = assignments;
            InToken = inToken;
            Expression = expression;
        }

        public override SyntaxKind Kind => SyntaxKind.LetInExpression;

        public SyntaxToken LetToken { get; }
        public IEnumerable<ExpressionSyntax> Assignments { get; }
        public SyntaxToken InToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return LetToken;

            foreach (var assignment in Assignments)
                yield return assignment;

            yield return InToken;
            yield return Expression;
        }

    }    

}