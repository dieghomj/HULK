using HULK.CodeAnalysis;
using HULK.CodeAnalysis.Syntax;

namespace HULK
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            while (true)
            {
                Console.Write(".>");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) return;

                if ( line == "#showtree"){
                    
                    showTree = !showTree;
                    Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                    continue;
                
                }


//===================================================================================
//                          Parse
//===================================================================================
                var syntaxTree = SyntaxTree.Parse(line);
                
//===================================================================================
//                          Tree Print
//===================================================================================
                if (showTree){
            
                    var color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    TreePrint(syntaxTree.Root);
                    Console.ResetColor();
            
                }
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

                    Console.ResetColor();
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