namespace Brimborium.Text;

public class StringSliceExtensionTests {
    [Fact()]
    public void AsStringSliceTest() {
        Assert.Equal("abc", "abc".AsStringSlice().ToString());
        Assert.Equal("bc", "abc".AsStringSlice(1).ToString());
        Assert.Equal("bc", "abcd".AsStringSlice(1, 2).ToString());
        Assert.Equal("b", "abcd".AsStringSlice(1..2).ToString());
    }

    [Fact]
    public void ReadWhileMatchesTest() {
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 1, out var match));
            Assert.Equal("a", match.ToString());
            Assert.Equal("abccc", slice.ToString());
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 2, out var match));
            Assert.Equal("aa", match.ToString());
            Assert.Equal("bccc", slice.ToString());
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 3, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 4, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 1, out var match));
            Assert.Equal("a", match.ToString());
            Assert.Equal("abccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 2, out var match));
            Assert.Equal("aa", match.ToString());
            Assert.Equal("bccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 3, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'a', 'b' };
            Assert.True(matches.ReadWhileMatches(ref slice, 4, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
    }

    [Fact]
    public void ReadWhileNotMatchesTest() {
        {
            var slice = "abccc".AsStringSlice();
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 1, out var match));
            Assert.Equal("a", match.ToString());
            Assert.Equal("bccc", slice.ToString());
        }
        {
            var slice = "abccc".AsStringSlice();
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 2, out var match));
            Assert.Equal("ab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 3, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "aabccc".AsStringSlice();
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 4, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 1, out var match));
            Assert.Equal("a", match.ToString());
            Assert.Equal("abccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 2, out var match));
            Assert.Equal("aa", match.ToString());
            Assert.Equal("bccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 3, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
        {
            var slice = "123456aabccc".AsStringSlice().Substring(6);
            var matches = new char[] { 'c' };
            Assert.True(matches.ReadWhileNotMatches(ref slice, 4, out var match));
            Assert.Equal("aab", match.ToString());
            Assert.Equal("ccc", slice.ToString());
        }
    }

    private static readonly char[] _ArrCharWhitespace = new char[] { ' ', '\t', '\n', '\r' };
    private static readonly char[] _ArrCharDoubleQuote = new char[] { '"' };
    private static readonly char[] _ArrCharEqual = new char[] { '=' };
    private static readonly char[] _ArrCharNameEndNotQuoted = new char[] { ' ', '\t', '\n', '\r', '=' };
    private static readonly char[] _ArrCharNameEndDoubleQuoted = new char[] { '"' };
    private static readonly char[] _ArrCharValueEndNotQuoted = new char[] { ' ', '\t', '\n', '\r' };
    private static readonly char[] _ArrCharValueEndDoubleQuoted = new char[] { '"' };

    [Fact]
    public void ReadWhileMatchesComplexTest() {
        // "([._A-Za-z0-9]+)(?:\\W*)(?:[=])(?:\\W*)(?:[\"])([^\"]*)(?:[\"])"
        var ssParameter = "abc=\"def\" ghi=123".AsStringSlice();
        var act = ParseParameter(ssParameter);
        Assert.Equal(2, act.ListParameter.Count);
        Assert.Equal("abc", act.ListParameter[0].Name.ToString());
        Assert.Equal("def", act.ListParameter[0].Value.ToString());
        Assert.Equal("ghi", act.ListParameter[1].Name.ToString());
        Assert.Equal("123", act.ListParameter[1].Value.ToString());

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
