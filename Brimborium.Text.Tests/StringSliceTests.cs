namespace Brimborium.Text;

public class StringSliceTests {
    [Fact]
    public void Struct_Ctor_Empty() {
        StringSlice act = new();
        Assert.NotNull(act.Text);
    }

    [Fact]
    public void SubString_Ctor() {
        {
            var sut = new StringSlice();
            Assert.Equal("", sut.ToString());
        }
        {
            var sut = new StringSlice("abc");
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new StringSlice("abc", 1..2);
            Assert.Equal("b", sut.ToString());
        }
        {
            var sut = new StringSlice("abcdefg", 1..^1);
            Assert.Equal("bcdef", sut.ToString());
        }
    }

    [Fact()]
    public void EmptyTest() {
        Assert.Equal("", StringSlice.Empty.ToString());
    }

    [Fact()]
    public void RangeTest() {
        {
            var sut = new StringSlice("abcdefg");
            var act = sut[1..3];
            Assert.Equal("bc", act.ToString());
        }

        {
            var sut = new StringSlice("abcdefg", 1..^0);
            var act = sut[1..3];
            Assert.Equal("cd", act.ToString());
        }

        {
            var sut = new StringSlice("abcdefg", 4..^0);
            Assert.Equal("efg", sut.ToString());

            var act = sut[1..^0];
            Assert.Equal("fg", act.ToString());
        }

        {
            var sut = new StringSlice("abcdefg", 4..^0);
            Assert.Equal("efg", sut.ToString());

            var act1 = sut[1..3];
            Assert.Equal("fg", act1.ToString());
        }


        {
            var sut = new StringSlice("abcdefg", 5..^0);
            Assert.Equal("fg", sut.ToString());

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var act1 = sut[1..3];
            });
        }
    }

    [Fact()]
    public void GetSubStringPosTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1);
            Assert.Equal("bcdefg", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1);
            var sut = bcdef.Substring(1);
            Assert.Equal("cdefg", sut.ToString());
        }
        {

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(-1);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(42);
            });
        }
    }

    [Fact()]
    public void GetSubStringPosLengthTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1, 5);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1, 5);
            var sut = bcdef.Substring(1, 3);
            Assert.Equal("cde", sut.ToString());
        }
        {

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(-1, 2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(42, 2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(0, 42);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("abcdefg").Substring(0, -1);
            });

        }
    }

    [Fact()]
    public void GetSubStringRangeTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..6);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1..6);
            var sut = bcdef.Substring(1..4);
            Assert.Equal("cde", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..^1);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1..^1);
            var sut = bcdef.Substring(1..^1);
            Assert.Equal("cde", sut.ToString());
        }
    }

    [Fact]
    public void LeftTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdefg = abcdefg.Substring(1);
            var sut = abcdefg.Left(bcdefg);
            Assert.Equal("a", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdefg = abcdefg.Substring(1);
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                bcdefg.Left(abcdefg);
            });
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var abc = abcdefg.Substring(0, 3);
            var efg = abcdefg.Substring(4, 3);
            var sut = abc.Left(efg);
            Assert.Equal("abcd", sut.ToString());
        }
    }

    [Fact()]
    public void AsSpanTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.AsSpan();
            Assert.Equal("abcdefg", sut.ToString());
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..^1).AsSpan();
            Assert.Equal("bcdef", sut.ToString());
        }
    }

    [Fact()]
    public void IsNullOrEmptyTest() {
        {
            var sut = new StringSlice();
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
        {
            var sut = new StringSlice("abcdefg");
            Assert.Equal(false, sut.IsNullOrEmpty());

            sut = sut.Substring(1..3);
            Assert.Equal(false, sut.IsNullOrEmpty());

            sut = sut.Substring(1..1);
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
        {
            var sut = new StringSlice("abcdefg", new Range(0, 0));
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
    }

    [Fact()]
    public void IsNullOrWhiteSpaceTest() {
        {
            var sut = new StringSlice();
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new StringSlice("a     g");
            Assert.Equal(false, sut.IsNullOrWhiteSpace());

            sut = sut.Substring(1..3);
            Assert.Equal(true, sut.IsNullOrWhiteSpace());

            sut = sut.Substring(1..1);
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new StringSlice("abcdefg", new Range(0, 0));
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new StringSlice(" bcdefg", new Range(0, 3));
            Assert.Equal(false, sut.IsNullOrWhiteSpace());
        }
    }

    [Fact()]
    public void IndexOfTest() {
        {
            var sut = new StringSlice("abcdef");
            Assert.Equal(0, sut.IndexOf('a'));
            Assert.Equal(2, sut.IndexOf('c'));
            Assert.Equal(5, sut.IndexOf('f'));
            Assert.Equal(-1, sut.IndexOf('x'));
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            Assert.Equal(0, sut.IndexOf('a'));
            Assert.Equal(2, sut.IndexOf('c'));
            Assert.Equal(5, sut.IndexOf('f'));
            Assert.Equal(-1, sut.IndexOf('x'));
        }

        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            Assert.Equal(-1, sut.IndexOf('a', 1..^0));
            Assert.Equal(2, sut.IndexOf('c', 1..^0));
            Assert.Equal('c', sut[2]);
            Assert.Equal(5, sut.IndexOf('f', 1..^0));
            Assert.Equal('f', sut[5]);
            Assert.Equal(-1, sut.IndexOf('x', 1..^0));
        }
    }

    [Fact()]
    public void IndexOfAnyTest() {
        {
            var sut = new StringSlice("abcdef");
            Assert.Equal(0, sut.IndexOfAny("a".ToCharArray()));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray()));
            Assert.Equal(5, sut.IndexOfAny("fzk".ToCharArray()));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray()));
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            Assert.Equal(0, sut.IndexOfAny("azk".ToCharArray()));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray()));
            Assert.Equal(5, sut.IndexOfAny("fzk".ToCharArray()));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray()));
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            Assert.Equal(-1, sut.IndexOfAny("azk".ToCharArray(), 1..^0));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray(), 1..^0));
            Assert.Equal(5, sut.IndexOfAny("fzk".ToCharArray(), 1..^0));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray(), 1..^0));

            Assert.Equal(-1, sut.IndexOfAny("azk".ToCharArray(), 1..^1));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray(), 1..^1));
            Assert.Equal('c', sut[2]);
            Assert.Equal(-1, sut.IndexOfAny("fzk".ToCharArray(), 1..^1));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray(), 1..^1));
        }
    }

    [Fact()]
    public void SplitIntoSepTest() {
        {

            var sut = new StringSlice("abc\r\ndef");
            var act = sut.SplitInto("\r\n".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("def", act.Tail.ToString());
        }
    }

    [Fact()]
    public void SplitIntoSepStopTest() {
        {
            var sut = new StringSlice("abc\r\ndef$ghi");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("def", act.Tail.ToString());
        }
    }

    [Fact()]
    public void SplitIntoNoMatchTest() {
        {
            var sut = new StringSlice("abc");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
    }

    [Fact()]
    public void SplitIntoEmptyInputTest() {
        {
            var sut = new StringSlice(string.Empty);
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
    }

    [Fact()]
    public async Task UsingInAsyncTest() {
        var sut = await doSomething(new StringSlice("abcdef"));
        Assert.Equal("bcde", sut.ToString());
        async Task<StringSlice> doSomething(StringSlice subString) {
            await Task.Delay(1);
            return subString.Substring(1..^1);
        }
    }

    [Fact()]
    public void EqualTest() {
        {
            var a = new StringSlice("0123456789", 1..5);
            var b = new StringSlice("1234");

            Assert.Equal("1234", a.ToString());
            Assert.True(a.Equals(b));
        }
        {
            var a = new StringSlice("0123456789");
            var b = new StringSlice("1234");

            Assert.False(a.Equals(b));
        }
        {
            object a = new StringSlice("0123456789");
            var b = new StringSlice("0123456789");
            Assert.True(b.Equals(a));
        }
        {
            object a = 1;
            var b = new StringSlice("0123456789");
            Assert.False(b.Equals(a));
        }
        {
            var sut = new StringSlice("ABC");
            Assert.True(sut.Equals("abc", StringComparison.OrdinalIgnoreCase));
        }
        {
            var sut = new StringSlice("0123456789");
            Assert.True(sut.Equals("0123456789".AsSpan(), StringComparison.Ordinal));
        }
        {
            var s = "1234";
            var a = s.AsStringSlice(1);
            var b = s.AsStringSlice(1);
            Assert.True(a.Equals(b));
        }
    }

    [Fact()]
    public void SplitIntoWhileTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : 1);
            Assert.Equal("0123456789", act.Found.ToString());
            Assert.Equal("ABCDEF", act.Tail.ToString());
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : -1);
            Assert.Equal("0123456789", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
        {
            var sut = new StringSlice("");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : -1);
            Assert.Equal("", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
        {
            var sut = new StringSlice("aaaaaaa");
            var act = sut.SplitIntoWhile((c, _) => c == 'a' ? 0 : -1);
            Assert.Equal("aaaaaaa", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
    }

    [Fact()]
    public void IndexTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            Assert.Equal('0', sut[0]);
            Assert.Equal('F', sut[15]);
        }
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("0123456789ABCDEF");
                var act = sut[16];
            });
        }

        {
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new StringSlice("0123456789ABCDEF");
                var act = sut[-1];
            });
        }
        {
            var txt = "012345";
            var sut = new StringSlice(txt);
            var act = sut[3];
            Assert.Equal('3', act);
        }
        {
            var txt = "012345";
            var sut = new StringSlice(txt).Substring(1).Substring(1);
            var act = sut[1];
            Assert.Equal('3', act);
        }

    }

    [Fact()]
    public void StartsWithTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            Assert.True(sut.StartsWith("0123", StringComparison.Ordinal));
            Assert.False(sut.StartsWith("X", StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            Assert.True(sut.StartsWith("0123".AsSpan(), StringComparison.Ordinal));
            Assert.False(sut.StartsWith("X".AsSpan(), StringComparison.Ordinal));
        }
    }

    [Fact()]
    public void TrimStartTest() {
        {
            var sut = new StringSlice("  0123");
            var act = sut.TrimStart();
            Assert.Equal("0123", act.ToString());
            Assert.Equal(2..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimStart();
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimStart();
            Assert.Equal("", act.ToString());
            Assert.Equal(4..4, act.Range);
        }
    }

    [Fact()]
    public void TrimEndTest() {
        {
            var sut = new StringSlice("  0123  ");
            var act = sut.TrimEnd();
            Assert.Equal("  0123", act.ToString());
            Assert.Equal(0..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimEnd();
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimEnd();
            Assert.Equal("", act.ToString());
            Assert.Equal(0..0, act.Range);
        }

        {
            var sut = new StringSlice("  0123  ");
            var act = sut.TrimEnd(new char[] { ' ' });
            Assert.Equal("  0123", act.ToString());
            Assert.Equal(0..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimEnd(new char[] { ' ' });
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimEnd(new char[] { ' ' });
            Assert.Equal("", act.ToString());
            Assert.Equal(0..0, act.Range);
        }
    }

    [Fact()]
    public void TrimTest() {
        {
            var sut = new StringSlice("  0123    ");
            var act = sut.Trim();
            Assert.Equal("0123", act.ToString());
            Assert.Equal(2..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.Trim();
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.Trim();
            Assert.Equal("", act.ToString());
            Assert.Equal(0..0, act.Range);
        }

        {
            var sut = new StringSlice("  0123  ");
            var act = sut.Trim(new char[] { ' ' });
            Assert.Equal("0123", act.ToString());
            Assert.Equal(2..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.Trim(new char[] { ' ' });
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.Trim(new char[] { ' ' });
            Assert.Equal("", act.ToString());
            Assert.Equal(0..0, act.Range);
        }
    }

    [Fact()]
    public void TrimWhileTest() {
        {
            var sut = new StringSlice("  0123");
            var act = sut.TrimWhile(decide);
            Assert.Equal("0123", act.ToString());
            Assert.Equal(2..6, act.Range);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimWhile(decide);
            Assert.Equal("0123", act.ToString());
            Assert.Equal(0..4, act.Range);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimWhile(decide);
            Assert.Equal("", act.ToString());
            Assert.Equal(4..4, act.Range);
        }
        {
            var sut = new StringSlice("");
            var act = sut.TrimWhile(decide);
            Assert.Equal("", act.ToString());
            Assert.Equal(0..0, act.Range);
        }
        int decide(char value, int _) {
            if (value == ' ') {
                return 0;
            }

            return 1;
        }
    }

    [Fact]
    public void ReplaceTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.Replace('0', 'X');
            Assert.Equal("X123456789ABCDEF", act.ToString());
        }
        {
            var sut = new StringSlice("0023456789ABCD00");
            var act = sut.Replace('0', 'X');
            Assert.Equal("XX23456789ABCDXX", act.ToString());
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.Replace('X', 'Y');
            Assert.Equal("0123456789ABCDEF", act.ToString());
        }
    }

    [Fact]
    public void ReadWhileTest() {
        {
            var sut = new StringSlice("");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            Assert.Equal("", act.ToString());
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            Assert.Equal("0123456789", act.ToString());
        }
        {
            var sut = new StringSlice("-1234 abc");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            Assert.Equal("-1234", act.ToString());
        }
        {
            var sut = new StringSlice("ABCDEF");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            Assert.Equal("", act.ToString());
        }
    }

    [Fact]
    public void AsImmutableStringSlice_ReturnsEquivalentImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        Assert.Equal("Hello", immutableSlice.ToString());
        Assert.Equal(stringSlice.Text, immutableSlice.Text);
        Assert.Equal(stringSlice.Range, immutableSlice.Range);
    }

    [Fact]
    public void AsImmutableStringSlice_WithEmptySlice_ReturnsEmptyImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello", 2..2);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        Assert.Equal("", immutableSlice.ToString());
        Assert.Equal(0, immutableSlice.Length);
        Assert.Equal(2, immutableSlice.Range.Start.Value);
        Assert.Equal(2, immutableSlice.Range.End.Value);
    }

    [Fact]
    public void AsImmutableStringSlice_WithMiddleRange_ReturnsCorrectImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 6..11);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        Assert.Equal("World", immutableSlice.ToString());
        Assert.Equal(5, immutableSlice.Length);
        Assert.Equal(6, immutableSlice.Range.Start.Value);
        Assert.Equal(11, immutableSlice.Range.End.Value);
    }

    [Fact]
    public void AsImmutableStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var stringSlice = new StringSlice(originalText, 0..5);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        Assert.Same(originalText, immutableSlice.Text);  // Verifies same string instance is referenced
    }

    [Fact]
    public void AsImmutableStringSlice_AndExplicitConversion_ReturnEquivalentResults() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var immutableSlice1 = stringSlice.AsImmutableStringSlice();
        var immutableSlice2 = (ImmutableStringSlice)stringSlice;

        // Assert
        Assert.Equal(immutableSlice1.ToString(), immutableSlice2.ToString());
        Assert.Equal(immutableSlice1.Text, immutableSlice2.Text);
        Assert.Equal(immutableSlice1.Range, immutableSlice2.Range);
    }

    [Fact]
    public void AsMutableStringSliceTest() {
        // Arrange
        var original = new StringSlice("Hello World", 0..5);

        // Act
        var mutable = original.AsMutableStringSlice();

        // Assert
        Assert.Equal("Hello", mutable.ToString());
        Assert.Equal(original.Text, mutable.Text);
        Assert.Equal(original.Range, mutable.Range);

        // Verify mutable behavior
        mutable.Range = 6..11;
        Assert.Equal("World", mutable.ToString());
        // Original should remain unchanged
        Assert.Equal("Hello", original.ToString());
    }

    [Fact()]
    public void EndsWithTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            Assert.True(sut.EndsWith("CDEF", StringComparison.Ordinal));
            Assert.False(sut.EndsWith("X", StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            Assert.True(sut.EndsWith("AB", StringComparison.Ordinal));
            Assert.False(sut.EndsWith("ABC", StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("Hello WORLD");
            Assert.True(sut.EndsWith("world", StringComparison.OrdinalIgnoreCase));
            Assert.False(sut.EndsWith("world", StringComparison.Ordinal));
        }
    }

    [Fact()]
    public void EndsWithStringSliceTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var search = new StringSlice("CDEF");
            Assert.True(sut.EndsWith(search, StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            var search = new StringSlice("ABC");
            Assert.False(sut.EndsWith(search, StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("Hello WORLD");
            var search = new StringSlice("WORLD");
            Assert.True(sut.EndsWith(search, StringComparison.Ordinal));
            Assert.True(sut.EndsWith(new StringSlice("world"), StringComparison.OrdinalIgnoreCase));
        }
    }

    [Fact()]
    public void EndsWithSpanTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            Assert.True(sut.EndsWith("CDEF".AsSpan(), StringComparison.Ordinal));
            Assert.False(sut.EndsWith("X".AsSpan(), StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            Assert.True(sut.EndsWith("AB".AsSpan(), StringComparison.Ordinal));
            Assert.False(sut.EndsWith("ABC".AsSpan(), StringComparison.Ordinal));
        }
        {
            var sut = new StringSlice("Hello WORLD");
            Assert.True(sut.EndsWith("world".AsSpan(), StringComparison.OrdinalIgnoreCase));
            Assert.False(sut.EndsWith("world".AsSpan(), StringComparison.Ordinal));
        }
    }

    [Theory]
    [InlineData("Hello World", "World", StringComparison.Ordinal, true)]
    [InlineData("Hello World", "world", StringComparison.Ordinal, false)]
    [InlineData("Hello World", "world", StringComparison.OrdinalIgnoreCase, true)]
    [InlineData("Hello World", "Hello", StringComparison.Ordinal, false)]
    [InlineData("", "", StringComparison.Ordinal, true)]
    [InlineData("A", "", StringComparison.Ordinal, true)]
    [InlineData("", "A", StringComparison.Ordinal, false)]
    public void EndsWithTheory(string input, string search, StringComparison comparison, bool expected) {
        var sut = new StringSlice(input);
        Assert.Equal(expected, sut.EndsWith(search, comparison));
    }

    [Fact]
    public void GetOffsetAndLength_ReturnsCorrectValues() {
        // Arrange
        var slice1 = new StringSlice("Hello World", 2..7);  // "llo W"
        var slice2 = new StringSlice("Test", 0..4);         // "Test"
        var slice3 = new StringSlice("ABC", 1..1);          // ""

        // Act
        var result1 = slice1.GetOffsetAndLength();
        var result2 = slice2.GetOffsetAndLength();
        var result3 = slice3.GetOffsetAndLength();

        // Assert
        Assert.Equal((2, 5), result1);
        Assert.Equal((0, 4), result2);
        Assert.Equal((1, 0), result3);
    }

    [Fact]
    public void TryFind_StringSlice_Test() {
        // Arrange
        var slice = new StringSlice("Hello World!");

        // Act & Assert - Basic search
        {
            var searchFor = new StringSlice("World");
            bool found = slice.TryFind(searchFor, out var result);

            Assert.True(found);
            Assert.Equal("Hello ", result.Before.ToString());
            Assert.Equal("Hello World", result.BeforeAndFound.ToString());
            Assert.Equal("World", result.Found.ToString());
            Assert.Equal("World!", result.FoundAndAfter.ToString());
            Assert.Equal("!", result.After.ToString());
        }

        // Act & Assert - Search with case insensitivity
        {
            var searchFor = new StringSlice("world");
            bool found = slice.TryFind(searchFor, out var result, StringComparison.OrdinalIgnoreCase);

            Assert.True(found);
            Assert.Equal("Hello ", result.Before.ToString());
            Assert.Equal("Hello World", result.BeforeAndFound.ToString());
            Assert.Equal("World", result.Found.ToString());
            Assert.Equal("World!", result.FoundAndAfter.ToString());
            Assert.Equal("!", result.After.ToString());
        }

        // Act & Assert - Search in middle of string
        {
            var searchFor = new StringSlice("lo Wo");
            bool found = slice.TryFind(searchFor, out var result);

            Assert.True(found);
            Assert.Equal("Hel", result.Before.ToString());
            Assert.Equal("Hello Wo", result.BeforeAndFound.ToString());
            Assert.Equal("lo Wo", result.Found.ToString());
            Assert.Equal("lo World!", result.FoundAndAfter.ToString());
            Assert.Equal("rld!", result.After.ToString());
        }

        // Act & Assert - Search not found
        {
            var searchFor = new StringSlice("NotFound");
            bool found = slice.TryFind(searchFor, out var result);

            Assert.False(found);
        }

        // Act & Assert - Search in substring
        {
            var slice2 = slice.Substring(6); // "World!"
            var searchFor = new StringSlice("or");

            bool found = slice2.TryFind(searchFor, out var result);

            Assert.True(found);
            Assert.Equal("W", result.Before.ToString());
            Assert.Equal("or", result.Found.ToString());
            Assert.Equal("ld!", result.After.ToString());
        }
    }

    [Fact]
    public void TryGetFirst_Test() {
        // Arrange & Act & Assert - Non-empty slice
        {
            var slice = new StringSlice("Hello");
            bool success = slice.TryGetFirst(out var firstChar);

            Assert.True(success);
            Assert.Equal('H', firstChar);
        }

        // Arrange & Act & Assert - Empty slice
        {
            var slice = new StringSlice("");
            bool success = slice.TryGetFirst(out var firstChar);

            Assert.False(success);
            Assert.Equal(default, firstChar);
        }

        // Arrange & Act & Assert - Slice with range that makes it empty
        {
            var slice = new StringSlice("Hello", 2..2);
            bool success = slice.TryGetFirst(out var firstChar);

            Assert.False(success);
            Assert.Equal(default, firstChar);
        }

        // Arrange & Act & Assert - Slice with non-zero start
        {
            var slice = new StringSlice("Hello", 2..5);
            bool success = slice.TryGetFirst(out var firstChar);

            Assert.True(success);
            Assert.Equal('l', firstChar);
        }

        // Arrange & Act & Assert - Nested slices
        {
            var original = new StringSlice("Hello World");
            var nested = original.Substring(6..11);
            bool success = nested.TryGetFirst(out var firstChar);

            Assert.True(success);
            Assert.Equal('W', firstChar);
        }


        // Arrange & Act & Assert - while
        {
            var current = new StringSlice("Hello World");
            while (current.TryGetFirst(out var firstChar)) {
                current = current.Substring(1);
            }
            var (offset, length) = current.GetOffsetAndLength();
            Assert.Equal(11, offset);
            Assert.Equal(0, length);
        }
    }

    [Fact]
    public void SubstringBetweenStartAndStart_Test() {
        // Arrange
        var original = new StringSlice("Hello World");
        var other = original.Substring(6); // "World"

        // Act
        var result = original.SubstringBetweenStartAndStart(other);

        // Assert
        Assert.Equal("Hello ", result.ToString());

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndStart(differentText));

        // Test with invalid other slice (offset before this slice)
        Assert.Throws<ArgumentOutOfRangeException>(() => other.SubstringBetweenStartAndStart(original));

        // Test with invalid other slice (offset outside this slice)
        var beyondSlice = new StringSlice("Hello World Extra", 12..17); // "Extra"
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndStart(beyondSlice));
    }

    [Fact]
    public void SubstringBetweenStartAndEnd_Test() {
        // Arrange
        var original = new StringSlice("Hello World");
        var other = original.Substring(6, 5); // "World"

        // Act
        var result = original.SubstringBetweenStartAndEnd(other);

        // Assert
        Assert.Equal("Hello World", result.ToString());

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndEnd(differentText));

        // Test with invalid other slice (end beyond this slice)
        var longText = "Hello World and more text";
        var longSlice = new StringSlice(longText);
        var partSlice = new StringSlice(longText, 0..11); // "Hello World"
        var act = partSlice.SubstringBetweenStartAndEnd(longSlice);
        Assert.Equal("Hello World and more text", act.ToString());
    }

    [Fact]
    public void SubstringBetweenEndAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text)[0..11]; // "Hello World"
        var second = new StringSlice(text)[16..20]; // "Text"

        // Act
        var result = first.SubstringBetweenEndAndStart(second);

        // Assert
        Assert.Equal(" Gap ", result.ToString());

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SubstringBetweenEndAndStart(differentText));

        // Test with invalid other slice (start before this slice's end)
        Assert.Throws<ArgumentOutOfRangeException>(() => second.SubstringBetweenEndAndStart(first));
    }

    [Fact]
    public void SubstringBetweenEndAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 0..20); // "Hello World Gap Text"

        // Act
        var result = first.SubstringBetweenEndAndEnd(second);

        // Assert
        Assert.Equal(" World Gap Text", result.ToString());

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SubstringBetweenEndAndEnd(differentText));

        // Test with invalid other slice (end before this slice's end)
        Assert.Throws<ArgumentOutOfRangeException>(() => second.SubstringBetweenEndAndEnd(first));
    }

    [Fact]
    public void TrySubstringBetweenStartAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..11); // "Hello World"
        var second = new StringSlice(text, 16..20); // "Text"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenStartAndStart(second, out var result);

            Assert.True(success);
            Assert.Equal("Hello World Gap ", result.ToString());
        }

        // Act & Assert - Invalid case (second starts before first)
        {
            bool success = second.TrySubstringBetweenStartAndStart(first, out var result);

            Assert.False(success);
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenStartAndStart(differentText, out _));
        }
    }

    [Fact]
    public void TrySubstringBetweenStartAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 6..11); // "World"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenStartAndEnd(second, out var result);

            Assert.True(success);
            Assert.Equal("Hello World", result.ToString());
        }

        // Act & Assert - Invalid case (first starts after second ends)
        {
            var later = new StringSlice(text, 12..16); // "Gap"
            bool success = later.TrySubstringBetweenStartAndEnd(second, out var result);

            Assert.False(success);
            Assert.Equal("", result.ToString());
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenStartAndEnd(differentText, out _));
        }
    }

    [Fact]
    public void TrySubstringBetweenEndAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 12..15); // "Gap"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenEndAndStart(second, out var result);

            Assert.True(success);
            Assert.Equal(" World ", result.ToString());
        }

        // Act & Assert - Invalid case (first ends after second starts)
        {
            var overlap = new StringSlice(text, 0..13); // "Hello World G"
            bool success = overlap.TrySubstringBetweenEndAndStart(second, out var result);

            Assert.False(success);
            Assert.Equal("", result.ToString());
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenEndAndStart(differentText, out _));
        }
    }

    [Fact]
    public void TrySubstringBetweenEndAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 6..11); // "World"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenEndAndEnd(second, out var result);

            Assert.True(success);
            Assert.Equal(" World", result.ToString());
        }

        // Act & Assert - Invalid case (first ends after second ends)
        {
            var longer = new StringSlice(text, 0..15); // "Hello World Gap"
            bool success = longer.TrySubstringBetweenEndAndEnd(second, out var result);

            Assert.False(success);
            Assert.Equal("", result.ToString());
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenEndAndEnd(differentText, out _));
        }
    }
}

public class StringSliceAsSpanTests {
    [Fact]
    public void AsSpan_WithRange_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(1..4);

        // Assert
        Assert.Equal("ell", span.ToString());
    }

    [Fact]
    public void AsSpan_WithFullRange_ReturnsEntireSlice() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(..);

        // Assert
        Assert.Equal("Hello World", span.ToString());
    }

    [Fact]
    public void AsSpan_WithPartialSlice_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World", 6..11); // "World"

        // Act
        var span = slice.AsSpan(1..3); // "or"

        // Assert
        Assert.Equal("or", span.ToString());
    }

    [Fact]
    public void AsSpan_WithEmptyRange_ReturnsEmptySpan() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(1..1);

        // Assert
        Assert.Equal(0, span.Length);
        Assert.Equal("", span.ToString());
    }

    [Fact]
    public void AsSpan_WithFromEnd_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(^5..^0);

        // Assert
        Assert.Equal("World", span.ToString());
    }

    [Theory]
    [InlineData(0, false, 5, false, "Hello")]
    [InlineData(0, false, 0, false, "")]
    [InlineData(5, true, 0, true, "World")]
    public void AsSpan_WithVariousRanges_ReturnsExpectedResults(int start, bool startFromEnd, int end, bool endFromEnd, string expected) {
        Range range = new Range(new Index(start, startFromEnd), new Index(end, endFromEnd));
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(range);

        // Assert
        Assert.Equal(expected, span.ToString());
    }

    [Theory]
    [InlineData(1, true, 5, false)]
    [InlineData(0, false, 12, false)]
    [InlineData(6, false, 5, false)]
    public void AsSpan_WithInvalidRange_ThrowsArgumentOutOfRangeException(int start, bool startFromEnd, int end, bool endFromEnd) {
        Range range = new Range(new Index(start, startFromEnd), new Index(end, endFromEnd));

        // Arrange
        var slice = new StringSlice("Hello World");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => slice.AsSpan(range));
    }

    [Fact]
    public void AsSpan_WithNestedSlices_MaintainsCorrectOffsets() {
        // Arrange
        var originalSlice = new StringSlice("Hello World", 6..11); // "World"
        var middleSlice = originalSlice.Substring(1..4); // "orl"

        // Act
        var finalSpan = middleSlice.AsSpan(1..2); // "r"

        // Assert
        Assert.Equal("r", finalSpan.ToString());
    }

    [Fact]
    public void SubstringOffset() {
        var orginal = new StringSlice("0123456789ABCDEF");
        Assert.Equal(0, orginal.GetOffsetAndLength().Offset);
        Assert.Equal(8, orginal.Substring(2).Substring(6).GetOffsetAndLength().Offset);
        Assert.Equal(8, orginal.Substring(8).GetOffsetAndLength().Offset);

    }
}

public class StringSliceAdjustTests {
    [Fact]
    public void TryAdjustStartToLeft_Test() {
        // Arrange
        var text = "  Hello World";
        var slice = new StringSlice(text, 2..13); // "Hello World"
        
        // Act - Successfully adjust to include whitespace
        bool success = slice.TryAdjustStartToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        Assert.True(success);
        Assert.Equal("  ", result.Difference.ToString());
        Assert.Equal("  Hello World", result.Value.ToString());
        
        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustStartToLeft(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        Assert.False(nullStop);
        Assert.Equal("Hello World", nullResult.Value.ToString());
        
        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustStartToLeft(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        Assert.False(falseStop);
        Assert.Equal("Hello World", falseResult.Value.ToString());
    }
    
    [Fact]
    public void TryAdjustStartToRight_Test() {
        // Arrange
        var text = "  Hello World";
        var slice = new StringSlice(text, 0..13); // "  Hello World"
        
        // Act - Successfully adjust to exclude whitespace
        bool success = slice.TryAdjustStartToRight(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        Assert.True(success);
        Assert.Equal("  ", result.Difference.ToString());
        Assert.Equal("Hello World", result.Value.ToString());
        
        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustStartToRight(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        Assert.False(nullStop);
        Assert.Equal("  Hello World", nullResult.Value.ToString());
        
        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustStartToRight(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        Assert.False(falseStop);
        Assert.Equal("  Hello World", falseResult.Value.ToString());
    }
    
    [Fact]
    public void TryAdjustEndToLeft_Test() {
        // Arrange
        var text = "Hello World  ";
        var slice = new StringSlice(text, 0..13); // "Hello World  "
        
        // Act - Successfully adjust to exclude trailing whitespace
        bool success = slice.TryAdjustEndToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        Assert.True(success);
        Assert.Equal("  ", result.Difference.ToString());
        Assert.Equal("Hello World", result.Value.ToString());
        
        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustEndToLeft(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        Assert.False(nullStop);
        Assert.Equal("Hello World  ", nullResult.Value.ToString());
        
        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustEndToLeft(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        Assert.False(falseStop);
        Assert.Equal("Hello World  ", falseResult.Value.ToString());
        
        // Arrange - Slice with mixed content
        var mixedSlice = new StringSlice("abc123", 0..6);
        
        // Act - Adjust to exclude digits with conditional logic
        bool mixedSuccess = mixedSlice.TryAdjustEndToLeft(
            ch => {
                if (char.IsDigit(ch)) return true;
                return false;
            }, 
            out var mixedResult);
        
        // Assert
        Assert.True(mixedSuccess);
        Assert.Equal("123", mixedResult.Difference.ToString());
        Assert.Equal("abc", mixedResult.Value.ToString());
    }
    
    [Fact]
    public void TryAdjustEndToRight_Test() {
        // Arrange
        var text = "Hello World";
        var slice = new StringSlice(text, 0..5); // "Hello"
        
        // Act - Successfully adjust to include the space and "World"
        bool success = slice.TryAdjustEndToRight(
            ch => true, 
            out var result);
        
        // Assert
        Assert.True(success);
        Assert.Equal(" World", result.Difference.ToString());
        Assert.Equal("Hello World", result.Value.ToString());
        
        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustEndToRight(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        Assert.False(nullStop);
        Assert.Equal("Hello", nullResult.Value.ToString());
        
        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustEndToRight(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned
        Assert.False(falseStop);
        Assert.Equal("Hello", falseResult.Value.ToString());
        
        // Arrange - Slice with specific condition
        var specificSlice = new StringSlice("Hello World123", 0..11); // "Hello World"
        
        // Act - Adjust to include only digits
        bool specificSuccess = specificSlice.TryAdjustEndToRight(
            ch => char.IsDigit(ch) ? true : false, 
            out var specificResult);
        
        // Assert
        Assert.True(specificSuccess);
        Assert.Equal("123", specificResult.Difference.ToString());
        Assert.Equal("Hello World123", specificResult.Value.ToString());
    }
    
    [Fact]
    public void TryAdjust_WithEmptySlice_ReturnsExpectedResults() {
        // Arrange
        var emptySlice = new StringSlice("");
        
        // Act & Assert - TryAdjustStartToLeft
        {
            bool success = emptySlice.TryAdjustStartToLeft(
                ch => true, 
                out var result);
            
            Assert.False(success);
            Assert.Equal("", result.Value.ToString());
            Assert.Equal("", result.Difference.ToString());
        }
        
        // Act & Assert - TryAdjustStartToRight
        {
            bool success = emptySlice.TryAdjustStartToRight(
                ch => true, 
                out var result);
            
            Assert.False(success);
            Assert.Equal("", result.Value.ToString());
            Assert.Equal("", result.Difference.ToString());
        }
        
        // Act & Assert - TryAdjustEndToLeft
        {
            bool success = emptySlice.TryAdjustEndToLeft(
                ch => true, 
                out var result);
            
            Assert.False(success);
            Assert.Equal("", result.Value.ToString());
            Assert.Equal("", result.Difference.ToString());
        }
        
        // Act & Assert - TryAdjustEndToRight
        {
            bool success = emptySlice.TryAdjustEndToRight(
                ch => true, 
                out var result);
            
            Assert.False(success);
            Assert.Equal("", result.Value.ToString());
            Assert.Equal("", result.Difference.ToString());
        }
    }
    
    [Fact]
    public void TryAdjust_WithNestedSlices_MaintainsCorrectOffsets() {
        // Arrange
        var originalText = "  Hello World  ";
        var originalSlice = new StringSlice(originalText);
        var trimmedSlice = originalSlice.Substring(2..11); // "Hello Wor"
        
        // Act - Adjust start to left
        bool startLeftSuccess = trimmedSlice.TryAdjustStartToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var startLeftResult);
        
        // Assert
        Assert.True(startLeftSuccess);
        Assert.Equal("  ", startLeftResult.Difference.ToString());
        Assert.Equal("  Hello Wor", startLeftResult.Value.ToString());
        Assert.Equal(0, startLeftResult.Value.Range.Start.Value);
        
        // Act - Adjust end to right
        bool endRightSuccess = trimmedSlice.TryAdjustEndToRight(
            ch => ch == 'l' || ch == 'd' || char.IsWhiteSpace(ch) ? true : false, 
            out var endRightResult);
        
        // Assert
        Assert.True(endRightSuccess);
        Assert.Equal("ld  ", endRightResult.Difference.ToString());
        Assert.Equal("Hello World  ", endRightResult.Value.ToString());
        Assert.Equal(15, endRightResult.Value.Range.End.Value);
    }
    
    [Fact]
    public void TryAdjust_WithComplexPredicates_WorksCorrectly() {
        // Arrange
        var text = "12abc34def56";
        var slice = new StringSlice(text, 2..8); // "abc34d"
        
        // Act - Find digits to the left
        bool leftDigitsSuccess = slice.TryAdjustStartToLeft(
            ch => {
                if (char.IsDigit(ch)) return true;
                return false;
            }, 
            out var leftDigitsResult);
        
        // Assert
        Assert.True(leftDigitsSuccess);
        Assert.Equal("12", leftDigitsResult.Difference.ToString());
        Assert.Equal("12abc34d", leftDigitsResult.Value.ToString());
        
        // Act - Find digits to the right
        bool rightDigitsSuccess = slice.TryAdjustEndToRight(
            ch => {
                return (ch == 'e' || ch == 'f' || char.IsDigit(ch));
            }, 
            out var rightDigitsResult);
        
        // Assert
        Assert.True(rightDigitsSuccess);
        Assert.Equal("ef56", rightDigitsResult.Difference.ToString());
        Assert.Equal("abc34def56", rightDigitsResult.Value.ToString());
    }
    
    [Fact]
    public void TryAdjust_WithConditionalStops_WorksCorrectly() {
        // Arrange
        var text = "abc123def456";
        var slice = new StringSlice(text, 3..6); // "123"
        
        // Act - Adjust left until we hit 'b', then stop
        bool leftSuccess = slice.TryAdjustStartToLeft(
            ch => {
                if (ch == 'b') return false; // Stop at 'b'
                return true;                 // Include everything else
            }, 
            out var leftResult);
        
        // Assert
        Assert.True(leftSuccess);
        Assert.Equal("c", leftResult.Difference.ToString());
        Assert.Equal("c123", leftResult.Value.ToString());
        
        // Act - Adjust right until we hit 'e', then abort
        bool rightSuccess = slice.TryAdjustEndToRight(
            ch => {
                if (ch == 'd') return true;  // Include 'd'
                return null;                 // Abort for anything else
            }, 
            out var rightResult);
        
        // Assert
        Assert.False(rightSuccess); // Should return false because we hit null
        Assert.Equal("123d", rightResult.Value.ToString());
    }
}