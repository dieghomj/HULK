
namespace HULK.CodeAnalysis.Syntax
{
    internal sealed class FunctionDeclarationExpressionSyntax : ExpressionSyntax
    {
        public FunctionDeclarationExpressionSyntax(SyntaxToken functionToken, SyntaxToken functionName, SyntaxToken openParenthesisToken,List<SyntaxToken> parameters, SyntaxToken closeParenthesisToken, SyntaxToken equalToken, SyntaxToken greaterToken, ExpressionSyntax expression)
        {
            FunctionToken = functionToken;
            FunctionName = functionName;
            OpenParenthesisToken = openParenthesisToken;
            Parameters = parameters;
            CloseParenthesisToken = closeParenthesisToken;
            EqualToken = equalToken;
            GreaterToken = greaterToken;
            Expression = expression;
        }
        public override SyntaxKind Kind => SyntaxKind.FunctionDeclarationExpression;

        public SyntaxToken FunctionToken { get; }
        public SyntaxToken FunctionName { get; }
        public SyntaxToken OpenParenthesisToken { get; }
        public List<SyntaxToken> Parameters { get; }
        public SyntaxToken CloseParenthesisToken { get; }
        public SyntaxToken EqualToken { get; }
        public SyntaxToken GreaterToken { get; }
        public ExpressionSyntax Expression { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return FunctionToken;
            yield return FunctionName;  
            yield return OpenParenthesisToken;
            foreach (var parameter in Parameters)
                yield return parameter;
            yield return CloseParenthesisToken; 
            yield return Expression;
        
        }
    }
}