namespace HULK.CodeAnalysis.Syntax
{
    public enum SyntaxKind
    {
        //Tokens
        BadToken,
        EndOfFileToken,
        WitheSpaceToken,
        IdentifierToken,
        NumberToken,
        StringToken,
        PlusToken,
        MinusToken,
        StarToken,
        DivToken,
        EqualsToken,
        BangToken,
        AmpersandToken,
        PipeToken,
        EqualEqualToken,
        BangEqualToken,
        OpenParenthesisToken,
        CloseParenthesisToken,

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

    }
}