using System;

namespace HULK.CodeAnalysis.Syntax
{


    internal sealed class Parser
    {
        // 1 + 2 * 3

        //      +
        //    /   \
        //  1      *
        //       /   \
        //      2     3

        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private int _position;
        private string _text;
        private SyntaxToken[] _tokens;

        public Parser(string text)
        {
            _text = text;
            var tokens = new List<SyntaxToken>();

            var lexer = new Lexer(text);
            SyntaxToken token;

            do
            {
                token = lexer.Lex();

                if (token.Kind != SyntaxKind.WitheSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            }
            while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _tokens.Length)
                return _tokens[_tokens.Length];
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken()
        {
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind)
        {
            if (Current.Kind == kind) return NextToken();

            _diagnostics.ReportUnexpectedToken(Current.TextSpan, Current.Kind, kind);
            return new SyntaxToken(kind, Current.Position, Current.Text, null);
        }

        public CompilationUnitSyntax ParseCompilationUnit()
        {
            var expression = ParseExpression();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
            return new CompilationUnitSyntax(expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression()
        {
            if (Current.Kind == SyntaxKind.FunctionKeyword)
                return ParseFunctionDeclarationExpression();
            else return ParseBinaryExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {
            var identifierToken = Match(SyntaxKind.IdentifierToken);
            var operatorToken = Match(SyntaxKind.EqualsToken);
            var right = ParseExpression();
            return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
        }
        
        private ExpressionSyntax ParseBinaryExpression(int parentPrecedence = 0)
        {
            ExpressionSyntax left;
            var unaryOperatorPrecedence = Current.Kind.GetUnaryOperatorPrecedence();
            if( unaryOperatorPrecedence != 0 && unaryOperatorPrecedence >= parentPrecedence)
            {
                var operatorToken = NextToken();
                var operand = ParseBinaryExpression(unaryOperatorPrecedence);
                left = new UnaryExpressionSyntax(operatorToken, operand);
            }
            else
            {
                left = ParsePrimaryExpression();
            }

            while(true)
            {
                var precedence = Current.Kind.GetBinaryOperatorPrecedence();
                if(precedence == 0 || precedence <= parentPrecedence)
                    break;

                var operatorToken = NextToken(); 
                var right = ParseBinaryExpression(precedence);
                left = new BinaryExpressionSyntax(left, operatorToken, right);
            }

            return left;
        }


        private ExpressionSyntax ParsePrimaryExpression()
        {

            switch (Current.Kind)
            {
                case SyntaxKind.OpenParenthesisToken:
                    return ParseParenthesizedExpression();

                case SyntaxKind.FalseKeyword:
                case SyntaxKind.TrueKeyword:
                    return ParseBoolLiteral();

                case SyntaxKind.StringToken:
                    return ParseStringLiteral();

                case SyntaxKind.IdentifierToken:
                {
                    if(Peek(1).Kind == SyntaxKind.OpenParenthesisToken)
                        return ParseFunctionCallExpression();
                    else 
                        return ParseNameExpression();
                }
                case SyntaxKind.LetKeyword:
                    return ParseLetInExpression();
                case SyntaxKind.IfKeyword:
                    return ParseIfElseExpression();

                case SyntaxKind.PredefinedFunctionKeyword:
                    return ParsePredefinedFunction();
                case SyntaxKind.ConstantKeyword:
                    return ParseConstantKeyword();

                default:
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                }
        }

        private ExpressionSyntax ParseConstantKeyword()
        {
            var constant = Match(SyntaxKind.ConstantKeyword);
            if(constant.Text == "PI")
                return new LiteralExpressionSyntax(constant, Math.PI);        
            else if(constant.Text == "E")
                return new LiteralExpressionSyntax(constant, Math.E);        
            else 
            {
                throw new Exception($"Undefined constant <{constant.Text}>");
            }
        }

        private ExpressionSyntax ParsePredefinedFunction()
        {
            var function = Match(SyntaxKind.PredefinedFunctionKeyword);
            var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
            var argument = ParseArguments();
            var closedParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);
            return new PredefinedFunctionExpressionSyntax(function, openParenthesisToken, argument, closedParenthesisToken);
        }

        private ExpressionSyntax ParseFunctionCallExpression()
        {
            var identifierToken = Match(SyntaxKind.IdentifierToken);
            var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
            var arguments = ParseArguments();
            var closedParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);

            return new FunctionCallExpressionSyntax(identifierToken, openParenthesisToken, arguments, closedParenthesisToken);
        }

        private ExpressionSyntax ParseFunctionDeclarationExpression()
        {
            var functionToken = Match(SyntaxKind.FunctionKeyword);
            var functionName = Match(SyntaxKind.IdentifierToken);
            var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
            var parameters = ParseParameters();
            var closedParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);
            var equalToken = Match(SyntaxKind.EqualsToken);
            var greaterToken = Match(SyntaxKind.GreaterToken);
            var body = ParseExpression();

            return new FunctionDeclarationExpressionSyntax(functionToken, functionName, openParenthesisToken, parameters, closedParenthesisToken, equalToken, greaterToken, body);

        }


        private ExpressionSyntax ParseIfElseExpression()
        {
            var ifToken = Match(SyntaxKind.IfKeyword);
            var openParenthesisToken = Match(SyntaxKind.OpenParenthesisToken);
            var condition = ParseExpression();
            var closedParenthesisToken = Match(SyntaxKind.CloseParenthesisToken);
            var trueExpression = ParseExpression();
            var elseToken = Match(SyntaxKind.ElseKeyword);
            var falseExpression = ParseExpression();

            return new IfElseExpressionSyntax(ifToken, openParenthesisToken, condition, closedParenthesisToken, trueExpression, elseToken, falseExpression);

        }

        private ExpressionSyntax ParseLetInExpression()
        {
            var letToken = Match(SyntaxKind.LetKeyword);

            List<ExpressionSyntax> assignments = ParseMultipleAssignments();

            var inToken = Match(SyntaxKind.InKeyword);
            var expression = ParseExpression();

            return new LetInExpressionSyntax(letToken, assignments, inToken, expression);
        }

        private List<ExpressionSyntax> ParseMultipleAssignments()
        {
            var assignments = new List<ExpressionSyntax>();
            while (Current.Kind != SyntaxKind.InKeyword)
            {
                var assignment = ParseAssignmentExpression();
                assignments.Add(assignment);
                if (Current.Kind == SyntaxKind.CommaToken)
                    Match(SyntaxKind.CommaToken);
                else 
                    break;
            }

            return assignments;
        }
        private List<ExpressionSyntax> ParseArguments()
        {
            var arguments = new List<ExpressionSyntax>();
            while (Current.Kind != SyntaxKind.CloseParenthesisToken)
            {
                var argument = ParseExpression();
                arguments.Add(argument);
                if (Current.Kind == SyntaxKind.CommaToken)
                    Match(SyntaxKind.CommaToken);
                else 
                    break;
            }

            return arguments;
        }

        private List<SyntaxToken> ParseParameters()
        {
            var parameters = new List<SyntaxToken>();
            while (Current.Kind != SyntaxKind.CloseParenthesisToken)
            {
                var parameter = Match(SyntaxKind.IdentifierToken);
                parameters.Add(parameter);
                if (Current.Kind == SyntaxKind.CommaToken)
                    Match(SyntaxKind.CommaToken);
                else 
                    break;
            }
            return parameters;
        }

        private ExpressionSyntax ParseNumberLiteral()
        {
            var numberToken = Match(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }

        private ExpressionSyntax ParseParenthesizedExpression()
        {
            var left = Match(SyntaxKind.OpenParenthesisToken);
            var expression = ParseExpression();
            var right = Match(SyntaxKind.CloseParenthesisToken);
            return new ParenthesizedExpressionSyntax(left, expression, right);
        }

        private ExpressionSyntax ParseBoolLiteral()
        {
            var isTrue = Current.Kind == SyntaxKind.TrueKeyword; 
            var keywordToken = isTrue ? Match(SyntaxKind.TrueKeyword) : Match(SyntaxKind.FalseKeyword);
            return new LiteralExpressionSyntax(keywordToken, isTrue);
        }

        private ExpressionSyntax ParseStringLiteral()
        {
            var stringToken = Match(SyntaxKind.StringToken);
            return new LiteralExpressionSyntax(stringToken);
        }

        private ExpressionSyntax ParseNameExpression()
        {
            var identifierToken = Match(SyntaxKind.IdentifierToken);
            return new NameExpressionSyntax(identifierToken);
        }

    }
}

