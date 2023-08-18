namespace HULK
{
    class Program
    {
        public static void Main(string[] args)
        {
            while(true){
                Console.Write(".>");
                var line = Console.ReadLine();
                if(string.IsNullOrWhiteSpace(line))return;

                var lexer = new Lexer(line);
                while(true){
                    var token = lexer.NextToken();
                    if(token.Kind == SyntaxKind.EndOfFileToken)break;
                    Console.WriteLine($"({token.Kind}): ({token.Text})");
                    
                    if(token.Value != null)Console.WriteLine($"({token.Value})");
                }

            }
        }
    }
}