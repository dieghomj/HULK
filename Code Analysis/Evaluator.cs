namespace HULK
{
    public class Evaluator
    {
        private readonly ExpressionSyntax root;
        public Evaluator(ExpressionSyntax root)
        {
            this.root = root;
        }

        public int Evaluate()
        {
            return EvaluateExpression(root);
        }

        private int EvaluateExpression(ExpressionSyntax node)
        {
            //BinaryExpression
            //NumberExpression

            if(node is LiteralExpressionSyntax n)
            {
                return (int) n.LiteralToken.Value;
            }

            if(node is UnaryExpressionSyntax u)
            {
                var operand = EvaluateExpression(u.Operand);

                if(u.OperatorToken.Kind == SyntaxKind.MinusToken)
                {
                    return -operand;
                }

                else if(u.OperatorToken.Kind == SyntaxKind.PlusToken)
                {
                    return operand;
                } 
                else 
                    throw new Exception($"Unexpected unary operator <{u.OperatorToken.Kind}>");
            }

            if(node is BinaryExpressionSyntax b)
            {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);
                if(b.OperatorToken.Kind == SyntaxKind.PlusToken)
                    return left + right;
                else if(b.OperatorToken.Kind == SyntaxKind.MinusToken)
                    return left - right;
                else if(b.OperatorToken.Kind == SyntaxKind.StarToken)
                    return left * right;
                else if(b.OperatorToken.Kind == SyntaxKind.DivToken)
                    return left / right;
                else 
                    throw new Exception($"Unexpected binary operator <{b.OperatorToken.Kind}>");
            }

            if (node is ParenthisizedExpressionSyntax p )
                return EvaluateExpression(p.Expression);

            else throw new Exception($"Unexpected node {node.Kind}");
        }
    }
}

