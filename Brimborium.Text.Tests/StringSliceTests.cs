namespace Brimborium.Text;

public class StringSliceTests {
    [Test]
    public async Task Struct_Ctor_Empty() {
        StringSlice act = new();
        await Assert.That(act.Text).IsNotNull();
    }

    [Test]
    public async Task SubString_Ctor() {
        {
            var sut = new StringSlice();
            await Assert.That(sut.ToString()).IsEqualTo("");
        }
        {
            var sut = new StringSlice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
        }
        {
            var sut = new StringSlice("abc", 1..2);
            await Assert.That(sut.ToString()).IsEqualTo("b");
        }
        {
            var sut = new StringSlice("abcdefg", 1..^1);
            await Assert.That(sut.ToString()).IsEqualTo("bcdef");
        }
    }

    [Test]
    public async Task EmptyTest() {
        await Assert.That(StringSlice.Empty.ToString()).IsEqualTo("");
    }

    [Test]
    public async Task RangeTest() {
        {
            var sut = new StringSlice("abcdefg");
            var act = sut[1..3];
            await Assert.That(act.ToString()).IsEqualTo("bc");
        }

        {
            var sut = new StringSlice("abcdefg", 1..^0);
            var act = sut[1..3];
            await Assert.That(act.ToString()).IsEqualTo("cd");
        }

        {
            var sut = new StringSlice("abcdefg", 4..^0);
            await Assert.That(sut.ToString()).IsEqualTo("efg");

            var act = sut[1..^0];
            await Assert.That(act.ToString()).IsEqualTo("fg");
        }

        {
            var sut = new StringSlice("abcdefg", 4..^0);
            await Assert.That(sut.ToString()).IsEqualTo("efg");

            var act1 = sut[1..3];
            await Assert.That(act1.ToString()).IsEqualTo("fg");
        }


        {
            var sut = new StringSlice("abcdefg", 5..^0);
            await Assert.That(sut.ToString()).IsEqualTo("fg");

            Assert.Throws<ArgumentOutOfRangeException>(() => {
                var act1 = sut[1..3];
            });
        }
    }

    [Test]
    public async Task GetSubStringPosTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1);
            await Assert.That(sut.ToString()).IsEqualTo("bcdefg");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1);
            var sut = bcdef.Substring(1);
            await Assert.That(sut.ToString()).IsEqualTo("cdefg");
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

    [Test]
    public async Task GetSubStringPosLengthTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1, 5);
            await Assert.That(sut.ToString()).IsEqualTo("bcdef");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1, 5);
            var sut = bcdef.Substring(1, 3);
            await Assert.That(sut.ToString()).IsEqualTo("cde");
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

    [Test]
    public async Task GetSubStringRangeTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..6);
            await Assert.That(sut.ToString()).IsEqualTo("bcdef");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1..6);
            var sut = bcdef.Substring(1..4);
            await Assert.That(sut.ToString()).IsEqualTo("cde");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..^1);
            await Assert.That(sut.ToString()).IsEqualTo("bcdef");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdef = abcdefg.Substring(1..^1);
            var sut = bcdef.Substring(1..^1);
            await Assert.That(sut.ToString()).IsEqualTo("cde");
        }
    }

    [Test]
    public async Task LeftTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var bcdefg = abcdefg.Substring(1);
            var sut = abcdefg.Left(bcdefg);
            await Assert.That(sut.ToString()).IsEqualTo("a");
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
            await Assert.That(sut.ToString()).IsEqualTo("abcd");
        }
    }

    [Test]
    public async Task AsSpanTest() {
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.AsSpan();
            await Assert.That(sut.ToString()).IsEqualTo("abcdefg");
        }
        {
            var abcdefg = new StringSlice("abcdefg");
            var sut = abcdefg.Substring(1..^1).AsSpan();
            await Assert.That(sut.ToString()).IsEqualTo("bcdef");
        }
    }

    [Test]
    public async Task IsNullOrEmptyTest() {
        {
            var sut = new StringSlice();
            await Assert.That(sut.IsNullOrEmpty()).IsTrue();
        }
        {
            var sut = new StringSlice("abcdefg");
            await Assert.That(sut.IsNullOrEmpty()).IsFalse();

            sut = sut.Substring(1..3);
            await Assert.That(sut.IsNullOrEmpty()).IsFalse();

            sut = sut.Substring(1..1);
            await Assert.That(sut.IsNullOrEmpty()).IsTrue();
        }
        {
            var sut = new StringSlice("abcdefg", new Range(0, 0));
            await Assert.That(sut.IsNullOrEmpty()).IsTrue();
        }
    }

    [Test]
    public async Task IsNullOrWhiteSpaceTest() {
        {
            var sut = new StringSlice();
            await Assert.That(sut.IsNullOrWhiteSpace()).IsTrue();
        }
        {
            var sut = new StringSlice("a     g");
            await Assert.That(sut.IsNullOrWhiteSpace()).IsFalse();

            sut = sut.Substring(1..3);
            await Assert.That(sut.IsNullOrWhiteSpace()).IsTrue();

            sut = sut.Substring(1..1);
            await Assert.That(sut.IsNullOrWhiteSpace()).IsTrue();
        }
        {
            var sut = new StringSlice("abcdefg", new Range(0, 0));
            await Assert.That(sut.IsNullOrWhiteSpace()).IsTrue();
        }
        {
            var sut = new StringSlice(" bcdefg", new Range(0, 3));
            await Assert.That(sut.IsNullOrWhiteSpace()).IsFalse();
        }
    }

    [Test]
    public async Task IndexOfTest() {
        {
            var sut = new StringSlice("abcdef");
            await Assert.That(sut.IndexOf('a')).IsEqualTo(0);
            await Assert.That(sut.IndexOf('c')).IsEqualTo(2);
            await Assert.That(sut.IndexOf('f')).IsEqualTo(5);
            await Assert.That(sut.IndexOf('x')).IsEqualTo(-1);
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            await Assert.That(sut.IndexOf('a')).IsEqualTo(0);
            await Assert.That(sut.IndexOf('c')).IsEqualTo(2);
            await Assert.That(sut.IndexOf('f')).IsEqualTo(5);
            await Assert.That(sut.IndexOf('x')).IsEqualTo(-1);
        }

        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            await Assert.That(sut.IndexOf('a', 1..^0)).IsEqualTo(-1);
            await Assert.That(sut.IndexOf('c', 1..^0)).IsEqualTo(2);
            await Assert.That(sut[2]).IsEqualTo('c');
            await Assert.That(sut.IndexOf('f', 1..^0)).IsEqualTo(5);
            await Assert.That(sut[5]).IsEqualTo('f');
            await Assert.That(sut.IndexOf('x', 1..^0)).IsEqualTo(-1);
        }
    }

    [Test]
    public async Task IndexOfAnyTest() {
        {
            var sut = new StringSlice("abcdef");
            await Assert.That(sut.IndexOfAny("a".ToCharArray())).IsEqualTo(0);
            await Assert.That(sut.IndexOfAny("czk".ToCharArray())).IsEqualTo(2);
            await Assert.That(sut.IndexOfAny("fzk".ToCharArray())).IsEqualTo(5);
            await Assert.That(sut.IndexOfAny("xzk".ToCharArray())).IsEqualTo(-1);
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            await Assert.That(sut.IndexOfAny("azk".ToCharArray())).IsEqualTo(0);
            await Assert.That(sut.IndexOfAny("czk".ToCharArray())).IsEqualTo(2);
            await Assert.That(sut.IndexOfAny("fzk".ToCharArray())).IsEqualTo(5);
            await Assert.That(sut.IndexOfAny("xzk".ToCharArray())).IsEqualTo(-1);
        }
        {
            var sut = new StringSlice("xxabcdefxx");
            sut = sut.Substring(2..8);
            await Assert.That(sut.IndexOfAny("azk".ToCharArray(), 1..^0)).IsEqualTo(-1);
            await Assert.That(sut.IndexOfAny("czk".ToCharArray(), 1..^0)).IsEqualTo(2);
            await Assert.That(sut.IndexOfAny("fzk".ToCharArray(), 1..^0)).IsEqualTo(5);
            await Assert.That(sut.IndexOfAny("xzk".ToCharArray(), 1..^0)).IsEqualTo(-1);

            await Assert.That(sut.IndexOfAny("azk".ToCharArray(), 1..^1)).IsEqualTo(-1);
            await Assert.That(sut.IndexOfAny("czk".ToCharArray(), 1..^1)).IsEqualTo(2);
            await Assert.That(sut[2]).IsEqualTo('c');
            await Assert.That(sut.IndexOfAny("fzk".ToCharArray(), 1..^1)).IsEqualTo(-1);
            await Assert.That(sut.IndexOfAny("xzk".ToCharArray(), 1..^1)).IsEqualTo(-1);
        }
    }

    [Test]
    public async Task SplitIntoSepTest() {
        {

            var sut = new StringSlice("abc\r\ndef");
            var act = sut.SplitInto("\r\n".ToCharArray());
            await Assert.That(act.Found.ToString()).IsEqualTo("abc");
            await Assert.That(act.Tail.ToString()).IsEqualTo("def");
        }
    }

    [Test]
    public async Task SplitIntoSepStopTest() {
        {
            var sut = new StringSlice("abc\r\ndef$ghi");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            await Assert.That(act.Found.ToString()).IsEqualTo("abc");
            await Assert.That(act.Tail.ToString()).IsEqualTo("def");
        }
    }

    [Test]
    public async Task SplitIntoNoMatchTest() {
        {
            var sut = new StringSlice("abc");
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            await Assert.That(act.Found.ToString()).IsEqualTo("abc");
            await Assert.That(act.Tail.ToString()).IsEqualTo("");
        }
    }

    [Test]
    public async Task SplitIntoEmptyInputTest() {
        {
            var sut = new StringSlice(string.Empty);
            var act = sut.SplitInto("\r\n".ToCharArray(), "$".ToCharArray());
            await Assert.That(act.Found.ToString()).IsEqualTo("");
            await Assert.That(act.Tail.ToString()).IsEqualTo("");
        }
    }

    [Test]
    public async Task UsingInAsyncTest() {
        var sut = await doSomething(new StringSlice("abcdef"));
        await Assert.That(sut.ToString()).IsEqualTo("bcde");
        async Task<StringSlice> doSomething(StringSlice subString) {
            await Task.Delay(1);
            return subString.Substring(1..^1);
        }
    }

    [Test]
    public async Task EqualTest() {
        {
            var a = new StringSlice("0123456789", 1..5);
            var b = new StringSlice("1234");

            await Assert.That(a.ToString()).IsEqualTo("1234");
            await Assert.That(a.Equals(b)).IsTrue();
        }
        {
            var a = new StringSlice("0123456789");
            var b = new StringSlice("1234");

            await Assert.That(a.Equals(b)).IsFalse();
        }
        {
            object a = new StringSlice("0123456789");
            var b = new StringSlice("0123456789");
            await Assert.That(b.Equals(a)).IsTrue();
        }
        {
            object a = 1;
            var b = new StringSlice("0123456789");
            await Assert.That(b.Equals(a)).IsFalse();
        }
        {
            var sut = new StringSlice("ABC");
            await Assert.That(sut.Equals("abc", StringComparison.OrdinalIgnoreCase)).IsTrue();
        }
        {
            var sut = new StringSlice("0123456789");
            await Assert.That(sut.Equals("0123456789".AsSpan(), StringComparison.Ordinal)).IsTrue();
        }
        {
            var s = "1234";
            var a = s.AsStringSlice(1);
            var b = s.AsStringSlice(1);
            await Assert.That(a.Equals(b)).IsTrue();
        }
    }

    [Test]
    public async Task SplitIntoWhileTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : 1);
            await Assert.That(act.Found.ToString()).IsEqualTo("0123456789");
            await Assert.That(act.Tail.ToString()).IsEqualTo("ABCDEF");
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : -1);
            await Assert.That(act.Found.ToString()).IsEqualTo("0123456789");
            await Assert.That(act.Tail.ToString()).IsEqualTo("");
        }
        {
            var sut = new StringSlice("");
            var act = sut.SplitIntoWhile((c, _) => char.IsDigit(c) ? 0 : -1);
            await Assert.That(act.Found.ToString()).IsEqualTo("");
            await Assert.That(act.Tail.ToString()).IsEqualTo("");
        }
        {
            var sut = new StringSlice("aaaaaaa");
            var act = sut.SplitIntoWhile((c, _) => c == 'a' ? 0 : -1);
            await Assert.That(act.Found.ToString()).IsEqualTo("aaaaaaa");
            await Assert.That(act.Tail.ToString()).IsEqualTo("");
        }
    }

    [Test]
    public async Task IndexTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            await Assert.That(sut[0]).IsEqualTo('0');
            await Assert.That(sut[15]).IsEqualTo('F');
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
            await Assert.That(act).IsEqualTo('3');
        }
        {
            var txt = "012345";
            var sut = new StringSlice(txt).Substring(1).Substring(1);
            var act = sut[1];
            await Assert.That(act).IsEqualTo('3');
        }

    }

    [Test]
    public async Task StartsWithTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            await Assert.That(sut.StartsWith("0123", StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.StartsWith("X", StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            await Assert.That(sut.StartsWith("0123".AsSpan(), StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.StartsWith("X".AsSpan(), StringComparison.Ordinal)).IsFalse();
        }
    }

    [Test]
    public async Task TrimStartTest() {
        {
            var sut = new StringSlice("  0123");
            var act = sut.TrimStart();
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(2..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimStart();
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimStart();
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(4..4);
        }
    }

    [Test]
    public async Task TrimEndTest() {
        {
            var sut = new StringSlice("  0123  ");
            var act = sut.TrimEnd();
            await Assert.That(act.ToString()).IsEqualTo("  0123");
            await Assert.That(act.Range).IsEqualTo(0..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimEnd();
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimEnd();
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(0..0);
        }

        {
            var sut = new StringSlice("  0123  ");
            var act = sut.TrimEnd(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("  0123");
            await Assert.That(act.Range).IsEqualTo(0..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimEnd(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimEnd(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(0..0);
        }
    }

    [Test]
    public async Task TrimTest() {
        {
            var sut = new StringSlice("  0123    ");
            var act = sut.Trim();
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(2..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.Trim();
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.Trim();
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(0..0);
        }

        {
            var sut = new StringSlice("  0123  ");
            var act = sut.Trim(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(2..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.Trim(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.Trim(new char[] { ' ' });
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(0..0);
        }
    }

    [Test]
    public async Task TrimWhileTest() {
        {
            var sut = new StringSlice("  0123");
            var act = sut.TrimWhile(decide);
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(2..6);
        }
        {
            var sut = new StringSlice("0123");
            var act = sut.TrimWhile(decide);
            await Assert.That(act.ToString()).IsEqualTo("0123");
            await Assert.That(act.Range).IsEqualTo(0..4);
        }
        {
            var sut = new StringSlice("    ");
            var act = sut.TrimWhile(decide);
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(4..4);
        }
        {
            var sut = new StringSlice("");
            var act = sut.TrimWhile(decide);
            await Assert.That(act.ToString()).IsEqualTo("");
            await Assert.That(act.Range).IsEqualTo(0..0);
        }
        int decide(char value, int _) {
            if (value == ' ') {
                return 0;
            }

            return 1;
        }
    }

    [Test]
    public async Task ReplaceTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.Replace('0', 'X');
            await Assert.That(act.ToString()).IsEqualTo("X123456789ABCDEF");
        }
        {
            var sut = new StringSlice("0023456789ABCD00");
            var act = sut.Replace('0', 'X');
            await Assert.That(act.ToString()).IsEqualTo("XX23456789ABCDXX");
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.Replace('X', 'Y');
            await Assert.That(act.ToString()).IsEqualTo("0123456789ABCDEF");
        }
    }

    [Test]
    public async Task ReadWhileTest() {
        {
            var sut = new StringSlice("");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            await Assert.That(act.ToString()).IsEqualTo("");
        }
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            await Assert.That(act.ToString()).IsEqualTo("0123456789");
        }
        {
            var sut = new StringSlice("-1234 abc");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            await Assert.That(act.ToString()).IsEqualTo("-1234");
        }
        {
            var sut = new StringSlice("ABCDEF");
            var act = sut.ReadWhile((value, idx) => { return char.IsDigit(value) || (idx == 0 && (value == '-' || value == '+')); });
            await Assert.That(act.ToString()).IsEqualTo("");
        }
    }

    [Test]
    public async Task AsImmutableStringSlice_ReturnsEquivalentImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        await Assert.That(immutableSlice.ToString()).IsEqualTo("Hello");
        await Assert.That(immutableSlice.Text).IsEqualTo(stringSlice.Text);
        await Assert.That(immutableSlice.Range).IsEqualTo(stringSlice.Range);
    }

    [Test]
    public async Task AsImmutableStringSlice_WithEmptySlice_ReturnsEmptyImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello", 2..2);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        await Assert.That(immutableSlice.ToString()).IsEqualTo("");
        await Assert.That(immutableSlice.Length).IsEqualTo(0);
        await Assert.That(immutableSlice.Range.Start.Value).IsEqualTo(2);
        await Assert.That(immutableSlice.Range.End.Value).IsEqualTo(2);
    }

    [Test]
    public async Task AsImmutableStringSlice_WithMiddleRange_ReturnsCorrectImmutableStringSlice() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 6..11);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        await Assert.That(immutableSlice.ToString()).IsEqualTo("World");
        await Assert.That(immutableSlice.Length).IsEqualTo(5);
        await Assert.That(immutableSlice.Range.Start.Value).IsEqualTo(6);
        await Assert.That(immutableSlice.Range.End.Value).IsEqualTo(11);
    }

    [Test]
    public async Task AsImmutableStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var stringSlice = new StringSlice(originalText, 0..5);

        // Act
        var immutableSlice = stringSlice.AsImmutableStringSlice();

        // Assert
        await Assert.That(immutableSlice.Text).IsSameReferenceAs(originalText);  // Verifies same string instance is referenced
    }

    [Test]
    public async Task AsImmutableStringSlice_AndExplicitConversion_ReturnEquivalentResults() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var immutableSlice1 = stringSlice.AsImmutableStringSlice();
        var immutableSlice2 = (ImmutableStringSlice)stringSlice;

        // Assert
        await Assert.That(immutableSlice2.ToString()).IsEqualTo(immutableSlice1.ToString());
        await Assert.That(immutableSlice2.Text).IsEqualTo(immutableSlice1.Text);
        await Assert.That(immutableSlice2.Range).IsEqualTo(immutableSlice1.Range);
    }

    [Test]
    public async Task AsMutableStringSliceTest() {
        // Arrange
        var original = new StringSlice("Hello World", 0..5);

        // Act
        var mutable = original.AsMutableStringSlice();

        // Assert
        await Assert.That(mutable.ToString()).IsEqualTo("Hello");
        await Assert.That(mutable.Text).IsEqualTo(original.Text);
        await Assert.That(mutable.Range).IsEqualTo(original.Range);

        // Verify mutable behavior
        mutable.Range = 6..11;
        await Assert.That(mutable.ToString()).IsEqualTo("World");
        // Original should remain unchanged
        await Assert.That(original.ToString()).IsEqualTo("Hello");
    }

    [Test]
    public async Task EndsWithTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            await Assert.That(sut.EndsWith("CDEF", StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.EndsWith("X", StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            await Assert.That(sut.EndsWith("AB", StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.EndsWith("ABC", StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("Hello WORLD");
            await Assert.That(sut.EndsWith("world", StringComparison.OrdinalIgnoreCase)).IsTrue();
            await Assert.That(sut.EndsWith("world", StringComparison.Ordinal)).IsFalse();
        }
    }

    [Test]
    public async Task EndsWithStringSliceTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            var search = new StringSlice("CDEF");
            await Assert.That(sut.EndsWith(search, StringComparison.Ordinal)).IsTrue();
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            var search = new StringSlice("ABC");
            await Assert.That(sut.EndsWith(search, StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("Hello WORLD");
            var search = new StringSlice("WORLD");
            await Assert.That(sut.EndsWith(search, StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.EndsWith(new StringSlice("world"), StringComparison.OrdinalIgnoreCase)).IsTrue();
        }
    }

    [Test]
    public async Task EndsWithSpanTest() {
        {
            var sut = new StringSlice("0123456789ABCDEF");
            await Assert.That(sut.EndsWith("CDEF".AsSpan(), StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.EndsWith("X".AsSpan(), StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("0123456789ABCDEF", 0..12); // "0123456789AB"
            await Assert.That(sut.EndsWith("AB".AsSpan(), StringComparison.Ordinal)).IsTrue();
            await Assert.That(sut.EndsWith("ABC".AsSpan(), StringComparison.Ordinal)).IsFalse();
        }
        {
            var sut = new StringSlice("Hello WORLD");
            await Assert.That(sut.EndsWith("world".AsSpan(), StringComparison.OrdinalIgnoreCase)).IsTrue();
            await Assert.That(sut.EndsWith("world".AsSpan(), StringComparison.Ordinal)).IsFalse();
        }
    }

    [Test]
    [Arguments("Hello World", "World", StringComparison.Ordinal, true)]
    [Arguments("Hello World", "world", StringComparison.Ordinal, false)]
    [Arguments("Hello World", "world", StringComparison.OrdinalIgnoreCase, true)]
    [Arguments("Hello World", "Hello", StringComparison.Ordinal, false)]
    [Arguments("", "", StringComparison.Ordinal, true)]
    [Arguments("A", "", StringComparison.Ordinal, true)]
    [Arguments("", "A", StringComparison.Ordinal, false)]
    public async Task EndsWithTheory(string input, string search, StringComparison comparison, bool expected) {
        var sut = new StringSlice(input);
        await Assert.That(sut.EndsWith(search, comparison)).IsEqualTo(expected);
    }

    [Test]
    public async Task GetOffsetAndLength_ReturnsCorrectValues() {
        // Arrange
        var slice1 = new StringSlice("Hello World", 2..7);  // "llo W"
        var slice2 = new StringSlice("Test", 0..4);         // "Test"
        var slice3 = new StringSlice("ABC", 1..1);          // ""

        // Act
        var result1 = slice1.GetOffsetAndLength();
        var result2 = slice2.GetOffsetAndLength();
        var result3 = slice3.GetOffsetAndLength();

        // Assert
        await Assert.That(result1).IsEqualTo((2, 5));
        await Assert.That(result2).IsEqualTo((0, 4));
        await Assert.That(result3).IsEqualTo((1, 0));
    }

    [Test]
    public async Task TryFind_StringSlice_Test() {
        // Arrange
        var slice = new StringSlice("Hello World!");

        // Act & Assert - Basic search
        {
            var searchFor = new StringSlice("World");
            bool found = slice.TryFind(searchFor, out var result);

            await Assert.That(found).IsTrue();
            await Assert.That(result.Before.ToString()).IsEqualTo("Hello ");
            await Assert.That(result.BeforeAndFound.ToString()).IsEqualTo("Hello World");
            await Assert.That(result.Found.ToString()).IsEqualTo("World");
            await Assert.That(result.FoundAndAfter.ToString()).IsEqualTo("World!");
            await Assert.That(result.After.ToString()).IsEqualTo("!");
        }

        // Act & Assert - Search with case insensitivity
        {
            var searchFor = new StringSlice("world");
            bool found = slice.TryFind(searchFor, out var result, StringComparison.OrdinalIgnoreCase);

            await Assert.That(found).IsTrue();
            await Assert.That(result.Before.ToString()).IsEqualTo("Hello ");
            await Assert.That(result.BeforeAndFound.ToString()).IsEqualTo("Hello World");
            await Assert.That(result.Found.ToString()).IsEqualTo("World");
            await Assert.That(result.FoundAndAfter.ToString()).IsEqualTo("World!");
            await Assert.That(result.After.ToString()).IsEqualTo("!");
        }

        // Act & Assert - Search in middle of string
        {
            var searchFor = new StringSlice("lo Wo");
            bool found = slice.TryFind(searchFor, out var result);

            await Assert.That(found).IsTrue();
            await Assert.That(result.Before.ToString()).IsEqualTo("Hel");
            await Assert.That(result.BeforeAndFound.ToString()).IsEqualTo("Hello Wo");
            await Assert.That(result.Found.ToString()).IsEqualTo("lo Wo");
            await Assert.That(result.FoundAndAfter.ToString()).IsEqualTo("lo World!");
            await Assert.That(result.After.ToString()).IsEqualTo("rld!");
        }

        // Act & Assert - Search not found
        {
            var searchFor = new StringSlice("NotFound");
            bool found = slice.TryFind(searchFor, out var result);

            await Assert.That(found).IsFalse();
        }

        // Act & Assert - Search in substring
        {
            var slice2 = slice.Substring(6); // "World!"
            var searchFor = new StringSlice("or");

            bool found = slice2.TryFind(searchFor, out var result);

            await Assert.That(found).IsTrue();
            await Assert.That(result.Before.ToString()).IsEqualTo("W");
            await Assert.That(result.Found.ToString()).IsEqualTo("or");
            await Assert.That(result.After.ToString()).IsEqualTo("ld!");
        }
    }

    [Test]
    public async Task TryGetFirst_Test() {
        // Arrange & Act & Assert - Non-empty slice
        {
            var slice = new StringSlice("Hello");
            bool success = slice.TryGetFirst(out var firstChar);

            await Assert.That(success).IsTrue();
            await Assert.That(firstChar).IsEqualTo('H');
        }

        // Arrange & Act & Assert - Empty slice
        {
            var slice = new StringSlice("");
            bool success = slice.TryGetFirst(out var firstChar);

            await Assert.That(success).IsFalse();
            await Assert.That(firstChar).IsEqualTo(default);
        }

        // Arrange & Act & Assert - Slice with range that makes it empty
        {
            var slice = new StringSlice("Hello", 2..2);
            bool success = slice.TryGetFirst(out var firstChar);

            await Assert.That(success).IsFalse();
            await Assert.That(firstChar).IsEqualTo(default);
        }

        // Arrange & Act & Assert - Slice with non-zero start
        {
            var slice = new StringSlice("Hello", 2..5);
            bool success = slice.TryGetFirst(out var firstChar);

            await Assert.That(success).IsTrue();
            await Assert.That(firstChar).IsEqualTo('l');
        }

        // Arrange & Act & Assert - Nested slices
        {
            var original = new StringSlice("Hello World");
            var nested = original.Substring(6..11);
            bool success = nested.TryGetFirst(out var firstChar);

            await Assert.That(success).IsTrue();
            await Assert.That(firstChar).IsEqualTo('W');
        }


        // Arrange & Act & Assert - while
        {
            var current = new StringSlice("Hello World");
            while (current.TryGetFirst(out var firstChar)) {
                current = current.Substring(1);
            }
            var (offset, length) = current.GetOffsetAndLength();
            await Assert.That(offset).IsEqualTo(11);
            await Assert.That(length).IsEqualTo(0);
        }
    }

    [Test]
    public async Task SubstringBetweenStartAndStart_Test() {
        // Arrange
        var original = new StringSlice("Hello World");
        var other = original.Substring(6); // "World"

        // Act
        var result = original.SubstringBetweenStartAndStart(other);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Hello ");

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndStart(differentText));

        // Test with invalid other slice (offset before this slice)
        Assert.Throws<ArgumentOutOfRangeException>(() => other.SubstringBetweenStartAndStart(original));

        // Test with invalid other slice (offset outside this slice)
        var beyondSlice = new StringSlice("Hello World Extra", 12..17); // "Extra"
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndStart(beyondSlice));
    }

    [Test]
    public async Task SubstringBetweenStartAndEnd_Test() {
        // Arrange
        var original = new StringSlice("Hello World");
        var other = original.Substring(6, 5); // "World"

        // Act
        var result = original.SubstringBetweenStartAndEnd(other);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo("Hello World");

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => original.SubstringBetweenStartAndEnd(differentText));

        // Test with invalid other slice (end beyond this slice)
        var longText = "Hello World and more text";
        var longSlice = new StringSlice(longText);
        var partSlice = new StringSlice(longText, 0..11); // "Hello World"
        var act = partSlice.SubstringBetweenStartAndEnd(longSlice);
        await Assert.That(act.ToString()).IsEqualTo("Hello World and more text");
    }

    [Test]
    public async Task SubstringBetweenEndAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text)[0..11]; // "Hello World"
        var second = new StringSlice(text)[16..20]; // "Text"

        // Act
        var result = first.SubstringBetweenEndAndStart(second);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo(" Gap ");

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SubstringBetweenEndAndStart(differentText));

        // Test with invalid other slice (start before this slice's end)
        Assert.Throws<ArgumentOutOfRangeException>(() => second.SubstringBetweenEndAndStart(first));
    }

    [Test]
    public async Task SubstringBetweenEndAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 0..20); // "Hello World Gap Text"

        // Act
        var result = first.SubstringBetweenEndAndEnd(second);

        // Assert
        await Assert.That(result.ToString()).IsEqualTo(" World Gap Text");

        // Test with invalid other slice (different text)
        var differentText = new StringSlice("Different");
        Assert.Throws<ArgumentOutOfRangeException>(() => first.SubstringBetweenEndAndEnd(differentText));

        // Test with invalid other slice (end before this slice's end)
        Assert.Throws<ArgumentOutOfRangeException>(() => second.SubstringBetweenEndAndEnd(first));
    }

    [Test]
    public async Task TrySubstringBetweenStartAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..11); // "Hello World"
        var second = new StringSlice(text, 16..20); // "Text"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenStartAndStart(second, out var result);

            await Assert.That(success).IsTrue();
            await Assert.That(result.ToString()).IsEqualTo("Hello World Gap ");
        }

        // Act & Assert - Invalid case (second starts before first)
        {
            bool success = second.TrySubstringBetweenStartAndStart(first, out var result);

            await Assert.That(success).IsFalse();
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenStartAndStart(differentText, out _));
        }
    }

    [Test]
    public async Task TrySubstringBetweenStartAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 6..11); // "World"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenStartAndEnd(second, out var result);

            await Assert.That(success).IsTrue();
            await Assert.That(result.ToString()).IsEqualTo("Hello World");
        }

        // Act & Assert - Invalid case (first starts after second ends)
        {
            var later = new StringSlice(text, 12..16); // "Gap"
            bool success = later.TrySubstringBetweenStartAndEnd(second, out var result);

            await Assert.That(success).IsFalse();
            await Assert.That(result.ToString()).IsEqualTo("");
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenStartAndEnd(differentText, out _));
        }
    }

    [Test]
    public async Task TrySubstringBetweenEndAndStart_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 12..15); // "Gap"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenEndAndStart(second, out var result);

            await Assert.That(success).IsTrue();
            await Assert.That(result.ToString()).IsEqualTo(" World ");
        }

        // Act & Assert - Invalid case (first ends after second starts)
        {
            var overlap = new StringSlice(text, 0..13); // "Hello World G"
            bool success = overlap.TrySubstringBetweenEndAndStart(second, out var result);

            await Assert.That(success).IsFalse();
            await Assert.That(result.ToString()).IsEqualTo("");
        }

        // Act & Assert - Different text throws exception
        {
            var differentText = new StringSlice("Different");
            Assert.Throws<ArgumentOutOfRangeException>(() =>
                first.TrySubstringBetweenEndAndStart(differentText, out _));
        }
    }

    [Test]
    public async Task TrySubstringBetweenEndAndEnd_Test() {
        // Arrange
        var text = "Hello World Gap Text";
        var first = new StringSlice(text, 0..5); // "Hello"
        var second = new StringSlice(text, 6..11); // "World"

        // Act & Assert - Valid case
        {
            bool success = first.TrySubstringBetweenEndAndEnd(second, out var result);

            await Assert.That(success).IsTrue();
            await Assert.That(result.ToString()).IsEqualTo(" World");
        }

        // Act & Assert - Invalid case (first ends after second ends)
        {
            var longer = new StringSlice(text, 0..15); // "Hello World Gap"
            bool success = longer.TrySubstringBetweenEndAndEnd(second, out var result);

            await Assert.That(success).IsFalse();
            await Assert.That(result.ToString()).IsEqualTo("");
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
    [Test]
    public async Task AsSpan_WithRange_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(1..4);

        // Assert
        await Assert.That(span.ToString()).IsEqualTo("ell");
    }

    [Test]
    public async Task AsSpan_WithFullRange_ReturnsEntireSlice() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(..);

        // Assert
        await Assert.That(span.ToString()).IsEqualTo("Hello World");
    }

    [Test]
    public async Task AsSpan_WithPartialSlice_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World", 6..11); // "World"

        // Act
        var span = slice.AsSpan(1..3); // "or"

        // Assert
        await Assert.That(span.ToString()).IsEqualTo("or");
    }

    [Test]
    public async Task AsSpan_WithEmptyRange_ReturnsEmptySpan() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(1..1);

        // Assert
        var length = span.Length;
        await Assert.That(span.ToString()).IsEqualTo("");
        await Assert.That(length).IsEqualTo(0);
    }

    [Test]
    public async Task AsSpan_WithFromEnd_ReturnsCorrectSubset() {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(^5..^0);

        // Assert
        await Assert.That(span.ToString()).IsEqualTo("World");
    }

    [Test]
    [Arguments(0, false, 5, false, "Hello")]
    [Arguments(0, false, 0, false, "")]
    [Arguments(5, true, 0, true, "World")]
    public async Task AsSpan_WithVariousRanges_ReturnsExpectedResults(int start, bool startFromEnd, int end, bool endFromEnd, string expected) {
        Range range = new Range(new Index(start, startFromEnd), new Index(end, endFromEnd));
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var span = slice.AsSpan(range);

        // Assert
        await Assert.That(span.ToString()).IsEqualTo(expected);
    }

    [Test]
    [Arguments(1, true, 5, false)]
    [Arguments(0, false, 12, false)]
    [Arguments(6, false, 5, false)]
    public void AsSpan_WithInvalidRange_ThrowsArgumentOutOfRangeException(int start, bool startFromEnd, int end, bool endFromEnd) {
        Range range = new Range(new Index(start, startFromEnd), new Index(end, endFromEnd));

        // Arrange
        var slice = new StringSlice("Hello World");

        // Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => slice.AsSpan(range));
    }

    [Test]
    public async Task AsSpan_WithNestedSlices_MaintainsCorrectOffsets() {
        // Arrange
        var originalSlice = new StringSlice("Hello World", 6..11); // "World"
        var middleSlice = originalSlice.Substring(1..4); // "orl"

        // Act
        var finalSpan = middleSlice.AsSpan(1..2); // "r"

        // Assert
        await Assert.That(finalSpan.ToString()).IsEqualTo("r");
    }

    [Test]
    public async Task SubstringOffset() {
        var orginal = new StringSlice("0123456789ABCDEF");
        await Assert.That(orginal.GetOffsetAndLength().Offset).IsEqualTo(0);
        await Assert.That(orginal.Substring(2).Substring(6).GetOffsetAndLength().Offset).IsEqualTo(8);
        await Assert.That(orginal.Substring(8).GetOffsetAndLength().Offset).IsEqualTo(8);

    }
}

public class StringSliceAdjustTests {
    [Test]
    public async Task TryAdjustStartToLeft_Test() {
        // Arrange
        var text = "  Hello World";
        var slice = new StringSlice(text, 2..13); // "Hello World"
        
        // Act - Successfully adjust to include whitespace
        bool success = slice.TryAdjustStartToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Difference.ToString()).IsEqualTo("  ");
        await Assert.That(result.Value.ToString()).IsEqualTo("  Hello World");

        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustStartToLeft(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        await Assert.That(nullStop).IsFalse();
        await Assert.That(nullResult.Value.ToString()).IsEqualTo("Hello World");

        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustStartToLeft(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        await Assert.That(falseStop).IsFalse();
        await Assert.That(falseResult.Value.ToString()).IsEqualTo("Hello World");
    }
    
    [Test]
    public async Task TryAdjustStartToRight_Test() {
        // Arrange
        var text = "  Hello World";
        var slice = new StringSlice(text, 0..13); // "  Hello World"
        
        // Act - Successfully adjust to exclude whitespace
        bool success = slice.TryAdjustStartToRight(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Difference.ToString()).IsEqualTo("  ");
        await Assert.That(result.Value.ToString()).IsEqualTo("Hello World");

        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustStartToRight(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        await Assert.That(nullStop).IsFalse();
        await Assert.That(nullResult.Value.ToString()).IsEqualTo("  Hello World");

        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustStartToRight(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        await Assert.That(falseStop).IsFalse();
        await Assert.That(falseResult.Value.ToString()).IsEqualTo("  Hello World");
    }
    
    [Test]
    public async Task TryAdjustEndToLeft_Test() {
        // Arrange
        var text = "Hello World  ";
        var slice = new StringSlice(text, 0..13); // "Hello World  "
        
        // Act - Successfully adjust to exclude trailing whitespace
        bool success = slice.TryAdjustEndToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var result);
        
        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Difference.ToString()).IsEqualTo("  ");
        await Assert.That(result.Value.ToString()).IsEqualTo("Hello World");

        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustEndToLeft(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        await Assert.That(nullStop).IsFalse();
        await Assert.That(nullResult.Value.ToString()).IsEqualTo("Hello World  ");

        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustEndToLeft(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned immediately
        await Assert.That(falseStop).IsFalse();
        await Assert.That(falseResult.Value.ToString()).IsEqualTo("Hello World  ");

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
        await Assert.That(mixedSuccess).IsTrue();
        await Assert.That(mixedResult.Difference.ToString()).IsEqualTo("123");
        await Assert.That(mixedResult.Value.ToString()).IsEqualTo("abc");
    }
    
    [Test]
    public async Task TryAdjustEndToRight_Test() {
        // Arrange
        var text = "Hello World";
        var slice = new StringSlice(text, 0..5); // "Hello"
        
        // Act - Successfully adjust to include the space and "World"
        bool success = slice.TryAdjustEndToRight(
            ch => true, 
            out var result);
        
        // Assert
        await Assert.That(success).IsTrue();
        await Assert.That(result.Difference.ToString()).IsEqualTo(" World");
        await Assert.That(result.Value.ToString()).IsEqualTo("Hello World");

        // Act - Stop adjustment with null return
        bool nullStop = slice.TryAdjustEndToRight(
            ch => null, 
            out var nullResult);
        
        // Assert - Should return false when null is returned
        await Assert.That(nullStop).IsFalse();
        await Assert.That(nullResult.Value.ToString()).IsEqualTo("Hello");

        // Act - Stop adjustment with false return
        bool falseStop = slice.TryAdjustEndToRight(
            ch => false, 
            out var falseResult);
        
        // Assert - Should return false when false is returned
        await Assert.That(falseStop).IsFalse();
        await Assert.That(falseResult.Value.ToString()).IsEqualTo("Hello");

        // Arrange - Slice with specific condition
        var specificSlice = new StringSlice("Hello World123", 0..11); // "Hello World"
        
        // Act - Adjust to include only digits
        bool specificSuccess = specificSlice.TryAdjustEndToRight(
            ch => char.IsDigit(ch) ? true : false, 
            out var specificResult);
        
        // Assert
        await Assert.That(specificSuccess).IsTrue();
        await Assert.That(specificResult.Difference.ToString()).IsEqualTo("123");
        await Assert.That(specificResult.Value.ToString()).IsEqualTo("Hello World123");
    }
    
    [Test]
    public async Task TryAdjust_WithEmptySlice_ReturnsExpectedResults() {
        // Arrange
        var emptySlice = new StringSlice("");
        
        // Act & Assert - TryAdjustStartToLeft
        {
            bool success = emptySlice.TryAdjustStartToLeft(
                ch => true, 
                out var result);
            
            await Assert.That(success).IsFalse();
            await Assert.That(result.Value.ToString()).IsEqualTo("");
            await Assert.That(result.Difference.ToString()).IsEqualTo("");
        }
        
        // Act & Assert - TryAdjustStartToRight
        {
            bool success = emptySlice.TryAdjustStartToRight(
                ch => true, 
                out var result);
            
            await Assert.That(success).IsFalse();
            await Assert.That(result.Value.ToString()).IsEqualTo("");
            await Assert.That(result.Difference.ToString()).IsEqualTo("");
        }
        
        // Act & Assert - TryAdjustEndToLeft
        {
            bool success = emptySlice.TryAdjustEndToLeft(
                ch => true, 
                out var result);
            
            await Assert.That(success).IsFalse();
            await Assert.That(result.Value.ToString()).IsEqualTo("");
            await Assert.That(result.Difference.ToString()).IsEqualTo("");
        }
        
        // Act & Assert - TryAdjustEndToRight
        {
            bool success = emptySlice.TryAdjustEndToRight(
                ch => true, 
                out var result);
            
            await Assert.That(success).IsFalse();
            await Assert.That(result.Value.ToString()).IsEqualTo("");
            await Assert.That(result.Difference.ToString()).IsEqualTo("");
        }
    }
    
    [Test]
    public async Task TryAdjust_WithNestedSlices_MaintainsCorrectOffsets() {
        // Arrange
        var originalText = "  Hello World  ";
        var originalSlice = new StringSlice(originalText);
        var trimmedSlice = originalSlice.Substring(2..11); // "Hello Wor"
        
        // Act - Adjust start to left
        bool startLeftSuccess = trimmedSlice.TryAdjustStartToLeft(
            ch => char.IsWhiteSpace(ch) ? true : false, 
            out var startLeftResult);
        
        // Assert
        await Assert.That(startLeftSuccess).IsTrue();
        await Assert.That(startLeftResult.Difference.ToString()).IsEqualTo("  ");
        await Assert.That(startLeftResult.Value.ToString()).IsEqualTo("  Hello Wor");
        await Assert.That(startLeftResult.Value.Range.Start.Value).IsEqualTo(0);

        // Act - Adjust end to right
        bool endRightSuccess = trimmedSlice.TryAdjustEndToRight(
            ch => ch == 'l' || ch == 'd' || char.IsWhiteSpace(ch) ? true : false, 
            out var endRightResult);
        
        // Assert
        await Assert.That(endRightSuccess).IsTrue();
        await Assert.That(endRightResult.Difference.ToString()).IsEqualTo("ld  ");
        await Assert.That(endRightResult.Value.ToString()).IsEqualTo("Hello World  ");
        await Assert.That(endRightResult.Value.Range.End.Value).IsEqualTo(15);
    }
    
    [Test]
    public async Task TryAdjust_WithComplexPredicates_WorksCorrectly() {
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
        await Assert.That(leftDigitsSuccess).IsTrue();
        await Assert.That(leftDigitsResult.Difference.ToString()).IsEqualTo("12");
        await Assert.That(leftDigitsResult.Value.ToString()).IsEqualTo("12abc34d");

        // Act - Find digits to the right
        bool rightDigitsSuccess = slice.TryAdjustEndToRight(
            ch => {
                return (ch == 'e' || ch == 'f' || char.IsDigit(ch));
            }, 
            out var rightDigitsResult);
        
        // Assert
        await Assert.That(rightDigitsSuccess).IsTrue();
        await Assert.That(rightDigitsResult.Difference.ToString()).IsEqualTo("ef56");
        await Assert.That(rightDigitsResult.Value.ToString()).IsEqualTo("abc34def56");
    }
    
    [Test]
    public async Task TryAdjust_WithConditionalStops_WorksCorrectly() {
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
        await Assert.That(leftSuccess).IsTrue();
        await Assert.That(leftResult.Difference.ToString()).IsEqualTo("c");
        await Assert.That(leftResult.Value.ToString()).IsEqualTo("c123");

        // Act - Adjust right until we hit 'e', then abort
        bool rightSuccess = slice.TryAdjustEndToRight(
            ch => {
                if (ch == 'd') return true;  // Include 'd'
                return null;                 // Abort for anything else
            }, 
            out var rightResult);
        
        // Assert
        await Assert.That(rightSuccess).IsFalse(); // Should return false because we hit null
        await Assert.That(rightResult.Value.ToString()).IsEqualTo("123d");
    }
}