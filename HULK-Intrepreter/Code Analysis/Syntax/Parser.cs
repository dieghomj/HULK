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
            return ParseAssignmentExpression();
        }

        private ExpressionSyntax ParseAssignmentExpression()
        {

            if( Peek(0).Kind == SyntaxKind.IdentifierToken && 
                Peek(1).Kind == SyntaxKind.EqualsToken)
            {
                var identifierToken = NextToken();
                var operatorToken = NextToken();
                var right = ParseAssignmentExpression();
                return new AssignmentExpressionSyntax(identifierToken, operatorToken, right);
            }

            return ParseBinaryExpression();

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
                    return ParseNameExpression();

                default:
                case SyntaxKind.NumberToken:
                    return ParseNumberLiteral();
                }
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

