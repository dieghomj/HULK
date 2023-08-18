using System;

namespace HULK{

    abstract class SyntaxNode
    {

        public abstract SyntaxKind Kind { get; }

    }

    abstract class ExpressionSyntax: SyntaxNode
    {

    }

    sealed class NumberExpressionSyntax : ExpressionSyntax
    {
        public NumberExpressionSyntax( SyntaxToken numberToken)
        {

        }

        public override SyntaxKind Kind => SyntaxKind.NumberExpression;
    }

    sealed class BinaryExpressionSyntax : ExpressionSyntax
    {
        public BinaryExpressionSyntax(ExpressionSyntax left, SyntaxNode operatorToken, ExpressionSyntax rigth)
        {
            Left = left;
            OperatorToken = operatorToken;
            Rigth = rigth;
        }

        public override SyntaxKind Kind => SyntaxKind.BinaryExpression;

        public ExpressionSyntax Left { get; }
        public SyntaxNode OperatorToken { get; }
        public ExpressionSyntax Rigth { get; }
    }

    public class Parser
    {
        // 1 + 2 * 3

        //      +
        //    /   \
        //  1      *
        //       /   \
        //      2     3

        private int _position;
        private SyntaxToken[] _tokens;

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
        } 

        private SyntaxToken Peek(int offset)
        {
            var index = _position + offset;
            if(index >= _tokens.Length)
                return _tokens[_tokens.Length];
            return  _tokens[index];
        }

        private SyntaxToken Current => Peek(0);


    }
}

