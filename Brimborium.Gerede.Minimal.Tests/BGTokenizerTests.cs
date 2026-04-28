using Brimborium.Text;

namespace Brimborium.Gerede;

public class BGTokenizerTests {
    [Test]
    public async Task AcceptEOF_Empty_Test() {
        var tokenizer = BGTokenizer.AcceptEOF();
        var result = tokenizer.TryGetToken(new StringRange(string.Empty), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptEOF_NonEmpty_Test() {
        var tokenizer = BGTokenizer.AcceptEOF();
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("abc");
    }

    [Test]
    public async Task AcceptChar_Matches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange("bxyz"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task AcceptChar_NotMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange("zxy"), out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("zxy");
    }

    [Test]
    public async Task AcceptChar_Empty_Test() {
        var tokenizer = BGTokenizer.AcceptChar("abc");
        var result = tokenizer.TryGetToken(new StringRange(string.Empty), out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Or_FirstAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or(new IBGTokenizer[] {
            BGTokenizer.AcceptChar("a"),
            BGTokenizer.AcceptChar("b"),
        });
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Or_SecondAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or(new IBGTokenizer[] {
            BGTokenizer.AcceptChar("x"),
            BGTokenizer.AcceptChar("a"),
        });
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Or_NoAlternativeMatches_Test() {
        var tokenizer = BGTokenizer.Or(new IBGTokenizer[] {
            BGTokenizer.AcceptChar("x"),
            BGTokenizer.AcceptChar("y"),
        });
        var result = tokenizer.TryGetToken(new StringRange("abc"), out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("abc");
    }

    [Test]
    public async Task Repeat_ZeroOrMore_NoMatch_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").Repeat(0, 5);
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Repeat_AtLeastOne_WithMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").Repeat(1, 10);
        var result = tokenizer.TryGetToken(new StringRange("aaabc"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Repeat_AtLeastOne_NoMatches_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").Repeat(1, 10);
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task Repeat_RespectsMaxRepeat_Test() {
        var tokenizer = BGTokenizer.AcceptChar("a").Repeat(0, 2);
        var result = tokenizer.TryGetToken(new StringRange("aaaaa"), out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(next.ToString()).IsEqualTo("aaa");
    }

    [Test]
    public async Task Capture_Matches_ProducesGenericToken_Test() {
        IBGTokenizer inner = BGTokenizer.AcceptChar("a").Repeat(1, 10);
        var tokenizer = inner.Capture(new StringSelector());
        var result = tokenizer.TryGetToken(new StringRange("aaabc"), out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Match.ToString()).IsEqualTo("aaa");
        await Assert.That(token.Value).IsEqualTo("aaa");
        await Assert.That(next.ToString()).IsEqualTo("bc");
    }

    [Test]
    public async Task Capture_NotMatches_Test() {
        IBGTokenizer inner = BGTokenizer.AcceptChar("a").Repeat(1, 10);
        var tokenizer = inner.Capture(new StringSelector());
        var result = tokenizer.TryGetToken(new StringRange("xyz"), out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEqualTo("xyz");
    }
}

internal sealed class StringSelector : IBGTokenizerResultAccept<string> {
    public string Select(StringRange match) => match.ToString();
}

internal sealed class CharSelector : IBGTokenizerResultAccept<char> {
    public char Select(StringRange match) => match.AsSpan()[0];
}

internal sealed class CountRepeatSelector<T> : IBGTokenizerResultRepeat<T, int> {
    public int Select(IReadOnlyList<BGToken<T>> items, StringRange match) => items.Count;
}

internal sealed class CharPairNextSelector : IBGTokenizerResultNext<char, char, string> {
    public string Select(BGToken<char> first, BGToken<char> next, StringRange match)
        => $"{first.Value}{next.Value}";
}
