namespace HULK.CodeAnalysis.Syntax
{
    public sealed class CompilationUnitSyntax : SyntaxNode
    {
        public CompilationUnitSyntax(ExpressionSyntax expression, SyntaxToken endOfFileToken)
        {
            Expression = expression;
            EndOfFileToken = endOfFileToken;
        }

        public override SyntaxKind Kind => SyntaxKind.CompilationUnit;
        public ExpressionSyntax Expression { get; }
        public SyntaxToken EndOfFileToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Expression;
            yield return EndOfFileToken;
        
        }
    }
}
