using System;
using System.Collections;
using System.Collections.Generic;
using HULK.CodeAnalysis;
using HULK.CodeAnalysis.Syntax;
using Xunit;

namespace HULK_Tests.CodeAnalysis.Syntax
{   
    public class EvaluatorTests
    {
        [Theory]
        //TODO: Add more tests
        
        [InlineData("1;", 1)]
        [InlineData("-1;", -1)]
        [InlineData("+1;", 1)]
        
        [InlineData("\"Hello World\";", "Hello World")]
        
        [InlineData("1 + 2;", 3)]
        [InlineData("1 * 2;", 2)]
        [InlineData("8 / 2;", 4)]
        [InlineData("2 ^ 2;", 4)]
        [InlineData("8 % 2;", 0)]
        [InlineData("1 % 2;", 1)]
        [InlineData("1 - 2;", -1)]
        
        [InlineData("\"Tres\" @ 2;", "Tres2")]
        
        [InlineData("a = 1;", 1)]
        [InlineData("(a = 1) + 1;", 2)]
        [InlineData("(a = 1) * 2;", 2)]
        [InlineData("a = \"Yes\";", "Yes")]
        [InlineData("a = \"Yes\" @ \"No\" @ 2345 ;", "YesNo2345")]
        
        [InlineData("1 == 1;", true)]
        [InlineData("2 == 1;", false)]
        [InlineData(" 2 * 1 == 2;", true)]
        [InlineData("3 + 5 == 1;", false)]
        [InlineData("1 != 1;", false)]

        [InlineData("true;", true)]
        [InlineData("false;", false)]
        [InlineData("!true;", false)]
        [InlineData("!false;", true)]        
        [InlineData("true == true;", true)]
        [InlineData("false == false;", true)]
        [InlineData("!true == false;", true)]
        [InlineData("!true & true == false;", false)]
        [InlineData("false | false;", false)]
        [InlineData("true | false;", true)]

        public void SyntaxFactGetTextRoundTrips(string text, object expectedValue)
        {
            var expression = SyntaxTree.Parse(text);
            var compilation = new Compilation(expression);
            var variables = new Dictionary<VariableSymbol, object>();
            var actualResult = compilation.Evaluate(variables);

            Assert.Empty(actualResult.Diagnostics);
            Assert.Equal(expectedValue, actualResult.Value);
        }
    }
}
