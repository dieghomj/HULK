using HULK.CodeAnalysis.Syntax;

namespace HULK.CodeAnalysis.Binding
{
    internal sealed class Binder
    {

        private DiagnosticBag _diagnostics = new DiagnosticBag();
        private Dictionary<VariableSymbol, object> _variables;
        private Dictionary<FunctionSymbol, object> _functions;

        public Binder(Dictionary<VariableSymbol, object> variables, Dictionary<FunctionSymbol, object> functions)
        {
            _variables = variables;
            _functions = functions;
        }
        public DiagnosticBag Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax)
        {
            switch (syntax.Kind)
            {
                case SyntaxKind.ParenthesizedExpression:
                    return BindParenthesizedExpression((ParenthesizedExpressionSyntax)syntax);
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax)syntax);
                case SyntaxKind.NameExpression:
                    return BindNameExpression((NameExpressionSyntax)syntax);
                case SyntaxKind.AssignmentExpression:
                    return BindAssignmentExpression((AssignmentExpressionSyntax)syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax)syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax)syntax);
                case SyntaxKind.LetInExpression:
                    return BindLetInExpression((LetInExpressionSyntax)syntax);
                case SyntaxKind.IfElseExpression:
                    return BindIfElseExpression((IfElseExpressionSyntax)syntax);
                case SyntaxKind.FunctionCallExpression:
                    return BindFunctionCallExpression((FunctionCallExpressionSyntax)syntax);
                case SyntaxKind.FunctionDeclarationExpression:
                    return BindFunctionDeclarationExpression((FunctionDeclarationExpressionSyntax)syntax);
                case SyntaxKind.PredefinedFunctionExpression:
                    return BindPredefinedFunction((PredefinedFunctionExpressionSyntax)syntax);
                    
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindPredefinedFunction(PredefinedFunctionExpressionSyntax syntax)
        {
            var argumentsType = new List<Type>();
            var arguments = new List<BoundExpression>();
            foreach (var argument in syntax.Arguments)
            {
                arguments.Add(BindExpression(argument));
                argumentsType.Add(arguments.Last().Type);
            }   
                
            
            var name = syntax.Function.Text;
            var predefinedFunction = BoundPredefinedFunction.Bind(name, argumentsType.Count ,argumentsType.ToArray());
            
            if(predefinedFunction == null)
            {
                _diagnostics.ReportUndefinedFunction(syntax.Function.TextSpan,name,argumentsType.Count);
                return arguments.Last();
            }

            return new BoundPredefinedFunctionExpression(syntax.Function, arguments, predefinedFunction.ResultType);
        }

        private BoundExpression BindFunctionDeclarationExpression(FunctionDeclarationExpressionSyntax syntax)
        {
            var variablesKeys = new List<VariableSymbol>();

            foreach (var p in syntax.Parameters)
            {
                var variableHolder = new VariableSymbol(p.Text, typeof(void)); 
                variablesKeys.Add(variableHolder);
                _variables[variableHolder] = null;
            }

            var existingFunction = _functions.Keys.FirstOrDefault(v => v.Name == syntax.FunctionName.Text);

            if ( existingFunction != null )
                _functions.Remove(existingFunction);
            var functionHolder = new FunctionSymbol(syntax.FunctionName.Text, typeof(void), variablesKeys);

            _functions[functionHolder] = null;
            var boundFunctionBody = BindExpression(syntax.Expression);
            _functions.Remove(functionHolder);

            foreach (var v in variablesKeys)
                _variables.Remove(v);

            _functions[new FunctionSymbol(syntax.FunctionName.Text, typeof(void) ,variablesKeys)] = boundFunctionBody;
            return new BoundFunctionDeclarationExpression(syntax.FunctionName, variablesKeys);
        }

        private BoundExpression BindFunctionCallExpression(FunctionCallExpressionSyntax syntax)
        {
            var functionName = syntax.FunctionName.Text;
            var boundArguments = new List<BoundExpression>();
            foreach (var argument in syntax.Arguments)
                boundArguments.Add(BindExpression(argument));
            
            var function = _functions.Keys.FirstOrDefault(v => v.Name == functionName && v.Parameters.Count == boundArguments.Count);
            
            if(function == null)
            {
                _diagnostics.ReportUndefinedFunction(syntax.FunctionName.TextSpan,functionName,boundArguments.Count);
                return new BoundLiteralExpression(0);
            }

            return new BoundFunctionCallExpression(functionName, boundArguments);
        }

        private BoundExpression BindIfElseExpression(IfElseExpressionSyntax syntax)
        {
            
            var boundCondition = BindExpression(syntax.Condition);
            if(boundCondition.Type != typeof(bool))
                _diagnostics.ReportUnexpectedType(syntax.OpenParenthesisToken.TextSpan, syntax.CloseParenthesisToken.TextSpan,boundCondition.Type,typeof(bool));
            
            var boundTrueExpression = BindExpression(syntax.TrueExpression);
            var boundFalseExpression = BindExpression(syntax.FalseExpression);

            return new BoundIfElseExpression( boundCondition, boundTrueExpression, boundFalseExpression);
        }

        private BoundExpression BindLetInExpression(LetInExpressionSyntax syntax)
        {
            var boundAssignments = new List<BoundExpression>();

            foreach (var assignment in syntax.Assignments)
            {
                boundAssignments.Add(BindExpression(assignment));
            }
            var boundExpression = BindExpression(syntax.Expression);

            return new BoundLetInExpression(boundAssignments, boundExpression);
        }

        private BoundExpression BindParenthesizedExpression(ParenthesizedExpressionSyntax syntax)
        {
            return BindExpression(syntax.Expression);
        }

        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {

            var value = syntax.Value ?? 0.0;
            return new BoundLiteralExpression(value);
        }

        private BoundExpression BindNameExpression(NameExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var variable = _variables.Keys.FirstOrDefault(v => v.Name == name );

            if(variable == null)
            {
                _diagnostics.ReportUndefinedName(syntax.IdentifierToken.TextSpan,name);
                return new BoundLiteralExpression(0);
            }
            return new BoundVariableExpression(variable);

        }

        private BoundExpression BindAssignmentExpression(AssignmentExpressionSyntax syntax)
        {
            var name = syntax.IdentifierToken.Text;
            var boundExpression = BindExpression(syntax.Expression);
            var existingVariable = _variables.Keys.FirstOrDefault(v => (v.Name == name) && (v.Type != typeof(void)));
            if( existingVariable != null)
                _variables.Remove(existingVariable);

            var variable = new VariableSymbol(name,boundExpression.Type);
            _variables[variable] = null;

            return new BoundAssignmentExpression(variable,boundExpression);
        }

        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperand = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperand.Type);
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedUnaryOperator(syntax.OperatorToken.TextSpan, syntax.OperatorToken.Text, boundOperand.Type);
                return boundOperand;
            }

            return new BoundUnaryExpression(boundOperator, boundOperand);
        }


        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);

            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);
            if (boundOperator == null)
            {
                _diagnostics.ReportUndefinedBinaryOperator(syntax.OperatorToken.TextSpan, syntax.OperatorToken.Text, boundLeft.Type, boundRight.Type);
                return boundLeft;
            }
            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);
        }
    }
}