using HULK.CodeAnalysis;
using HULK.CodeAnalysis.Binding;
using HULK.CodeAnalysis.Syntax;

namespace HULK
{
    internal static class Program
    {
        private static void Main()
        {
            bool showTree = false;
            var variables = new Dictionary<VariableSymbol,object>();

            while (true)
            {
                Console.Write("> ");
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
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables);

                var diagnostics = result.Diagnostics;

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

                if (!diagnostics.Any())
                {
                    Console.WriteLine(result.Value);
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.DarkRed;

                    foreach (var diagnostic in diagnostics)
                    {
                        Console.WriteLine();

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine(diagnostic);
                        Console.ResetColor();
                        
                        var prefix = line.Substring(0, diagnostic.Span.Start);
                        var error = line.Substring(diagnostic.Span.Start, diagnostic.Span.Length);
                        var suffix = line.Substring(diagnostic.Span.End);

                        Console.Write(prefix);

                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.Write(error);
                        Console.ResetColor();

                        Console.Write(suffix);

                        Console.WriteLine();
                    }
                }

                Console.WriteLine();

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