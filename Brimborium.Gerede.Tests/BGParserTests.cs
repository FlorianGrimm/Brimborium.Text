using Brimborium.Text;

namespace Brimborium.Gerede;

public class BGParserTests {

    private BGParserInput CreateInput(string text) {
        return new BGParserInput(new StringRange(text), new BGTokenList());
    }

    [Test]
    public async Task Token_Matches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc").Capture(new CharSelector());
        var parser = BGParser.Token(tokenizer);

        var input = CreateInput("bxyz");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo('b');
        await Assert.That(match.Match.ToString()).IsEqualTo("b");
        await Assert.That(next.Input.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Token_NotMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc").Capture(new CharSelector());
        var parser = BGParser.Token(tokenizer);

        var input = CreateInput("xyz");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsFalse();
        await Assert.That(error.Message).IsNotEmpty();
        await Assert.That(next.Input.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Or_FirstMatches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Or(p2);
        var input = CreateInput("axy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo('a');
        await Assert.That(next.Input.ToString()).IsEqualTo("xy");
    }

    [Test]
    public async Task Or_SecondMatches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Or(p2);
        var input = CreateInput("bxy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo('b');
        await Assert.That(next.Input.ToString()).IsEqualTo("xy");
    }

    [Test]
    public async Task Or_NoneMatches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Or(p2);
        var input = CreateInput("cxy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsFalse();
        await Assert.That(error.Message).IsNotEmpty();
        await Assert.That(next.Input.ToString()).IsEqualTo("cxy");
    }

    [Test]
    public async Task Next_Matches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Next(p2, new CharPairParserSelector());
        var input = CreateInput("abxy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo("ab");
        await Assert.That(match.Match.ToString()).IsEqualTo("ab");
        await Assert.That(next.Input.ToString()).IsEqualTo("xy");
    }

    [Test]
    public async Task Next_FirstFails_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Next(p2, new CharPairParserSelector());
        var input = CreateInput("cbxy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("cbxy");
    }

    [Test]
    public async Task Next_SecondFails_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var p2 = BGParser.Token(BGTokenizer.AcceptChar("b").Capture(new CharSelector()));

        var parser = p1.Next(p2, new CharPairParserSelector());
        var input = CreateInput("acxy");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("acxy");
    }

    [Test]
    public async Task Repeat_ZeroOrMore_NoMatch_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var parser = p1.Repeat(0, 5, new CountParserSelector<char>());

        var input = CreateInput("xyz");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo(0);
        await Assert.That(next.Input.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Repeat_AtLeastOne_WithMatches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var parser = p1.Repeat(1, 10, new CountParserSelector<char>());

        var input = CreateInput("aaabc");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsTrue();
        await Assert.That(match.Value).IsEqualTo(3);
        await Assert.That(match.Match.ToString()).IsEqualTo("aaa");
        await Assert.That(next.Input.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Repeat_AtLeastOne_NoMatches_Test() {
        var p1 = BGParser.Token(BGTokenizer.AcceptChar("a").Capture(new CharSelector()));
        var parser = p1.Repeat(1, 10, new CountParserSelector<char>());

        var input = CreateInput("xyz");
        var result = parser.Parse(input, out var match, out var error, out var next);

        await Assert.That(result).IsFalse();
        await Assert.That(error.Message).IsNotEmpty();
        await Assert.That(next.Input.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task ParserMacroComment() {
        // /* #Macro:abc */
        var tokenWhitespace = BGTokenizer.AcceptChar(" /t\r\n").Repeat(0, 1024);
        var tokenMultiCommentStart =
            BGTokenizer.AcceptChar("/")
            .Next(BGTokenizer.AcceptChar("*").Repeat(1, 10))
            .Next(tokenWhitespace);
        var tokenPrefixMacro = BGTokenizer.AcceptString("#Macro:");
        var tokenIdentifierStart = BGTokenizer.AcceptChar((new BGCharRange('A', 'Z') + new BGCharRange('A', 'Z')).Build());
        var tokenIdentifierRest = BGTokenizer.AcceptChar("_ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        var tokenIdentifier = tokenIdentifierStart
            .Next(tokenIdentifierRest.Repeat(0, 255));
        var tokenName = BGTokenizer.AcceptString("#Macro:");
        var tokenMultiCommentEnd =
            tokenWhitespace
            .Next(BGTokenizer.AcceptChar("*").Repeat(1, 10))
            .Next(BGTokenizer.AcceptChar("/"))
            ;


        var parser = BGParser.Token(tokenMultiCommentStart)
            .Next(BGParser.Token(tokenPrefixMacro))
            .Next(BGParser.Token(tokenIdentifier.Capture(new BGTokenizerResultAcceptString())))
            .Next(BGParser.Token(tokenMultiCommentEnd))
            ;

        await Assert.That(ParserRun(parser, "/* #Macro:abc */$")).IsEquivalentTo((true, "/* #Macro:abc */", "abc", "$"));

    }

    private (bool result, string matchText, T? matchValue, string next) ParserRun<T>(IBGParser<T> parser, string input) {
        var result = parser.Parse(
            new BGParserInput(new StringRange(input), new BGTokenList()),
            out var match,
            out var error,
            out var next);
        return (result, match.Match.ToString(), match.Value, next.Input.ToString());
    }
}

internal sealed class CharPairParserSelector : IBGParserResultNext<char, char, string> {
    public string Select(BGResult<char> first, BGResult<char> next, StringRange match)
        => $"{first.Value}{next.Value}";
}

internal sealed class CountParserSelector<T> : IBGParserResultRepeat<T, int> {
    public int Select(IReadOnlyList<BGResult<T>> items, StringRange match) => items.Count;
}
