#pragma warning disable IDE0130 // Namespace does not match folder structure
#pragma warning disable IDE0059 // Unnecessary assignment of a value

using Brimborium.Text;

namespace Brimborium.Gerede;

public class BGTokenizerTests {
    [Test]
    public async Task AcceptEOF_Empty_Test() {
        var tokenizer = BGTokenizer.AcceptEOF();
        var result = tokenizer.TryGetToken(new StringRange(string.Empty), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptEOF_NonEmpty_Test() {
        var tokenizer = BGTokenizer.AcceptEOF();
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("abc");
    }

    [Test]
    public async Task AcceptChar_Matches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange("bxyz"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task AcceptChar_NotMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange("zxy"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("zxy");
    }

    [Test]
    public async Task AcceptChar_Empty_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange(string.Empty), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Or_FirstAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or(
            [
                BGTokenizer.AcceptChar("a"),
                BGTokenizer.AcceptChar("b"),
            ]);
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Or_SecondAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or([
            BGTokenizer.AcceptChar("x"),
            BGTokenizer.AcceptChar("a"),
        ]);
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Or_NoAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or([
            BGTokenizer.AcceptChar("x"),
            BGTokenizer.AcceptChar("y"),
        ]);
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("abc");
    }

    [Test]
    public async Task Repeat_ZeroOrMore_NoMatch_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").TRepeat(0, 5);
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Repeat_AtLeastOne_WithMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").TRepeat(1, 10);
        var result = tokenizer.TryGetToken(new StringRange("aaabc"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Repeat_AtLeastOne_NoMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").TRepeat(1, 10);
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Repeat_RespectsMaxRepeat_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").TRepeat(0, 2);
        var result = tokenizer.TryGetToken(new StringRange("aaaaa"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("aaa");
    }

    [Test]
    public async Task Capture_Matches_ProducesGenericToken_Test() {
        IBGTokenizer<BGVoid> inner = BGTokenizer.AcceptChar("a").TRepeat(1, 10);
        var tokenizer = inner.TCapture(new BGTokenizerResultTransformMatchAsString<BGVoid>());
        var result = tokenizer.TryGetToken(new StringRange("aaabc"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Match.ToString()).IsEqualTo("aaa");
        await Assert.That(token.Value).IsEqualTo("aaa");
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Capture_NotMatches_Test() {
        IBGTokenizer<BGVoid> inner = BGTokenizer.AcceptChar("a").TRepeat(1, 10);
        var tokenizer = inner.TCapture(new BGTokenizerResultTransformMatchAsString<BGVoid>());
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    private (bool result, string n) TokenizerRun<T>(IBGTokenizer<T> tokenizer, string input) {
        if (tokenizer.TryGetToken(new StringRange(input), out var token, out var next)) {
            return (true, next.ToString());
        } else {
            return (false, next.ToString());
        }
    }

    [Test]
    public async Task TokenizerMacroComment() {
        // /* #Macro:abc */
        var tokenWhitespace = BGTokenizer.AcceptChar(" /t\r\n").TRepeat(0, 1024);
        var tokenMultiCommentStart =
            BGTokenizer.AcceptChar("/")
            .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
            .Then(tokenWhitespace)
            .Returns((_, _, _, _) => new BGVoid());
        var tokenPrefixMacro = BGTokenizer.AcceptString("#Macro:");
        var tokenIdentifierStart = BGTokenizer.AcceptChar("ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz");
        var tokenIdentifierRest = BGTokenizer.AcceptChar("_ABCDEFGHIJKLMNOPQRSTUWXYZabcdefghijklmnopqrstuvwxyz0123456789");
        var tokenIdentifier = tokenIdentifierStart
            .Then(tokenIdentifierRest.TRepeat(0, 255))
            .Returns((_, _, _) => new BGVoid());
        var tokenName = BGTokenizer.AcceptString("#Macro:");
        var tokenMultiCommentEnd =
            tokenWhitespace
            .Then(BGTokenizer.AcceptChar("*").TRepeat(1, 10))
            .Then(BGTokenizer.AcceptChar("/"))
            .Returns((_, _, _, _) => new BGVoid());

        await Assert.That(TokenizerRun(tokenWhitespace, "  /t$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentStart, "/*$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentStart, "/**$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentStart, "abc$")).IsEquivalentTo((false, "abc$"));
        await Assert.That(TokenizerRun(tokenPrefixMacro, "#Macro:$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenPrefixMacro, "#define$")).IsEquivalentTo((false, "#define$"));
        await Assert.That(TokenizerRun(tokenIdentifier, "abc$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenIdentifier, "#abc$")).IsEquivalentTo((false, "#abc$"));
        await Assert.That(TokenizerRun(tokenMultiCommentEnd, "*/$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentEnd, "**/$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentEnd, " **/$")).IsEquivalentTo((true, "$"));
        await Assert.That(TokenizerRun(tokenMultiCommentEnd, "abc*/$")).IsEquivalentTo((false, "abc*/$"));
    }
}

internal sealed class CharSelector : IBGTokenizerResultTransform<BGVoid, char> {    
    public char Select(StringRange match) => match.AsSpan()[0];

    public char Select(BGToken<BGVoid> value1, StringRange match)
        => match.AsSpan()[0];
}

internal sealed class CountRepeatSelector<T> : IBGTokenizerResultRepeat<T, int> {
    public int Select(IReadOnlyList<BGToken<T>> items, StringRange match) => items.Count;
}

internal sealed class CharPairNextSelector : IBGTokenizerResultTransform<char, char, string> {
    public string Select(BGToken<char> first, BGToken<char> next, StringRange match)
        => $"{first.Value}{next.Value}";
}
