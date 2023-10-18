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
        PercentToken,
        CircumflexToken,
        LessEqualToken,
        LessToken,
        GreaterToken,
        GreaterEqualToken,
        AtToken,
        EqualsToken,
        BangToken,
        AmpersandToken,
        PipeToken,
        EqualEqualToken,
        BangEqualToken,
        OpenParenthesisToken,
        CloseParenthesisToken,
        CommaToken,


        //KeyWords
        FalseKeyword,
        TrueKeyword,
        InKeyword,
        LetKeyword,
        IfKeyword,
        ElseKeyword,
        FunctionKeyword,

        //Nodes
        CompilationUnit,


        //Expressions
        LiteralExpression,
        NameExpression,
        UnaryExpression,
        BinaryExpression,
        ParenthesizedExpression,
        AssignmentExpression,
        LetInExpression,
        IfElseExpression,
        FunctionCallExpression,
        FunctionDeclarationExpression,
        PredefinedFunctionKeyword,
        ConstantKeyword,
        PredefinedFunctionExpression,
    }
}