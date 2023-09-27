namespace HULK.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        //Tokens
        BadToken,
        EndOfFileToken,
        WitheSpaceToken,
        NumberToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivToken,
        BangToken,
        AmpersandToken,
        PipeToken,
        OpenParenthisisToken,
        CloseParenthisisToken,
        IdentifierToken,

        //KeyWords
        FalseKeyword,
        TrueKeyword,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthisizedExpression,
        NegativeNumberExpression,

    }
}