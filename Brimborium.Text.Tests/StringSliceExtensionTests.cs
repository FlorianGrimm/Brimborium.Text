namespace Brimborium.Text;

public class StringSliceExtensionTests {
    [Test]
    public async Task AsStringSliceTest() {
        await Assert.That("abc".AsStringSlice().ToString()).IsEqualTo("abc");
        await Assert.That("abc".AsStringSlice(1).ToString()).IsEqualTo("bc");
        await Assert.That("abcd".AsStringSlice(1, 2).ToString()).IsEqualTo("bc");
        await Assert.That("abcd".AsStringSlice(1..2).ToString()).IsEqualTo("b");
    }

    [Test]
    public async Task ReadWhileMatchesTest() {
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 1, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("a");
            await Assert.That(slice.ToString()).IsEqualTo("abccc");
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 2, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aa");
            await Assert.That(slice.ToString()).IsEqualTo("bccc");
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 3, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 4, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 1, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("a");
            await Assert.That(slice.ToString()).IsEqualTo("abccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 2, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aa");
            await Assert.That(slice.ToString()).IsEqualTo("bccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 3, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            await Assert.That(matches.ReadWhileMatches(ref slice, 4, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
    }

    [Test]
    public async Task ReadWhileNotMatchesTest() {
        {
            var slice = "abccc".AsStringSlice();
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 1, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("a");
            await Assert.That(slice.ToString()).IsEqualTo("bccc");
        }
        {
            var slice = "abccc".AsStringSlice();
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 2, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("ab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 3, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 4, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 1, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("a");
            await Assert.That(slice.ToString()).IsEqualTo("abccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 2, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aa");
            await Assert.That(slice.ToString()).IsEqualTo("bccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 3, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            await Assert.That(matches.ReadWhileNotMatches(ref slice, 4, out var match)).IsTrue();
            await Assert.That(match.ToString()).IsEqualTo("aab");
            await Assert.That(slice.ToString()).IsEqualTo("ccc");
        }
    }

    private static readonly char[] _ArrCharWhitespace = new char[] { ' ', '\t', '\n', '\r' };
    private static readonly char[] _ArrCharDoubleQuote = new char[] { '"' };
    private static readonly char[] _ArrCharEqual = new char[] { '=' };
    private static readonly char[] _ArrCharNameEndNotQuoted = new char[] { ' ', '\t', '\n', '\r', '=' };
    private static readonly char[] _ArrCharNameEndDoubleQuoted = new char[] { '"' };
    private static readonly char[] _ArrCharValueEndNotQuoted = new char[] { ' ', '\t', '\n', '\r' };
    private static readonly char[] _ArrCharValueEndDoubleQuoted = new char[] { '"' };

    [Test]
    public async Task ReadWhileMatchesComplexTest() {
        // "([._A-Za-z0-9]+)(?:\\W*)(?:[=])(?:\\W*)(?:[\"])([^\"]*)(?:[\"])"
        var ssParameter = "abc=\"def\" ghi=123".AsStringSlice();
        var act = ParseParameter(ssParameter);
        await Assert.That(act.ListParameter.Count).IsEqualTo(2);
        await Assert.That(act.ListParameter[0].Name.ToString()).IsEqualTo("abc");
        await Assert.That(act.ListParameter[0].Value.ToString()).IsEqualTo("def");
        await Assert.That(act.ListParameter[1].Name.ToString()).IsEqualTo("ghi");
        await Assert.That(act.ListParameter[1].Value.ToString()).IsEqualTo("123");

        static ParseParameterResult ParseParameter(StringSlice ssParameter) {
            List<Parameter> result = new();
            var ssCurrent = ssParameter;
            ssCurrent = ssCurrent.TrimStart(_ArrCharWhitespace);
            if (ssCurrent.IsEmpty) {
                return new(result, ssParameter);
            }
            while (!ssCurrent.IsEmpty) {
                bool nameIsQuoted = (_ArrCharDoubleQuote.ReadWhileMatches(ref ssCurrent, 1, out var _));
                if (!(nameIsQuoted ? _ArrCharNameEndDoubleQuoted : _ArrCharNameEndNotQuoted)
                        .ReadWhileNotMatches(ref ssCurrent, 256, out var ssName)) {
                    return new(result, ssParameter);
                }
                if (nameIsQuoted) {
                    _ArrCharDoubleQuote.ReadWhileMatches(ref ssCurrent, 1, out var _);
                }

                ssCurrent = ssCurrent.TrimStart(_ArrCharWhitespace);
                if (!(_ArrCharEqual.ReadWhileMatches(ref ssCurrent, 1, out var _))) {
                    return new(result, ssParameter);
                }
                ssCurrent = ssCurrent.TrimStart(_ArrCharWhitespace);
                bool valueIsQuoted = (_ArrCharDoubleQuote.ReadWhileMatches(ref ssCurrent, 1, out var _));
                if (!(valueIsQuoted ? _ArrCharValueEndDoubleQuoted : _ArrCharValueEndNotQuoted)
                        .ReadWhileNotMatches(ref ssCurrent, 256, out var ssValue)) {
                    return new(result, ssParameter);
                }
                if (valueIsQuoted) {
                    _ArrCharDoubleQuote.ReadWhileMatches(ref ssCurrent, 1, out var _);
                }
                result.Add(new Parameter(ssName, ssValue));
                ssCurrent = ssCurrent.TrimStart(_ArrCharWhitespace);
            }
            return new(result, ssParameter);
        }
    }

    public record struct ParseParameterResult(
        List<Parameter> ListParameter,
        StringSlice Remainder
        );

    public record class Parameter(StringSlice Name, StringSlice Value);
}