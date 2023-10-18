using HULK.CodeAnalysis.Binding;
using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis
{
    internal class Evaluator
    {   
        private const int STACK_OVERFLOW_LIMIT = 1000;
        private int recursionCount = 0;
        private readonly BoundExpression root;
        private readonly Dictionary<FunctionSymbol, object> _functions;

        public Evaluator(BoundExpression root, Dictionary<VariableSymbol, object> variables, Dictionary<FunctionSymbol, object> functions)
        {
            this.root = root;
            _functions = functions;
        }

        public object Evaluate()
        {
            recursionCount = 0;
            return EvaluateExpression(root,new Dictionary<VariableSymbol, object>());
        }

        private object EvaluateExpression(BoundExpression node , Dictionary<VariableSymbol,object> variables)
        {
            // 1 + (let a = 1 in a) + (let a = 2 in a)

            switch (node.Kind)
            {
                case BoundNodeKind.LiteralExpression:
                    return EvaluateLiteralExpression((BoundLiteralExpression)node, variables);
                case BoundNodeKind.VariableExpression:
                    return EvaluateWithVariables(node,variables);
                case BoundNodeKind.AssignmentExpression:
                    return EvaluateWithVariables(node,variables);
                case BoundNodeKind.UnaryExpression:
                    return EvaluateUnaryExpression((BoundUnaryExpression)node, variables);
                case BoundNodeKind.BinaryExpression:
                    return EvaluateBinaryExpression((BoundBinaryExpression)node, variables);
                case BoundNodeKind.LetInExpression:
                    return EvaluateWithVariables(node, variables);
                case BoundNodeKind.IfElseExpression:
                    return EvaluateIfElseExpression((BoundIfElseExpression)node, variables);
                case BoundNodeKind.FunctionCallExpression:
                    return EvaluateFunctionCallExpression((BoundFunctionCallExpression)node, new Dictionary<VariableSymbol,object>(variables));
                case BoundNodeKind.FunctionDeclarationExpression:
                    return EvaluateFunctionDeclarationExpression((BoundFunctionDeclarationExpression)node, variables);
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
                    return EvaluateLetInExpression((BoundLetInExpression)node, new Dictionary<VariableSymbol,object>(variables));
                default: 
                    return EvaluateExpression(node,variables);            
            }
        }
        private object EvaluateFunctionDeclarationExpression(BoundFunctionDeclarationExpression node, Dictionary<VariableSymbol, object> variables)
        {
            var text = $"Function '{node.FunctionName.Text}' correctly declared";
            return text;
        }
        private object EvaluateFunctionCallExpression(BoundFunctionCallExpression node, Dictionary<VariableSymbol, object> variables)
        {
            var argumentsValue = new List<object>();
            foreach (var argument in node.Arguments)
                argumentsValue.Add(EvaluateExpression(argument,variables));
            return EvaluateFunction(node.FunctionName , argumentsValue, variables);
        }

        private object EvaluateFunction(string functionName, List<object> argumentsValue, Dictionary<VariableSymbol, object> variables)
        {
            var variableKeys = new List<VariableSymbol>();
            var functionSymbol = _functions.Keys.FirstOrDefault(f => f.Name == functionName);
            if(functionSymbol == null)
                throw new Exception($"Unexpected function name '{functionName}'");

            var functionBody = _functions[functionSymbol];

            for ( int i = 0; i < functionSymbol.Parameters.Count; i++)
            {
                var v = functionSymbol.Parameters[i];
                variables[v] = argumentsValue[i];
                variableKeys.Add(v);
            }

            return EvaluateExpression((BoundExpression)functionBody,variables);
        }

        private object EvaluateIfElseExpression(BoundIfElseExpression node, Dictionary<VariableSymbol, object> variables)
        {
            var condition = EvaluateExpression(node.Condition,variables);
            if((bool)condition)
                return EvaluateExpression(node.TrueExpression,variables);
            else 
                return EvaluateExpression(node.FalseExpression,variables);
        }

        private object EvaluateLetInExpression(BoundLetInExpression node, Dictionary<VariableSymbol, object> variables)
        {

            foreach (var a in node.Assignments)
                EvaluateWithVariables(a, variables);    

            var value =  EvaluateExpression(node.Expression, variables);

            return value;
        }

        private static object EvaluateLiteralExpression(BoundLiteralExpression n, Dictionary<VariableSymbol, object> variables)
        {
            return n.Value;
        }

        private object EvaluateVariableExpression(BoundVariableExpression v, Dictionary<VariableSymbol, object> variables)
        {
            if(variables.ContainsKey(v.Variable))
                return variables[v.Variable];
            else
            {
                return variables[variables.Keys.FirstOrDefault(k => k.Name == v.Variable.Name)];
            }
        }

        private object EvaluateAssignmentExpression(BoundAssignmentExpression a, Dictionary<VariableSymbol, object> variables)
        {
            var value = EvaluateExpression(a.Expression, variables);
            if(variables.ContainsKey(a.Variable))
                variables[a.Variable] = value;
            else 
                variables.Add(a.Variable, value);
            return value;
        }

        private object EvaluateUnaryExpression(BoundUnaryExpression u, Dictionary<VariableSymbol, object> variables)
        {
            var operand = EvaluateExpression(u.Operand, variables);

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

        private object EvaluateBinaryExpression(BoundBinaryExpression b, Dictionary<VariableSymbol, object> variables)
        {
            var left = EvaluateExpression(b.Left, variables);
            var right = EvaluateExpression(b.Right, variables);

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

