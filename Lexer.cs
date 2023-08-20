using System;

namespace HULK
{

    public class Lexer
    {
        private readonly string _text;
        private int _position;
        private List<string> _diagnostics = new List<string>();


        public Lexer(string line)
        {
            this._text = line;
        } 

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current
        {
            get
            {
                if (_position >= _text.Length)
                    return '\0';
                return _text[_position];
            }
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken NextToken()
        {
            //This method tokenizes the give string
            //At the moment reads:
            //<numbers>
            //<*-+/>
            //<whitespaces>
            //TODO : implement next tokens: <if - else keywords> <methods/functions> <let - in keywords> < math basic functions> <name variables>
        
            if (_position >= _text.Length)
            {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _position, "\0", null);
            }

            if (char.IsDigit(Current))
            {
                var start = _position;
                
                while (char.IsDigit(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);

                if(!int.TryParse(text, out var value))
                    _diagnostics.Add($"The number {_text} isn't valid Int32");

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);

            }

            if (char.IsWhiteSpace(Current))
            {
                var start = _position;

                while (char.IsWhiteSpace(Current))
                    Next();
                var length = _position - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxKind.WitheSpaceToken, start, text, null);

            }

            if (Current == '+')
                return new SyntaxToken(SyntaxKind.PlusToken, _position++, "+", null);
            else if (Current == '-')
                return new SyntaxToken(SyntaxKind.MinusToken, _position++, "-", null);
            else if (Current == '*')
                return new SyntaxToken(SyntaxKind.StarToken, _position++, "*", null);
            else if (Current == '/')
                return new SyntaxToken(SyntaxKind.DivToken, _position++, "/", null);
            else if (Current == '(')
                return new SyntaxToken(SyntaxKind.OpenParenthisisToken, _position++, "(", null);
            else if (Current == ')')
                return new SyntaxToken(SyntaxKind.CloseParenthisisToken, _position++, ")", null);

            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _position++, _text.Substring(_position - 1, 1), null);

        }

    }
}
