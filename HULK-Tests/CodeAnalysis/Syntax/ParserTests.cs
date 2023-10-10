using System.Collections;
using System.Collections.Generic;
using HULK.CodeAnalysis.Syntax;
using Xunit;

namespace HULK_Tests.CodeAnalysis.Syntax
{
    internal sealed class AssertingEnumerator
    {
        public ParserTests(Parameters)
        {
            
        }
    }

    public class ParserTests
    {
        [Theory]
        [MemberData(nameof(GetBinaryOperatorPairsData))]
        public void ParserBinaryExpressionHonorsPrecedence(SyntaxKind op1, SyntaxKind op2)
        {
            var op1Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op1);
            var op2Precedence = SyntaxFacts.GetBinaryOperatorPrecedence(op2);
            
            var op1Text = SyntaxFacts.GetText(op1);
            var op2Text = SyntaxFacts.GetText(op2);
            
            var text = $"a {op1Text} b {op2Text} c";

            if(op1Precedence > op2Precedence)
            {
                //       op2
                //      /   \
                //    op1    c
                //   /  \
                //  a    b
                Assert.False(true);
            }
            else
            {
                //       op1
                //      /  \
                //    a    op2
                //        /  \
                //       b    c
                Assert.False(true);
            }
        }

        private static IEnumerable<object[]> GetBinaryOperatorPairsData()
        {
            foreach (var op1 in SyntaxFacts.GetUnaryOperatorKinds())
                foreach (var op2 in SyntaxFacts.GetBinaryOperatorKinds())
                    yield return new object[] { op1, op2 }; 
        }
    }
}
