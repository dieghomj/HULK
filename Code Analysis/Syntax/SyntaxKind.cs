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
        EqualEqualToken,
        BangEqualToken,
        OpenParenthisisToken,
        CloseParenthisisToken,
        IdentifierToken,

        //KeyWords
        FalseKeyword,
        TrueKeyword,

        //Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssignmentExpression,
        EqualsToken,
    }
}