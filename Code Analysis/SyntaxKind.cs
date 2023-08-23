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
        BinaryExpression,
        ParenthisizedExpression,
        NegativeNumberExpression
    }
}