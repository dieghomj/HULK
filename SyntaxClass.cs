namespace HULK
{
    public enum SyntaxKind
    {
        NumberToken,
        WitheSpaceToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivToken,
        OpenParenthisisToken,
        CloseParenthisisToken,
        BadToken,
        EndOfFileToken,
        NumberExpression,
        BinaryExpression,
        ParenthisizedExpression
    }

    public abstract class SyntaxNode
    {

        public abstract SyntaxKind Kind { get; }

        public abstract IEnumerable<SyntaxNode> GetChildren();

    }

    public abstract class ExpressionSyntax: SyntaxNode
    {

    }

    public sealed class SyntaxTree
    {
        public SyntaxTree( IEnumerable<string> diagnostics,ExpressionSyntax root, SyntaxToken endOfFileToken)
        {
            Diagnostics = diagnostics.ToArray();
            Root = root;
            EndOfFileToken = endOfFileToken;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public ExpressionSyntax Root { get; }
        public SyntaxToken EndOfFileToken { get; }

        public static SyntaxTree Parse(string text){
            var parser = new Parser(text);
            return parser.Parse();
        }

    }

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax( SyntaxToken numberToken)
        {
            NumberToken = numberToken;
        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;

        public SyntaxToken NumberToken { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return  NumberToken;
        }
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxToken operatorToken, ExpressionSyntax right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public ExpressionSyntax Left { get; }
        public SyntaxToken OperatorToken { get; }
        public ExpressionSyntax Right { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            yield return Left;
            yield return OperatorToken;
            yield return Right;
        }
    }

    sealed class ParenthisizedExpressionSyntax : ExpressionSyntax
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

    public class SyntaxToken : SyntaxNode
    {
        public SyntaxToken(SyntaxKind kind, int position, string text, object value)
        {
            Kind = kind;
            Position = position;
            Text = text;
            Value = value;
        }

        public override SyntaxKind Kind { get; }

        public int Position { get; }
        public string Text { get; }
        public object Value { get; }

        public override IEnumerable<SyntaxNode> GetChildren()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}