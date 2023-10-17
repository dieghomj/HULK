using HULK.CodeAnalysis.Binding;
using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis
{
    internal class Evaluator
    {   
        private const int STACK_OVERFLOW_LIMIT = 1000;
        private Dictionary<FunctionSymbol, int> _stacks = new Dictionary<FunctionSymbol, int>();
        private int recursionCount = 0;
        private readonly BoundExpression root;
        private readonly Dictionary<VariableSymbol, object> _variables;
        private readonly Dictionary<FunctionSymbol, object> _functions;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables, Dictionary<FunctionSymbol, object> functions)
        {
            this.root = root;
            _variables = variables;
            _functions = functions;
        }

        public object Evaluate()
        {
            recursionCount = 0;
            return EvaluateExpression(root);
        }

        private object EvaluateExpression(BoundExpression node)
        {
            // 1 + (let a = 1 in a) + (let a = 2 in a)

            recursionCount++;
            if(recursionCount >= STACK_OVERFLOW_LIMIT)
                return "Stack Overflow";

            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)node);
                case BoundNodeKind.VariableExpression:
                    return EvaluateWithVariables(node,_variables);
                case BoundNodeKind.AssignmentExpression:
                    return EvaluateWithVariables(node,_variables);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)node);
                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression)node);
                case BoundNodeKind.LetInExpression:
                    return EvaluateWithVariables(node, _variables);
                case BoundNodeKind.IfElseExpression:
                    return EvaluateIfElseExpression((BoundIfElseExpression)node);
                case BoundNodeKind.FunctionCallExpression:
                    return EvaluateFunctionCallExpression((BoundFunctionCallExpression)node);
                case BoundNodeKind.FunctionDeclarationExpression:
                    return EvaluateFunctionDeclarationExpression((BoundFunctionDeclarationExpression)node);
                default:
                    throw new Exception($"Unexpected node {node.Kind}");
            }
        }


        private object EvaluateWithVariables(BoundExpression node, Dictionary<VariableSymbol, object> variables)
        {
            switch (node.Kind)
            {
                case BoundNodeKind.VariableExpression:
                    return EvaluateVariableExpression((BoundVariableExpression)node, variables);
                case BoundNodeKind.AssignmentExpression:
                    return EvaluateAssignmentExpression((BoundAssignmentExpression)node, variables);
                case BoundNodeKind.LetInExpression:
                    return EvaluateLetInExpression((BoundLetInExpression)node, variables);
                default: 
                    return EvaluateExpression(node);            
            }
        }
        private object EvaluateFunctionDeclarationExpression(BoundFunctionDeclarationExpression node)
        {
            var text = $"Function {node.FunctionName.Text} correctly declared";
            return text;
        }
        private object EvaluateFunctionCallExpression(BoundFunctionCallExpression node)
        {
            var argumentsValue = new List<object>();
            foreach (var argument in node.Arguments)
                argumentsValue.Add(EvaluateExpression(argument));
            return EvaluateFunction(node.FunctionName , argumentsValue);
        }

        private object EvaluateFunction(string functionName, List<object> argumentsValue)
        {
            var variableKeys = new List<VariableSymbol>();
            var functionSymbol = _functions.Keys.FirstOrDefault(f => f.Name == functionName);
            if(functionSymbol == null)
                throw new Exception($"Unexpected function name {functionName}");

            var functionBody = _functions[functionSymbol];

            for ( int i = 0; i < functionSymbol.Parameters.Count; i++)
            {
                var v = functionSymbol.Parameters[i];
                _variables[v] = argumentsValue[i];
                variableKeys.Add(v);
            }

            return EvaluateExpression((BoundExpression)functionBody);
        }

        private object EvaluateIfElseExpression(BoundIfElseExpression node)
        {
            var condition = EvaluateExpression(node.Condition);
            if((bool)condition)
                return EvaluateExpression(node.TrueExpression);
            else 
                return EvaluateExpression(node.FalseExpression);
        }

        private object EvaluateLetInExpression(BoundLetInExpression node, Dictionary<VariableSymbol, object> variables)
        {

            foreach (var a in node.Assignments)
                EvaluateWithVariables(a, variables);    

            var value =  EvaluateExpression(node.Expression);

            return value;
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n)
        {
            return n.Value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression v, Dictionary<VariableSymbol, object> variables)
        {
            return variables[v.Variable];
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a, Dictionary<VariableSymbol, object> variables)
        {
            var value = EvaluateExpression(a.Expression);
            variables[a.Variable] = value;
            return value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u)
        {
            var operand = EvaluateExpression(u.Operand);

                //TODO FIX this
            var bind = BoundUnaryOperator.Bind(u.Op.SyntaxKind, operand.GetType());
            if(bind == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"! SEMANTIC ERROR: Unary operator '{SyntaxFacts.GetText(u.Op.SyntaxKind)}' is not defined for type '{operand.GetType()}'");
                Console.ResetColor();
                return null;
            }

            switch (u.Op.Kind)
            {
                case BoundUnaryOperatorKind.Identity:
                    return (double)operand;
                case BoundUnaryOperatorKind.Negation:
                    return -(double)operand;
                case BoundUnaryOperatorKind.LogicalNegation:
                    return !(bool)operand;
                default:
                    throw new Exception($"Unexpected unary operator <{u.Op.Kind}>");
            }
        }

        private object EvaluateBinaryExpression(BoundBinaryExpression b)
        {
            var left = EvaluateExpression(b.Left);
            var right = EvaluateExpression(b.Right);

                //TODO FIX this
            var bind = BoundBinaryOperator.Bind(b.Op.SyntaxKind, left.GetType(), right.GetType());
            if(bind == null)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine($"! SEMANTIC ERROR: Binary operator '{SyntaxFacts.GetText(b.Op.SyntaxKind)}' is not defined for types '{left.GetType()}' and '{right.GetType()}'");
                Console.ResetColor();
                return null;
            }
                

            switch (b.Op.Kind)
            {
                case BoundBinaryOperatorKind.Addition:
                    return (double)left + (double)right;
                case BoundBinaryOperatorKind.Subtraction:
                    return (double)left - (double)right;
                case BoundBinaryOperatorKind.Multiplication:
                    return (double)left * (double)right;
                case BoundBinaryOperatorKind.Division:
                    return (double)left / (double)right;
                case BoundBinaryOperatorKind.Remainder:
                    return (double)left % (double)right;
                case BoundBinaryOperatorKind.Exponentiation:
                    return Math.Pow((double)left, (double)right);
                case BoundBinaryOperatorKind.Concatenation:
                    return left.ToString() + right.ToString();
                case BoundBinaryOperatorKind.LogicalAnd:
                    return (bool)left && (bool)right;
                case BoundBinaryOperatorKind.LogicalOr:
                    return (bool)left || (bool)right;
                case BoundBinaryOperatorKind.Equals:
                    return Equals(left, right);
                case BoundBinaryOperatorKind.NotEquals:
                    return !Equals(left, right);
                case BoundBinaryOperatorKind.Greater:
                    return (double)left > (double)right;
                case BoundBinaryOperatorKind.GreaterEqual:
                    return (double)left >= (double)right;
                case BoundBinaryOperatorKind.Less:
                    return (double)left < (double)right;
                case BoundBinaryOperatorKind.LessEqual:
                    return (double)left <= (double)right;
                default:
                    throw new Exception($"Unexpected binary operator <{b.Op.Kind}>");
            }
        }
    }
}

