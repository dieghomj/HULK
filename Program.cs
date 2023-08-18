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

                var parser = new Parser(line);
                var expression = parser.Parse();

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                TreePrint(expression);
                Console.ForegroundColor = color;

                if(parser.Diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach(var diagnostic in parser.Diagnostics)
                        Console.WriteLine(diagnostic);

                    Console.ForegroundColor = color;
                }

            }
        }

        static void TreePrint(SyntaxNode node, string indent = "")
        {
            Console.Write(indent);
            Console.Write(node.Kind);
            if(node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += "    ";

            foreach(var child in node.GetChildren())
                TreePrint(child, indent);
        }

    }
}