using System;

namespace HULK
{

    internal sealed class Parser
    {
        // 1 + 2 * 3

        //      +
        //    /   \
        //  1      *
        //       /   \
        //      2     3

        private int _position;
        private SyntaxToken[] _tokens;
        private List<string> _diagnostics = new List<string>();

        public Parser(string text){

            //the constructor tokenizes the line and stores the tokens in the tokens array <_tokens>

            var tokens = new List<SyntaxToken>();
            
            var lexer = new Lexer(text);
            SyntaxToken token;
            
            do
            {
                token = lexer.NextToken();

                if( token.Kind != SyntaxKind.WitheSpaceToken &&
                    token.Kind != SyntaxKind.BadToken)
                {
                    tokens.Add(token);
                }
            }
            while( token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        } 

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if(index >= _tokens.Length)
                return _tokens[_tokens.Length];
            return  _tokens[index];
        }

        private SyntaxToken Current => Peek(0);

        private SyntaxToken NextToken(){
            var current = Current;
            _position++;
            return current;
        }

        private SyntaxToken Match(SyntaxKind kind)
        {
            if(Current.Kind == kind)return NextToken();
            
            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}> expected <{kind}>");
            return new SyntaxToken(kind, Current.Position, null, null);
        }

        public SyntaxTree Parse()
        {
            var expression =  ParseExpression();
            var endOfFileToken = Match(SyntaxKind.EndOfFileToken);
            return new SyntaxTree(_diagnostics, expression, endOfFileToken);
        }

        private ExpressionSyntax ParseExpression(){
            return ParseTerm();
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken ||
                    Current.Kind == SyntaxKind.MinusToken)
            {
                var operatorToken = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, operatorToken, right);

            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.StarToken ||
                    Current.Kind == SyntaxKind.DivToken)
            {
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, operatorToken, right);

            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {

            if(Current.Kind == SyntaxKind.MinusToken){
                
                var zero = new SyntaxToken(SyntaxKind.NumberToken, -1, "0", 0);
                var left = new LiteralExpressionSyntax(zero);
                var operatorToken = NextToken();
                var right = ParsePrimaryExpression();

                return new BinaryExpressionSyntax(left,operatorToken, right);
            }

            if(Current.Kind == SyntaxKind.OpenParenthisisToken){
                var left = NextToken();
                var expression = ParseExpression();
                var right = Match(SyntaxKind.CloseParenthisisToken);
                return new ParenthisizedExpressionSyntax(left,expression,right);
            }

            var numberToken = Match(SyntaxKind.NumberToken);
            return new LiteralExpressionSyntax(numberToken);
        }
    }
}

