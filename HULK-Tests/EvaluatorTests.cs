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
        
        [InlineData("1;", 1.0)]
        [InlineData("-1;", -1.0)]
        [InlineData("+1;", 1.0)]
        
        [InlineData("\"Hello World\";", "Hello World")]
        
        [InlineData("1 + 2;", 3.0)]
        [InlineData("1 * 2;", 2.0)]
        [InlineData("8 / 2;", 4.0)]
        [InlineData("2 ^ 2;", 4.0)]
        [InlineData("8 % 2;", 0.0)]
        [InlineData("1 % 2;", 1.0)]
        [InlineData("1 - 2;", -1.0)]
        
        [InlineData("\"Tres\" @ 2;", "Tres2")]
        
        [InlineData("let a = 1 in a;", 1.0)]
        [InlineData("let a = 1 in  a + 1;", 2.0)]
        [InlineData("let a = 1 in a * 2;", 2.0)]
        [InlineData("let a = \"Yes\" in a;", "Yes")]
        [InlineData("let a = \"Yes\" in a @ \"No\" @ 2345 ;", "YesNo2345")]
        
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

        [InlineData("let x = 5 in (let x = 10 in x + x);", 20.0)]

        [InlineData("if(true) 1 else 2;",1.0)]
        [InlineData("if(false) 1 else 2;",2.0)]
        [InlineData("if(true == true) 1 else 2;",1.0)]
        [InlineData("if(true != false) 1 else 2;",1.0)]
        [InlineData("if(2*2^2 == 2^2*2) 1 else 2;",1.0)]
        [InlineData("if( (let a = 10 in a * 10)%2 == 0 )  1 else 2;",1.0)]
        [InlineData("let a = if( (let a = 10 in a * 10)%2 == 0 )  1 else 2 in a * 5;",5.0)]


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
