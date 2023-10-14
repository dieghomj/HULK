namespace HULK.CodeAnalysis.Binding
{
    internal class BoundFunctionCallExpression : BoundExpression
    {

        public BoundFunctionCallExpression(string functionName, List<BoundExpression> arguments)
        {
            FunctionName = functionName;
            Arguments = arguments;
        }

        public string FunctionName { get; }
        public List<BoundExpression> Arguments { get; }

        public override Type Type => typeof(void);

        public override BoundNodeKind Kind => BoundNodeKind.FunctionCallExpression;
    }
}