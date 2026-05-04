namespace Brimborium.Text;

public class StringRangeTests {
    [Test]
    public async Task Ctor_Default_IsEmptyAndReferencesEmptyString() {
        var sut = new StringRange();
        await Assert.That(sut.Text).IsEqualTo(string.Empty);
        await Assert.That(sut.Start).IsEqualTo(0);
        await Assert.That(sut.End).IsEqualTo(0);
        await Assert.That(sut.Length).IsEqualTo(0);
        await Assert.That(sut.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Ctor_FromString_SpansEntireText() {
        var sut = new StringRange("abcdef");
        await Assert.That(sut.Text).IsEqualTo("abcdef");
        await Assert.That(sut.Start).IsEqualTo(0);
        await Assert.That(sut.End).IsEqualTo(6);
        await Assert.That(sut.Length).IsEqualTo(6);
        await Assert.That(sut.IsEmpty).IsFalse();
    }

    [Test]
    public async Task Ctor_FromEmptyString_IsEmpty() {
        var sut = new StringRange(string.Empty);
        await Assert.That(sut.Length).IsEqualTo(0);
        await Assert.That(sut.IsEmpty).IsTrue();
    }

    [Test]
    public async Task Ctor_StartEnd_SetsBoundsExactly() {
        var sut = new StringRange("abcdef", 1, 4);
        await Assert.That(sut.Start).IsEqualTo(1);
        await Assert.That(sut.End).IsEqualTo(4);
        await Assert.That(sut.Length).IsEqualTo(3);
    }

    [Test]
    public async Task Ctor_StartEqualsTextLength_IsAllowedAndEmpty() {
        var sut = new StringRange("abc", 3, 3);
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.Length).IsEqualTo(0);
    }

    [Test]
    public void Ctor_StartGreaterThanTextLength_Throws() {
        Assert.Throws<ArgumentException>(() => new StringRange("abc", 4, 4));
    }

    [Test]
    public void Ctor_EndGreaterThanTextLength_Throws() {
        Assert.Throws<ArgumentException>(() => new StringRange("abc", 0, 4));
    }

    [Test]
    public void Ctor_EndBeforeStart_Throws() {
        Assert.Throws<ArgumentException>(() => new StringRange("abcdef", 4, 2));
    }

    [Test]
    public async Task Range_ReturnsStartAndEnd() {
        var sut = new StringRange("abcdef", 2, 5);
        var range = sut.Range;
        await Assert.That(range.Start.Value).IsEqualTo(2);
        await Assert.That(range.End.Value).IsEqualTo(5);
        await Assert.That(range.Start.IsFromEnd).IsFalse();
        await Assert.That(range.End.IsFromEnd).IsFalse();
    }

    [Test]
    public async Task AsSpan_FullText_ReturnsAllCharacters() {
        var sut = new StringRange("hello");
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("hello");
    }

    [Test]
    public async Task AsSpan_PartialRange_ReturnsSliceContent() {
        var sut = new StringRange("abcdefg", 2, 5);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("cde");
    }

    [Test]
    public async Task AsSpan_EmptyRange_ReturnsEmptySpan() {
        var sut = new StringRange("abcdef", 3, 3);
        await Assert.That(sut.AsSpan().Length).IsEqualTo(0);
    }

    [Test]
    public async Task SubString_Start_AdvancesStartPosition() {
        var sut = new StringRange("abcdefg").Substring(2);
        await Assert.That(sut.Start).IsEqualTo(2);
        await Assert.That(sut.End).IsEqualTo(7);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("cdefg");
    }

    [Test]
    public async Task SubString_Start_OnAlreadyOffsetRange_AddsToParentStart() {
        var parent = new StringRange("abcdefg", 1, 6); // "bcdef"
        var sut = parent.Substring(2);                 // "def"
        await Assert.That(sut.Start).IsEqualTo(3);
        await Assert.That(sut.End).IsEqualTo(6);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("def");
    }

    [Test]
    public async Task SubString_Start_BeyondLength_ClampsToEnd() {
        var sut = new StringRange("abc").Substring(10);
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.Start).IsEqualTo(3);
        await Assert.That(sut.End).IsEqualTo(3);
    }

    [Test]
    public async Task SubString_StartLength_TakesRequestedSlice() {
        var sut = new StringRange("abcdefg").Substring(1, 3);
        await Assert.That(sut.Start).IsEqualTo(1);
        await Assert.That(sut.End).IsEqualTo(4);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("bcd");
    }

    [Test]
    public async Task SubString_StartLength_LengthExceedingRemainder_IsClamped() {
        var sut = new StringRange("abcdefg").Substring(4, 100);
        await Assert.That(sut.Start).IsEqualTo(4);
        await Assert.That(sut.End).IsEqualTo(7);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("efg");
    }

    [Test]
    public async Task SubString_StartLength_StartBeyondLength_ReturnsEmptyAtEnd() {
        var sut = new StringRange("abc").Substring(10, 5);
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.Start).IsEqualTo(3);
        await Assert.That(sut.End).IsEqualTo(3);
    }

    [Test]
    public async Task SubString_StartLength_OnAlreadyOffsetRange_AddsToParentStart() {
        var parent = new StringRange("abcdefg", 2, 7); // "cdefg"
        var sut = parent.Substring(1, 2);              // "de"
        await Assert.That(sut.Start).IsEqualTo(3);
        await Assert.That(sut.End).IsEqualTo(5);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("de");
    }

    [Test]
    public async Task Indexer_SimpleRange_ReturnsRequestedSlice() {
        var sut = new StringRange("abcdefg")[1..4];
        await Assert.That(sut.Start).IsEqualTo(1);
        await Assert.That(sut.End).IsEqualTo(4);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("bcd");
    }

    [Test]
    public async Task Indexer_FromEndRange_ReturnsRequestedSlice() {
        var sut = new StringRange("abcdefg")[^3..];
        await Assert.That(sut.Start).IsEqualTo(4);
        await Assert.That(sut.End).IsEqualTo(7);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("efg");
    }

    [Test]
    public async Task Indexer_FullRange_ReturnsSameContent() {
        var parent = new StringRange("abcdefg", 1, 5); // "bcde"
        var sut = parent[..];
        await Assert.That(sut.Start).IsEqualTo(1);
        await Assert.That(sut.End).IsEqualTo(5);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("bcde");
    }

    [Test]
    public async Task Indexer_EmptyRange_IsEmpty() {
        var sut = new StringRange("abcdefg")[2..2];
        await Assert.That(sut.IsEmpty).IsTrue();
        await Assert.That(sut.Start).IsEqualTo(2);
        await Assert.That(sut.End).IsEqualTo(2);
    }

    [Test]
    public async Task Indexer_OnAlreadyOffsetRange_AddsParentStart() {
        var parent = new StringRange("abcdefg", 2, 6); // "cdef"
        var sut = parent[1..3];                        // "de"
        await Assert.That(sut.Start).IsEqualTo(3);
        await Assert.That(sut.End).IsEqualTo(5);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("de");
    }

    [Test]
    public async Task Indexer_OnAlreadyOffsetRange_FromEnd_AddsParentStart() {
        var parent = new StringRange("abcdefg", 2, 6); // "cdef"
        var sut = parent[^2..];                        // "ef"
        await Assert.That(sut.Start).IsEqualTo(4);
        await Assert.That(sut.End).IsEqualTo(6);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("ef");
    }

    [Test]
    public void Indexer_RangeOutOfBounds_Throws() {
        var sut = new StringRange("abc");
        Assert.Throws<ArgumentOutOfRangeException>(() => { var _ = sut[0..10]; });
    }

    [Test]
    public async Task TryGetFirst_NonEmpty_ReturnsTrueAndFirstChar() {
        var sut = new StringRange("hello");
        var ok = sut.TryGetFirst(out var first);
        await Assert.That(ok).IsTrue();
        await Assert.That(first).IsEqualTo('h');
    }

    [Test]
    public async Task TryGetFirst_OffsetRange_ReturnsCharAtStart() {
        var sut = new StringRange("hello", 2, 5);
        var ok = sut.TryGetFirst(out var first);
        await Assert.That(ok).IsTrue();
        await Assert.That(first).IsEqualTo('l');
    }

    [Test]
    public async Task TryGetFirst_Empty_ReturnsFalseAndDefault() {
        var sut = new StringRange();
        var ok = sut.TryGetFirst(out var first);
        await Assert.That(ok).IsFalse();
        await Assert.That(first).IsEqualTo('\0');
    }

    [Test]
    public async Task StartsWith_Match_ReturnsTrue() {
        var sut = new StringRange("hello world");
        await Assert.That(sut.StartsWith("hello")).IsTrue();
    }

    [Test]
    public async Task StartsWith_NoMatch_ReturnsFalse() {
        var sut = new StringRange("hello world");
        await Assert.That(sut.StartsWith("world")).IsFalse();
    }

    [Test]
    public async Task StartsWith_OnOffsetRange_UsesSliceContent() {
        var sut = new StringRange("xyz-hello", 4, 9); // "hello"
        await Assert.That(sut.StartsWith("hel")).IsTrue();
        await Assert.That(sut.StartsWith("xyz")).IsFalse();
    }

    [Test]
    public async Task StartsWith_OrdinalIgnoreCase_MatchesIgnoringCase() {
        var sut = new StringRange("Hello");
        await Assert.That(sut.StartsWith("hello", StringComparison.OrdinalIgnoreCase)).IsTrue();
        await Assert.That(sut.StartsWith("hello", StringComparison.Ordinal)).IsFalse();
    }

    [Test]
    public async Task StartsWith_EmptySearch_ReturnsTrue() {
        var sut = new StringRange("abc");
        await Assert.That(sut.StartsWith(string.Empty)).IsTrue();
    }
    
    [Test]
    public async Task SubString_SubString() {
        var sut = new StringRange("abcdefg").Substring(2).Substring(2);
        await Assert.That(sut.Start).IsEqualTo(4);
        await Assert.That(sut.End).IsEqualTo(7);
        await Assert.That(sut.AsSpan().ToString()).IsEqualTo("efg");
    }

    [Test]
    public async Task TryCombine_Success() {
        //                               0123456789
        StringRange text = new("abcdefghij");
        var text_cd = text.Substring(2, 2);
        var text_ef = text.Substring(4, 2);
        await Assert.That(text_cd.TryCombine(text_ef, out var act) ? act.ToString() : "#NO#").IsEqualTo("cdef");
    }


    [Test]
    public async Task TryCombine_NotContinues() {
        //                               0123456789
        StringRange text = new("abcdefghij");
        var text_cd = text.Substring(2, 2);
        var text_ef = text.Substring(6, 2);
        await Assert.That(text_cd.TryCombine(text_ef, out var act) ? act.ToString() : "#NO#").IsEqualTo("#NO#");
    }
}
