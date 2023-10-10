using System.Collections.Generic;
using System.Linq;
using HULK.CodeAnalysis.Syntax;
using Xunit;

namespace HULK_Tests.CodeAnalysis.Syntax;

public class LexerTest
{
    [Theory]
    [MemberData(nameof(GetTokensData))]
    public void Lexer_Lexes_Tokens(SyntaxKind kind, string text)
    {
        var tokens = SyntaxTree.ParseTokens(text);   
        var token = Assert.Single(tokens);
        Assert.Equal(kind, token.Kind); 
        Assert.Equal(text, token.Text); 
    }

    [Theory]
    [MemberData(nameof(GetTokensPairsData))]
    public void Lexer_Lexes_TokensPairs(SyntaxKind t1kind, string t1text,SyntaxKind t2kind, string t2text)
    {
        var text = t1text + t2text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();   
        Assert.Equal(2, tokens.Length);
        Assert.Equal(tokens[0].Kind, t1kind); 
        Assert.Equal(tokens[0].Text, t1text); 
        Assert.Equal(tokens[1].Kind, t2kind); 
        Assert.Equal(tokens[1].Text, t2text); 
    }

    [Theory]
    [MemberData(nameof(GetTokensPairsWithSeparatorsData))]
    public void Lexer_Lexes_TokensPairsWithSeparators(SyntaxKind t1kind, string t1text, SyntaxKind separatorKind, string separatorText,SyntaxKind t2kind, string t2text)
    {
        var text = t1text + separatorText + t2text;
        var tokens = SyntaxTree.ParseTokens(text).ToArray();   

        Assert.Equal(3, tokens.Length);
        
        Assert.Equal(tokens[0].Kind, t1kind); 
        Assert.Equal(tokens[0].Text, t1text); 

        Assert.Equal(tokens[1].Kind, separatorKind); 
        Assert.Equal(tokens[1].Text, separatorText); 
        
        Assert.Equal(tokens[2].Kind, t2kind); 
        Assert.Equal(tokens[2].Text, t2text); 

    }

    public static IEnumerable<object[]> GetTokensData()
    {
        foreach (var t in GetTokens().Concat(GetSeparators()))
            yield return new object[] { t.kind, t.text };
    }
    public static IEnumerable<object[]> GetTokensPairsData()
    {
        foreach (var t in GetTokensPairs())
            yield return new object[] { t.t1kind, t.t1text, t.t2kind, t.t2text  };
    }
    public static IEnumerable<object[]> GetTokensPairsWithSeparatorsData()
    {
        foreach (var t in GetTokensPairsWithSeparator())
            yield return new object[] { t.t1kind, t.t1text, t.separator, t.separatorText, t.t2kind, t.t2text  };
    }

    private static IEnumerable<(SyntaxKind kind, string text)> GetTokens()
    {
        return new[] 
        {
            (SyntaxKind.PlusToken, "+"),
            (SyntaxKind.MinusToken, "-"),
            (SyntaxKind.StarToken, "*"),
            (SyntaxKind.DivToken, "/"),
            (SyntaxKind.PercentToken, "%"),
            (SyntaxKind.CircumflexToken, "^"),
            (SyntaxKind.AtToken, "@"),
            (SyntaxKind.EqualsToken, "="),
            (SyntaxKind.BangToken, "!"),
            (SyntaxKind.AmpersandToken, "&"),
            (SyntaxKind.PipeToken, "|"),
            (SyntaxKind.EqualEqualToken, "=="),
            (SyntaxKind.BangEqualToken, "!="),
            (SyntaxKind.OpenParenthesisToken, "("),
            (SyntaxKind.CloseParenthesisToken, ")"),

            (SyntaxKind.FalseKeyword, "false"),
            (SyntaxKind.TrueKeyword, "true"),

            (SyntaxKind.IdentifierToken, "asd"),
            (SyntaxKind.IdentifierToken, "a"),
            (SyntaxKind.IdentifierToken, "abc"),
            
            (SyntaxKind.NumberToken, "1"),
            (SyntaxKind.NumberToken, "123"),
            (SyntaxKind.StringToken, "\"asdasdasd\""),
            (SyntaxKind.StringToken, "\"Hello world\""),
        };
    }
    private static IEnumerable<(SyntaxKind kind, string text)> GetSeparators()
    {
        return new[] 
        {
            (SyntaxKind.WitheSpaceToken, " "),
            (SyntaxKind.WitheSpaceToken, "  "),
            (SyntaxKind.WitheSpaceToken, "\r"),
            (SyntaxKind.WitheSpaceToken, "\n"),
            (SyntaxKind.WitheSpaceToken, "\r\n"),
        };
    }

    private static bool RequiresSeparator(SyntaxKind t1Kind, SyntaxKind t2Kind)
    {
        var t1isKeyword = t1Kind.ToString().EndsWith("Keyword");
        var t2isKeyword = t2Kind.ToString().EndsWith("Keyword");
        
        if(t1isKeyword && t2isKeyword)
            return true;

        if(t1isKeyword && t2Kind == SyntaxKind.IdentifierToken)
            return true;

        if(t1Kind == SyntaxKind.IdentifierToken && t2isKeyword)
            return true;

        if(t1Kind == SyntaxKind.IdentifierToken && t2Kind == SyntaxKind.IdentifierToken)
            return true;

        if(t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.IdentifierToken)
            return true;

        if(t1Kind == SyntaxKind.NumberToken && t2Kind == SyntaxKind.NumberToken)
            return true;

        if(t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualsToken)
            return true;

        if(t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualsToken)
            return true;
        
        if(t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualEqualToken)
            return true;

        if(t1Kind == SyntaxKind.BangToken && t2Kind == SyntaxKind.EqualEqualToken)
            return true;

        if(t1Kind == SyntaxKind.EqualsToken && t2Kind == SyntaxKind.EqualEqualToken)
            return true;

        //TODO: 
        //Add more cases
        return false;
    
    }

    private static IEnumerable<(SyntaxKind t1kind, string t1text, SyntaxKind t2kind, string t2text)> GetTokensPairs()
    {
        foreach (var t1 in GetTokens())
        {
            foreach (var t2 in GetTokens())
                if(!RequiresSeparator(t1.kind,t2.kind))
                    yield return (t1.kind, t1.text, t2.kind, t2.text);
        }
    }

    private static IEnumerable<(SyntaxKind t1kind, string t1text, SyntaxKind separator, string separatorText, SyntaxKind t2kind, string t2text)> GetTokensPairsWithSeparator()
    {
        foreach (var t1 in GetTokens())
        {
            foreach (var t2 in GetTokens())
                if(RequiresSeparator(t1.kind,t2.kind))
                {
                    foreach (var s in GetSeparators())
                    yield return (t1.kind, t1.text, s.kind, s.text, t2.kind, t2.text);
                }

        }
    }
}