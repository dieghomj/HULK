using System.Collections;
using HULK.CodeAnalysis;
using HULK.CodeAnalysis.Syntax;

internal sealed class DiagnosticBag : IEnumerable<Diagnostic>
{
    private readonly List<Diagnostic> _diagnostics = new List<Diagnostic>();
    public IEnumerator<Diagnostic> GetEnumerator() => _diagnostics.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void AddRange(DiagnosticBag diagnostics)
    {
        _diagnostics.AddRange(diagnostics._diagnostics);
    }
    private void Report( TextSpan span, string message)
    {
        var diagnostic = new Diagnostic(span,message);
        _diagnostics.Add(diagnostic);
    }

    public void ReportInvalidNumber(TextSpan span, string text, Type type)
    {
        var message = $"! LEXICAL ERROR: The number {text} isn't valid {type}";
        Report(span,message);
    }

    public void ReportBadCharacter(int position, char character)
    {
        var span = new TextSpan(position, 1);
        var message = $"! LEXICAL ERROR: Bad character input: '{character}'";
        Report(span, message);
    }

    public void ReportUnexpectedToken(TextSpan span, SyntaxKind actualKind, SyntaxKind expectedKind)
    {
        var message = $"! SYNTAX ERROR: Unexpected token <{actualKind}> expected <{expectedKind}>";
        Report(span,message);
    }

    public void ReportUndefinedUnaryOperator(TextSpan span, string operatorText, Type operandType)
    {
        var message = $"! SEMANTIC ERROR: Unary operator '{operatorText}' is not defined for type {operandType}";
        Report(span,message);
    }

    public void ReportUndefinedBinaryOperator(TextSpan span, string operatorText, Type leftType, Type rightType)
    {
        var message = $"! SEMANTIC ERROR: Binary operator '{operatorText}' is not defined for types '{leftType}' and '{rightType}'";
        Report(span,message);
    }

    internal void ReportUndefinedName(TextSpan span, string name)
    {
        var message = $"! SEMANTIC ERROR: Variable '{name}' doesn't exist";
        Report(span,message);
    }

    internal void ReportExpectedCharacter(TextSpan span, char expectedCharacter)
    {
        var message = $"! LEXICAL ERROR: Expected '{expectedCharacter}'";
        Report(span,message);
    }

    internal void ReportUnexpectedType(TextSpan startSpan, TextSpan endSpan, Type actualType, Type expectedType)
    {

        var message = $"! SEMANTIC ERROR: Expected type '{expectedType.Name}' but got '{actualType.Name}'";
        Report(new TextSpan(startSpan.Start,endSpan.End-startSpan.Start),message);
    }

    internal void ReportUndefinedFunction(TextSpan span, string functionName, int count)
    {
        var message = $"! SEMANTIC ERROR: Function '{functionName}' with {count} parameters doesn't exist";
        Report(span,message);
    }
}

