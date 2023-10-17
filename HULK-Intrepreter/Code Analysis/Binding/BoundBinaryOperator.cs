using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis.Binding
{
    internal sealed class BoundBinaryOperator
    {
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type) 
        : this(syntaxKind, kind, type, type, type) 
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type type, Type resultType) 
        : this(syntaxKind, kind, type, type, resultType) 
        {
        }
        private BoundBinaryOperator(SyntaxKind syntaxKind, BoundBinaryOperatorKind kind, Type leftType, Type rightType, Type resultType)
        {
            SyntaxKind = syntaxKind;
            Kind = kind;
            LeftType = leftType;
            RightType = rightType;
            Type = resultType;
        }


        public SyntaxKind SyntaxKind { get; }
        public BoundBinaryOperatorKind Kind { get; }
        public Type LeftType { get; }
        public Type RightType { get; }
        public Type Type { get; }

        private static BoundBinaryOperator[] _operators = 
        {
            //Number Operators
            new BoundBinaryOperator(SyntaxKind.PlusToken, BoundBinaryOperatorKind.Addition, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.MinusToken, BoundBinaryOperatorKind.Subtraction, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.StarToken, BoundBinaryOperatorKind.Multiplication, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.DivToken, BoundBinaryOperatorKind.Division, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.PercentToken, BoundBinaryOperatorKind.Remainder, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.CircumflexToken, BoundBinaryOperatorKind.Exponentiation, typeof(double)),
            new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, typeof(double), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterToken, BoundBinaryOperatorKind.Greater, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LessToken, BoundBinaryOperatorKind.Less, typeof(double), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.GreaterEqualToken, BoundBinaryOperatorKind.GreaterEqual, typeof(double), typeof(bool)), 
            new BoundBinaryOperator(SyntaxKind.LessEqualToken, BoundBinaryOperatorKind.LessEqual, typeof(double), typeof(bool)),

            //MixedOperators
            new BoundBinaryOperator(SyntaxKind.AtToken, BoundBinaryOperatorKind.Concatenation, typeof(string), typeof(double), typeof(string)),       
            new BoundBinaryOperator(SyntaxKind.AtToken, BoundBinaryOperatorKind.Concatenation, typeof(double), typeof(string), typeof(string)),       

            //String Operators
            new BoundBinaryOperator(SyntaxKind.AtToken, BoundBinaryOperatorKind.Concatenation, typeof(string)),       
            new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, typeof(string), typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, typeof(string), typeof(bool)),

            //Bool Operators
            new BoundBinaryOperator(SyntaxKind.AmpersandToken, BoundBinaryOperatorKind.LogicalAnd, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.PipeToken, BoundBinaryOperatorKind.LogicalOr, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.EqualEqualToken, BoundBinaryOperatorKind.Equals, typeof(bool)),
            new BoundBinaryOperator(SyntaxKind.BangEqualToken, BoundBinaryOperatorKind.NotEquals, typeof(bool)),

        };

        public static BoundBinaryOperator Bind(SyntaxKind syntaxKind, Type leftType, Type rightType)
        {
            foreach(var op in _operators)
            {
                if(op.SyntaxKind == syntaxKind && (leftType == typeof(void) || rightType == typeof(void)))
                    return op;
                if(op.SyntaxKind == syntaxKind && op.LeftType == leftType && op.RightType == rightType) 
                    return op;
            }
            return null;
        }
    }

}