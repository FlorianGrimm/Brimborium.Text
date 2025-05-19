namespace Brimborium.Text;

public class StringSliceTests {
    [Fact]
    public void Struct_Ctor_Empty() {
        StringSlice act=new();
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

            Assert.Throws<ArgumentOutOfRangeException> (() => {
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
            Assert.Throws<ArgumentOutOfRangeException>(() => {
                abc.Left(efg);
            });
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
    public void GetOffsetAndLength_ReturnsCorrectValues()
    {
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
