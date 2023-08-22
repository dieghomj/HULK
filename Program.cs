namespace HULK
{
    class Program
    {
        public static void Main(string[] args)
        {
            bool showTree = false;
            while (true)
            {
                Console.Write(".>");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) return;

                if ( line == "#showtree"){
                    Console.WriteLine("Showing parse trees");

                    

                    continue;
                }

//===================================================================================
//                          Parse
//===================================================================================

                var syntaxTree = SyntaxTree.Parse(line);

//===================================================================================
//                          Tree Print
//===================================================================================

                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGray;

                TreePrint(syntaxTree.Root);
                Console.ForegroundColor = color;

//===================================================================================
//                       ERROR PRINTING
//===================================================================================

                if (!syntaxTree.Diagnostics.Any())
                {
                    var e = new Evaluator(syntaxTree.Root);
                    var result = e.Evaluate();
                    Console.WriteLine(result);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in syntaxTree.Diagnostics)
                        Console.WriteLine(diagnostic);

                    Console.ForegroundColor = color;
                }
                //===================================================================================
                //===================================================================================
            }
        }

        static void TreePrint(SyntaxNode node, string indent = "")
        {
            Console.Write(indent);
            Console.Write(node.Kind);
            if (node is SyntaxToken t && t.Value != null)
            {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += "    ";

            foreach (var child in node.GetChildren())
                TreePrint(child, indent);
        }

    }
}