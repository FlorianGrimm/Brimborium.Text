namespace Brimborium.Text;

public class StringSliceTests {
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
            Assert.Equal(5, sut.IndexOf('f', 1..^0));
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
}
