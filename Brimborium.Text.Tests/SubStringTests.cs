namespace Brimborium.Text;

public class SubStringTests {
    [Fact]
    public void SubString_Ctor() {
        {
            var sut = new SubString();
            Assert.Equal("", sut.Text);
            Assert.Equal("", sut.ToString());
        }
        {
            var sut = new SubString("abc");
            Assert.Equal("abc", sut.Text);
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new SubString("abc", 1..2);
            Assert.Equal("b", sut.Text);
            Assert.Equal("b", sut.ToString());
        }
        {
            var sut = new SubString("abcdefg", 1..^1);
            Assert.Equal("bcdef", sut.Text);
            Assert.Equal("bcdef", sut.ToString());
        }
    }

    [Fact()]
    public void EmptyTest() {
        Assert.Equal("", SubString.Empty.ToString());
    }

    [Fact()]
    public void GetSubStringPosLengthTest() {
        {
            var abcdefg = new SubString("abcdefg");
            var sut = abcdefg.GetSubString(1, 5);
            Assert.Equal("bcdef", sut.Text);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new SubString("abcdefg");
            var bcdef = abcdefg.GetSubString(1, 5);
            var sut = bcdef.GetSubString(1, 3);
            Assert.Equal("cde", sut.Text);
            Assert.Equal("cde", sut.ToString());
        }
        {

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new SubString("abcdefg").GetSubString(-1, 2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new SubString("abcdefg").GetSubString(42, 2);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new SubString("abcdefg").GetSubString(0, 42);
            });

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var sut = new SubString("abcdefg").GetSubString(0, -1);
            });

        }
    }

    [Fact()]
    public void GetSubStringRangeTest() {
        {
            var abcdefg = new SubString("abcdefg");
            var sut = abcdefg.GetSubString(1..6);
            Assert.Equal("bcdef", sut.Text);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new SubString("abcdefg");
            var bcdef = abcdefg.GetSubString(1..6);
            var sut = bcdef.GetSubString(1..4);
            Assert.Equal("cde", sut.Text);
            Assert.Equal("cde", sut.ToString());
        }
        {
            var abcdefg = new SubString("abcdefg");
            var sut = abcdefg.GetSubString(1..^1);
            Assert.Equal("bcdef", sut.Text);
            Assert.Equal("bcdef", sut.ToString());
        }
        {
            var abcdefg = new SubString("abcdefg");
            var bcdef = abcdefg.GetSubString(1..^1);
            var sut = bcdef.GetSubString(1..^1);
            Assert.Equal("cde", sut.Text);
            Assert.Equal("cde", sut.ToString());
        }
    }

    [Fact()]
    public void AsSpanTest() {
        {
            var abcdefg = new SubString("abcdefg");
            var sut = abcdefg.AsSpan();
            Assert.Equal("abcdefg", sut.ToString());
        }
        {
            var abcdefg = new SubString("abcdefg");
            var sut = abcdefg.GetSubString(1..^1).AsSpan();
            Assert.Equal("bcdef", sut.ToString());
        }
    }

    [Fact()]
    public void IsNullOrEmptyTest() {
        {
            var sut = new SubString();
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
        {
            var sut = new SubString("abcdefg");
            Assert.Equal(false, sut.IsNullOrEmpty());

            sut = sut.GetSubString(1..3);
            Assert.Equal(false, sut.IsNullOrEmpty());

            sut = sut.GetSubString(1..1);
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
        {
            var sut = new SubString("abcdefg", new Range(0, 0));
            Assert.Equal(true, sut.IsNullOrEmpty());
        }
    }

    [Fact()]
    public void IsNullOrWhiteSpaceTest() {
        {
            var sut = new SubString();
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new SubString("a     g");
            Assert.Equal(false, sut.IsNullOrWhiteSpace());

            sut = sut.GetSubString(1..3);
            Assert.Equal(true, sut.IsNullOrWhiteSpace());

            sut = sut.GetSubString(1..1);
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new SubString("abcdefg", new Range(0, 0));
            Assert.Equal(true, sut.IsNullOrWhiteSpace());
        }
        {
            var sut = new SubString(" bcdefg", new Range(0, 3));
            Assert.Equal(false, sut.IsNullOrWhiteSpace());
        }
    }

    [Fact()]
    public void IndexOfTest() {
        {
            var sut = new SubString("abcdef");
            Assert.Equal(0, sut.IndexOf('a'));
            Assert.Equal(2, sut.IndexOf('c'));
            Assert.Equal(5, sut.IndexOf('f'));
            Assert.Equal(-1, sut.IndexOf('x'));
        }
        {
            var sut = new SubString("xxabcdefxx");
            sut = sut.GetSubString(2..8);
            Assert.Equal(0, sut.IndexOf('a'));
            Assert.Equal(2, sut.IndexOf('c'));
            Assert.Equal(5, sut.IndexOf('f'));
            Assert.Equal(-1, sut.IndexOf('x'));
        }

        {
            var sut = new SubString("xxabcdefxx");
            sut = sut.GetSubString(2..8);
            Assert.Equal(-1, sut.IndexOf('a', 1..^0));
            Assert.Equal(2, sut.IndexOf('c', 1..^0));
            Assert.Equal(5, sut.IndexOf('f', 1..^0));
            Assert.Equal(-1, sut.IndexOf('x', 1..^0));
        }
    }

    [Fact()]
    public void IndexOfAnyTest() {
        {
            var sut = new SubString("abcdef");
            Assert.Equal(0, sut.IndexOfAny("a".ToCharArray()));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray()));
            Assert.Equal(5, sut.IndexOfAny("fzk".ToCharArray()));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray()));
        }
        {
            var sut = new SubString("xxabcdefxx");
            sut = sut.GetSubString(2..8);
            Assert.Equal(0, sut.IndexOfAny("azk".ToCharArray()));
            Assert.Equal(2, sut.IndexOfAny("czk".ToCharArray()));
            Assert.Equal(5, sut.IndexOfAny("fzk".ToCharArray()));
            Assert.Equal(-1, sut.IndexOfAny("xzk".ToCharArray()));
        }
        {
            var sut = new SubString("xxabcdefxx");
            sut = sut.GetSubString(2..8);
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

            var sut = new SubString("abc\r\ndef");
            var act = sut.SplitInto("\r\n".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("def", act.Tail.ToString());
        }
    }

    [Fact()]
    public void SplitIntoSepStopTest() {
        {
            var sut = new SubString("abc\r\ndef$ghi");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("def", act.Tail.ToString());
        }
    }

    [Fact()]
    public void SplitIntoNoMatchTest() {
        {
            var sut = new SubString("abc");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("abc", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
    }


    [Fact()]
    public void SplitIntoEmptyInputTest() {
        {
            var sut = new SubString(string.Empty);
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            Assert.Equal("", act.Found.ToString());
            Assert.Equal("", act.Tail.ToString());
        }
    }
    [Fact()]
    public async Task UsingInAsyncTest() {
        var sut = await doSomething(new SubString("abcdef"));
        Assert.Equal("bcde", sut.ToString());
        async Task<SubString> doSomething(SubString subString) {
            await Task.Delay(1);
            return subString.GetSubString(1..^1);
        }
    }

    [Fact()]
    public void SplitIntoWhileTest() {
        {
            var sut = new SubString("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : 1);
            Assert.Equal("0123456789", act.Found.ToString());
            Assert.Equal("ABCDEF", act.Tail.ToString());
        }
        {
            var sut = new SubString("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : -1);
            Assert.Equal("0123456789", act.Found.ToString());
        }
    }
}
