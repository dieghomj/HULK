namespace HULK
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
        OpenParenthisisToken,
        CloseParenthisisToken,

        //Expressions
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthisizedExpression,
        NegativeNumberExpression,
    }
}