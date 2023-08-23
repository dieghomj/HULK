namespace HULK
{
    public enum SyntaxKind
    {
        //SpecialTokens
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