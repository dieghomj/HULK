using System;

namespace HULK.CodeAnalysis.Syntax
{

    internal sealed class Lexer
    {
        private readonly DiagnosticBag _diagnostics = new DiagnosticBag();
        private readonly string _text;
        private int _position;
        private int _start;
        private SyntaxKind _kind;
        private object _value;


        public Lexer(string line)
        {
            this._text = line;
        }

        public DiagnosticBag Diagnostics => _diagnostics;

        private char Current => Peek(0);

        private char LookAhead => Peek(1);

        private char Peek(int offset)
        {
            var index = _position + offset;
            if (index >= _text.Length)
                return '\0';
            return _text[index];
        }

        private void Next()
        {
            _position++;
        }

        public SyntaxToken Lex()
        {
            //This method tokenizes the give string
            //At the moment reads:
            //<numbers>
            //<*-+/>
            //<whitespaces>
            //TODO : implement next tokens: <if - else keywords> <methods/functions> <let - in keywords> < math basic functions> <name variables>

            _start = _position;
            _kind = SyntaxKind.BadToken;
            _value = null;

            if (char.IsDigit(Current))
            {
                ReadNumberToken();
            }
            else if (char.IsWhiteSpace(Current))
            {
                ReadWhiteSpace();
            }
            else if (char.IsLetter(Current))
            {
                ReadIdentifierOrKeyword();
            }
            else if (Current == '"')
            {
                ReadString();
            }
            else
            {
                switch (Current)
                {
                    case '\0':
                    {
                        _diagnostics.ReportExpectedCharacter(new TextSpan(_position - 1, 1), ';');
                        _kind = SyntaxKind.EndOfFileToken; 
                        break;
                    }
                    case '+':
                    {
                        _kind = SyntaxKind.PlusToken;
                        _position++;
                        break;
                    }
                    case '-':
                    {
                        _kind = SyntaxKind.MinusToken;
                        _position++;
                        break;
                    }
                    case '*':
                    {
                        _kind = SyntaxKind.StarToken;
                        _position++;
                        break;
                    }
                    case '/':
                    {
                        _kind = SyntaxKind.DivToken;
                        _position++;
                        break;
                    }
                    case '%':
                    {
                        _kind = SyntaxKind.PercentToken;
                        _position++;
                        break;
                    }
                    case '^':
                    {
                        _kind = SyntaxKind.CircumflexToken;
                        _position++;
                        break;
                    }
                    case '@':
                    {
                        _kind = SyntaxKind.AtToken;
                        _position++;
                        break;
                    }
                    case '(':
                    {
                        _kind = SyntaxKind.OpenParenthesisToken;
                        _position++;
                        break;
                    }
                    case ')':
                    {
                        _kind = SyntaxKind.CloseParenthesisToken;
                        _position++;
                        break;
                    }
                    case '&':
                    {
                        _kind = SyntaxKind.AmpersandToken;
                        _position++;
                        break;
                    }
                    case '|':
                    {
                        _kind = SyntaxKind.PipeToken;
                        _position++;
                        break;
                    }
                    case '=':
                    if (LookAhead == '=')
                    {
                        _kind = SyntaxKind.EqualEqualToken;
                        _position+= 2;
                        break;
                    }
                    else
                    {
                        _kind = SyntaxKind.EqualsToken;
                        _position++;
                        break;
                    }
                    case '!':
                    if (LookAhead == '=')
                    {
                        _kind = SyntaxKind.BangEqualToken;
                        _position+= 2;
                        break;
                    }
                    else
                    {
                        _kind = SyntaxKind.BangToken;
                        _position++;
                        break;
                    }
                    case ',':
                        _kind = SyntaxKind.CommaToken;
                        _position++;
                        break;
                    case ';':
                    {
                        _kind = SyntaxKind.EndOfFileToken;
                        break;
                    }    
                    default:
                        _diagnostics.ReportBadCharacter(_position, Current);
                        // _kind = SyntaxKind.BadToken
                        _position++;
                        break;
                }
            }

            var length = _position - _start;
            var text = SyntaxFacts.GetText(_kind);
            if (text == null)
                text = _text.Substring(_start, length);

            return new SyntaxToken(_kind, _start, text, _value);
        }

        private void ReadWhiteSpace()
        {
            while (char.IsWhiteSpace(Current))
                Next();
            _kind = SyntaxKind.WitheSpaceToken;
        }

        private void ReadNumberToken()
        {
            while (char.IsDigit(Current))
                Next();
            var length = _position - _start;
            var text = _text.Substring(_start, length);

            if (!double.TryParse(text, out var value))
                _diagnostics.ReportInvalidNumber(new TextSpan(_start, length), text, typeof(int));

            _value = value;
            _kind = SyntaxKind.NumberToken;
        }
        
        private void ReadIdentifierOrKeyword()
        {
            while (char.IsLetter(Current))
                Next();
            var length = _position - _start;
            var text = _text.Substring(_start, length);
            _kind = SyntaxFacts.GetKeyWordKind(text);
        }
        
        private void ReadString()
        {
            Next();
            while (Current != '"')
            {
                if (_position < _text.Length)
                    Next();
                else
                {
                    _diagnostics.ReportExpectedCharacter(new TextSpan(_position - 1, 1), '"');
                    break;
                }
            }
            if (Current == '"')
                Next();
            var length = _position - _start;
            var text = _text.Substring(_start,length);

            _value = text.Substring(1,length-2);
            _kind = SyntaxKind.StringToken;
        }
    }
}
