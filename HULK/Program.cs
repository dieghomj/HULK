using System.ComponentModel.DataAnnotations;
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
            var functions = new Dictionary<FunctionSymbol, object>();
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("                           ##    ##    ##       ##   ##         ##     ##");
            Console.WriteLine("                           ##    ##    ##       ##   ##         ##   ##");
            Console.WriteLine("                           ##    ##    ##       ##   ##         ## ##");
            Console.WriteLine("                           ########    ##       ##   ##         ###");
            Console.WriteLine("                           ##    ##    ##       ##   ##         ## ##");
            Console.WriteLine("                           ##    ##     ##     ##    ##         ##   ##");
            Console.WriteLine("                           ##    ##       #####      ########   ##     ##");
            Console.WriteLine("=================================================================================================");
            
            PrintHULK();    
            Console.WriteLine();
            Console.WriteLine("=================================================================================================");
            Console.WriteLine("                               HAVANA UNIVERSITY LANGUAGE KOMPILER                               ");
            Console.WriteLine("=================================================================================================");
            Console.ResetColor();
            while (true)
            {
                Console.Write("> ");
                var line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) return;

                switch (line)
                {
                    case "#showtree":
                        showTree = !showTree;
                        Console.WriteLine(showTree ? "Showing parse trees." : "Not showing parse trees.");
                        continue;
                    case "#clear":
                        Console.Clear();
                        continue;
                }


                //===================================================================================
                //                          Parse
                //===================================================================================
                var syntaxTree = SyntaxTree.Parse(line);
                var compilation = new Compilation(syntaxTree);
                var result = compilation.Evaluate(variables, functions);

                var diagnostics = result.Diagnostics;

//===================================================================================
//                          Tree Print
//===================================================================================
                if (showTree){
            
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    TreePrint(syntaxTree.Root);
                    Console.ResetColor();
            
                }
                
                if (!diagnostics.Any())
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine(result.Value);
                    Console.ResetColor();
                }
                else
                {

//===================================================================================
//                       ERROR PRINTING
//===================================================================================
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

        static void PrintHULK()
        {
            Console.WriteLine("                                          .                                     ");
            Console.WriteLine("                                          .:****++==--::. :-:                    ");
            Console.WriteLine("                            .:-==++****************************+=:.              ");
            Console.WriteLine("                           .--=+*************************************=-          ");
            Console.WriteLine("                       .-+**********************************************+:       ");
            Console.WriteLine("                       .-=*************************************************=::   ");
            Console.WriteLine("                     :+*******************************************************-=:");
            Console.WriteLine("                      =********************************************************+:");
            Console.WriteLine("                      :*********************************************************.");
            Console.WriteLine("                      .********************************************************* ");
            Console.WriteLine("                       ************+***********+******************************** ");
            Console.WriteLine("                       +****-=**+:  +*******+-.  .=+-********+-.-******-:******+ ");
            Console.WriteLine("                       =****: ..     =***+-.         :****+:      =**-   .*****+ ");
            Console.WriteLine("                        ****: .:      --.             :=:                 -****= ");
            Console.WriteLine("                         +*+ .+                                         =  ***=  ");
            Console.WriteLine("                         :*: .*-.                                       == -*=   ");
            Console.WriteLine("                         .*:   :+**=:                               :==++.  *-   ");
            Console.WriteLine("                          +.     -****=.                        .-+***-.    +-   ");
            Console.WriteLine("                                  :**.**=.       :    :       .+*=***.      :.   ");
            Console.WriteLine("                                   +**=+**=.    -:    +     .=**=-**=            ");
            Console.WriteLine("                                    .:::::-=+++=: .::..=+==+++==++=:             ");
            Console.WriteLine("                            :                    :...::                          ");
            Console.WriteLine("                            :=:.            :.-:       .:..               ::     ");
            Console.WriteLine("                              .:-===-.   :-. =.          =.:::    ...:----:      ");
            Console.WriteLine("                                 ::.  .-:    :===:    -=-=.   -:   :=:           ");
            Console.WriteLine("                                -.  --.         :=++++=:        :-:  .-          ");
            Console.WriteLine("                               =  .-                              .-.  -.        ");
            Console.WriteLine("                               . -..--:-+-::.                .:-::: :-  -        ");
            Console.WriteLine("                                :.--    =   .--...:=-....=-:. :-   +=.- :        ");
            Console.WriteLine("                                :.***+=:=-               -    -  :+**-:.         ");
            Console.WriteLine("                                ..+*-:-****+++*=---------+-==+***=-+*=:          ");
            Console.WriteLine("                                  ==  .**************************:.-*:.          ");
            Console.WriteLine("                                  =+..:**************************...+-           ");
            Console.WriteLine("                                  +- .:+************************+   ==           ");
            Console.WriteLine("                                  ==-.  :**********************-..:.==           ");
            Console.WriteLine("                                   ++   :.:+*****************==.   =*:           ");
            Console.WriteLine("                                    -= :=   - ..+::=-::+:::=   =  :*:            ");
            Console.WriteLine("                                      :+*.  -   =..-:  =   :   *+-:              ");
            Console.WriteLine("                                         ..:::::.......::..-:.:.                 ");        
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