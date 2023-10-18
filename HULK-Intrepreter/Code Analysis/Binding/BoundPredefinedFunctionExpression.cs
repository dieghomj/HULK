using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis.Binding
{
    internal class BoundPredefinedFunctionExpression : BoundExpression
    {

        public BoundPredefinedFunctionExpression(SyntaxToken function, List<BoundExpression> arguments, Type resultType)
        {
            Function = function;
            Arguments = arguments;
            ResultType = resultType;
        }


        public SyntaxToken Function { get; }
        public List<BoundExpression> Arguments { get; }
        public Type ResultType { get; }

        public override Type Type => ResultType;

        public override BoundNodeKind Kind => BoundNodeKind.PredefinedFunction;

    }
}