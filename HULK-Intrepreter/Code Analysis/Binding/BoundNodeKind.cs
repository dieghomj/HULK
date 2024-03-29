namespace HULK.CodeAnalysis.Binding
{
    internal enum BoundNodeKind
    {
        LiteralExpression,
        UnaryExpression,
        BinaryExpression,
        VariableExpression,
        AssignmentExpression,
        LetInExpression,
        IfElseExpression,
        FunctionCallExpression,
        FunctionDeclarationExpression,
        PredefinedFunction
    }

}