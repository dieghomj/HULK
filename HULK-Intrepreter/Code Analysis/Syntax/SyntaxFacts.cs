
namespace HULK.CodeAnalysis.Syntax
{
    internal static class SyntaxFacts
    {
        public static int GetUnaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch(kind)
            {
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                case SyntaxKind.BangToken:
                    return 8;
                  
                default:
                    return 0;
            }
        }
        public static int GetBinaryOperatorPrecedence(this SyntaxKind kind)
        {
            switch(kind)
            {
                case SyntaxKind.CircumflexToken:
                    return 7;

                case SyntaxKind.StarToken:
                case SyntaxKind.DivToken:
                case SyntaxKind.PercentToken:
                    return 6;
                    
                case SyntaxKind.PlusToken:
                case SyntaxKind.MinusToken:
                    return 5;

                case SyntaxKind.AtToken:
                    return 4;
                
                case SyntaxKind.EqualEqualToken:
                case SyntaxKind.BangEqualToken:
                    return 3;

                case SyntaxKind.AmpersandToken:
                    return 2;
                    
                case SyntaxKind.PipeToken:
                    return 1;
                
                default:
                    return 0;
            }
        }

        public static SyntaxKind GetKeyWordKind(string text)
        {
            switch(text)
            {
                case "true":
                    return SyntaxKind.TrueKeyword;
                case "false":
                    return SyntaxKind.FalseKeyword;
                default:
                    return SyntaxKind.IdentifierToken;
            }
        }
    }
}

