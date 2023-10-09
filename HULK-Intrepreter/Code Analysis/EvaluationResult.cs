namespace HULK.CodeAnalysis
{
    public sealed class EvaluationResult
    {
        public EvaluationResult(IEnumerable<Diagnostic> diagnostics, object value)
        {
            Diagnostics = diagnostics;
            Value = value;
        }

        public IEnumerable<Diagnostic> Diagnostics { get; }
        public object Value { get; }
    }
}

