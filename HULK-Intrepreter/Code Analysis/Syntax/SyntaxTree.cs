namespace HULK.CodeAnalysis.Syntax
{
    public sealed class SyntaxTree
    {
        private SyntaxTree(string text)
        {
            var parser = new Parser(text);
            var root = parser.ParseCompilationUnit();

            Diagnostics = parser.Diagnostics.ToArray();
            Root = root;
        }

        public IReadOnlyList<Diagnostic> Diagnostics { get; }
        public CompilationUnitSyntax Root { get; }

        public static SyntaxTree Parse(string text){
            return new SyntaxTree(text);
        }
        public static IEnumerable<SyntaxToken> ParseTokens(string text){
            var lexer = new Lexer(text);
            while(true)
            {
                var token = lexer.Lex();
                if(token.Kind == SyntaxKind.EndOfFileToken)
                    break;
                yield return token; 
            }
        }
    }
}