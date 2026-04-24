

namespace Brimborium.Text;

public class StringSpliceTests {
    [Test]
    public async Task StringSpliceTest() {
        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
        }
        {
            var sut = new StringSplice(new StringSlice("abc"));
            await Assert.That(sut.ToString()).IsEqualTo("abc");
        }
        {
            var sut = new StringSplice(new StringSlice("abc"), 1, 1);
            await Assert.That(sut.ToString()).IsEqualTo("b");
        }
        {
            var sut = new StringSplice(new StringSlice("abc"), 1..3);
            await Assert.That(sut.ToString()).IsEqualTo("bc");
        }
    }

    [Test]
    public async Task AsSubStringTest() {
        {
            var sut = new StringSplice(new StringSlice("abcdef", 1..3));
            var act = sut.AsSubString();
            await Assert.That(act.ToString()).IsEqualTo("bc");
        }
    }

    [Test]
    public async Task GetTextTest() {
        {
            var sut = new StringSplice(new StringSlice("abcdef", 1..3));
            await Assert.That(sut.GetText()).IsEqualTo("bc");
        }
    }

    [Test]
    public async Task SetGetReplacementTextTest() {
        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            await Assert.That(part.ToString()).IsEqualTo("b");

            part.SetReplacementText("B");
            await Assert.That(part.GetReplacementText()).IsEqualTo("B");
            await Assert.That(part.ToString()).IsEqualTo("b");

            await Assert.That(sut.BuildReplacement()).IsEqualTo("aBc");
            await Assert.That(sut.ToString()).IsEqualTo("aBc");
        }
    }

    [Test]
    public async Task SetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var sb = new StringBuilder();
            part.SetReplacementBuilder(sb);
            await Assert.That(part.GetReplacementBuilder()).IsSameReferenceAs(sb);
            sb.Append("BB");
            await Assert.That(sut.BuildReplacement()).IsEqualTo("aBBc");
            await Assert.That(sut.ToString()).IsEqualTo("aBBc");
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            part.SetReplacementText("BBB");
            Assert.Throws<InvalidOperationException>(() => part.GetReplacementBuilder());
        }
    }

    [Test]
    public async Task GetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            await Assert.That(part.GetReplacementBuilder()).IsNotNull();
            part.GetReplacementBuilder().Append("BB");
            await Assert.That(sut.BuildReplacement()).IsEqualTo("aBBc");
            await Assert.That(sut.ToString()).IsEqualTo("aBBc");
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            await Assert.That(part.GetReplacementBuilder()).IsNotNull();
            Assert.Throws<InvalidOperationException>(() => part.SetReplacementText("BBB"));
        }
    }

    [Test]
    public async Task GetArrayPartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var part2 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            var part3 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            await Assert.That(sut.GetArrayPart()).IsNotNull();
            await Assert.That(sut.GetArrayPart()![0]).IsSameReferenceAs(part0);
            await Assert.That(sut.GetArrayPart()![1]).IsSameReferenceAs(part1);
            await Assert.That(sut.GetArrayPart()![2]).IsSameReferenceAs(part2);
            await Assert.That(sut.GetArrayPart()![3]).IsSameReferenceAs(part3);
        }
    }

    [Test]
    public async Task IsRangeValidTest() {
        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.IsRangeValid(1, 2)).IsTrue();
            await Assert.That(sut.IsRangeValid(1, 42)).IsFalse();
        }
    }

    [Test]
    public async Task CreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
        }
        {
            // overlap
            var sut = new StringSplice("abc");
            await Assert.That(sut.CreatePart(0..2)).IsNotNull();
            await Assert.That(sut.CreatePart(1..2)).IsNull();
        }

        {
            // append for zero length
            var sut = new StringSplice("abc");
            await Assert.That(sut.CreatePart(0..2)).IsNotNull();
            await Assert.That(sut.CreatePart(2..2)).IsNotNull();
            await Assert.That(sut.CreatePart(2..2)).IsNotNull();
            await Assert.That(sut.GetArrayPart()!.Length).IsEqualTo(3);
        }
    }


    [Test]
    public async Task GetOrCreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1, 2) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            await Assert.That(part1).IsNotSameReferenceAs(part0);
            await Assert.That(part2).IsSameReferenceAs(part0);
            await Assert.That(part2).IsNotSameReferenceAs(part1);
        }
    }

    [Test]
    public async Task GetLstPartInRangeTest() {
        {
            var sut = new StringSplice("abcdef");
            var part0 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1, 1) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(2, 1) ?? throw new InvalidOperationException();
            var part4 = sut.GetOrCreatePart(3, 1) ?? throw new InvalidOperationException();
            var act = sut.GetLstPartInRange(1, 2).ToList();
            await Assert.That(act.Count).IsEqualTo(2);
            await Assert.That(act[0]).IsSameReferenceAs(part1);
            await Assert.That(act[1]).IsSameReferenceAs(part2);
        }
    }

    [Test]
    public async Task BuildReplacementTest() {
        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            await Assert.That(sut.ToString()).IsEqualTo("ac");

            await Assert.That(part.GetReplacementBuilder()).IsNotNull();
            part.GetReplacementBuilder().Append("BB");
            await Assert.That(sut.BuildReplacement()).IsEqualTo("aBBc");
            await Assert.That(sut.ToString()).IsEqualTo("aBBc");

            part.GetReplacementBuilder().Append("bBB");
            await Assert.That(sut.BuildReplacement()).IsEqualTo("aBBbBBc");
        }

        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();

            await Assert.That(sut.BuildReplacement()).IsEqualTo("ac");
        }
    }

    [Test]
    public async Task BuildReplacementStringBuilderTest() {
        {
            var sut = new StringSplice("abc");
            await Assert.That(sut.ToString()).IsEqualTo("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            await Assert.That(part.GetReplacementBuilder()).IsNotNull();
            part.GetReplacementBuilder().Append("BB");
            var sb = new StringBuilder();
            sut.BuildReplacementStringBuilder(sb);
            await Assert.That(sb.ToString()).IsEqualTo("aBBc");
            await Assert.That(sut.ToString()).IsEqualTo("aBBc");
        }
    }

    [Test]
    public async Task CreatePart_OrderedUp_Success() {
        var sut = new StringSplice("Hello World");

        var part22 = sut.CreatePart(2, 2);
        await Assert.That(part22?.GetText()).IsEqualTo("ll");

        var part81 = sut.CreatePart(8, 1);
        await Assert.That(part81?.GetText()).IsEqualTo("r");

        await Assert.That(string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null"))).IsEqualTo("2..4;8..9");
    }
    [Test]
    public async Task CreatePart_OrderedDown_Success() {
        var sut = new StringSplice("Hello World");

        var part81 = sut.CreatePart(8, 1);
        await Assert.That(part81?.GetText()).IsEqualTo("r");

        var part22 = sut.CreatePart(2, 2);
        await Assert.That(part22?.GetText()).IsEqualTo("ll");

        await Assert.That(string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null"))).IsEqualTo("2..4;8..9");
    }

    [Test]
    public async Task CreatePart_MultiplePartsOrderedUp_Success() {
        var sut = new StringSplice("Hello World");
        var act = new List<StringSplice>();
        foreach (int start in new int[] { 0, 8, 6, 4 }) {
            var p = sut.CreatePart(start, 2);
            if (p is null) {
                throw new Exception("p is null");
            }

            act.Add(p);
        }
        await Assert.That(string.Join(";", sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null"))).IsEqualTo("0..2;4..6;6..8;8..10");
        await Assert.That(string.Join("", sut.GetArrayPart()?.Select(item => item.GetText())
            ?? throw new Exception("sut.GetArrayPart() is null"))).IsEqualTo("Heo Worl");
    }

    [Test]
    public async Task CreatePart_OverlappingRanges_ReturnsNull1() {
        var sut = new StringSplice("Hello World");

        await Assert.That(sut.CreatePart(4, 4)).IsNotNull();
        await Assert.That(sut.CreatePart(4, 4)).IsNull();
        await Assert.That(sut.CreatePart(2, 4)).IsNull();
    }

    [Test]
    public async Task CreatePart_InvalidLength_ReturnsNull() {
        var sut = new StringSplice("Hello World");
        await Assert.That(sut.CreatePart(4, 10)).IsNull();
    }

    [Test]
    public async Task GetOrCreatePart_ExistingRange_ReturnsSameInstance() {
        var sut = new StringSplice("Hello World");
        var a = sut.GetOrCreatePart(4, 4);
        await Assert.That(a).IsNotNull();
        await Assert.That(a.Range.Start).IsEqualTo(4);
        await Assert.That(a.Range.End).IsEqualTo(8);
        await Assert.That(a.Length).IsEqualTo(4);

        var b = sut.GetOrCreatePart(4, 4);
        await Assert.That(b).IsNotNull();

        await Assert.That(b).IsSameReferenceAs(a);

        var c = sut.GetOrCreatePart(4, 40);
        await Assert.That(c).IsNull();
    }

    [Test]
    public async Task BuildReplacement_SinglePart_Success() {
        var sut = new StringSplice("Hello World");
        var part = sut.CreatePart(1, 1);
        if (part is null) { throw new Exception("part is null"); }
        part.GetReplacementBuilder().Append("a");
        await Assert.That(sut.BuildReplacement()).IsEqualTo("Hallo World");
        var part2 = sut.CreatePart(sut.Length, 0);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.GetReplacementBuilder().Append("!");
        await Assert.That(sut.BuildReplacement()).IsEqualTo("Hallo World!");
    }

    [Test]
    public async Task BuildReplacement_NestedParts_Success() {
        var sut = new StringSplice("123BC");
        //var sutRange = new Range(1, new Index(1, true),
        var part1 = sut.CreatePart(1..^1);
        if (part1 is null) { throw new Exception("part is null"); }
        await Assert.That(part1.GetText()).IsEqualTo("23B");

        var part2 = part1.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        await Assert.That(part1.BuildReplacement()).IsEqualTo("2AB");
        await Assert.That(sut.BuildReplacement()).IsEqualTo("12ABC");
    }

    [Test]
    public async Task BuildReplacement_SubString_PreservesOriginal() {
        var sut = new StringSplice("123BC");

        var part = new StringSplice(sut.AsSubString(), 1..^1);
        if (part is null) { throw new Exception("part is null"); }
        await Assert.That(part.GetText()).IsEqualTo("23B");

        var part2 = part.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        await Assert.That(part.BuildReplacement()).IsEqualTo("2AB");

        await Assert.That(sut.BuildReplacement()).IsEqualTo("123BC");
    }

    [Test]
    public async Task BuildReplacement_MultipleZeroLengthParts_Success() {
        var sut = new StringSplice("15");

        sut.CreatePart(1, 0)?.SetReplacementText("2");
        sut.CreatePart(1, 0)?.SetReplacementText("3");
        sut.CreatePart(1, 0)?.SetReplacementText("4");

        await Assert.That(sut.BuildReplacement()).IsEqualTo("12345");
    }

     [Test]
    public async Task Constructor_WithString_CreatesValidInstance()
    {
        // Arrange & Act
        var splice = new StringSplice("Hello World");

        // Assert
        await Assert.That(splice.GetText()).IsEqualTo("Hello World");
        await Assert.That(splice.Range).IsEqualTo(0..11);
    }

    [Test]
    public async Task Constructor_WithStringSlice_CreatesValidInstance()
    {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var splice = new StringSplice(slice);

        // Assert
        await Assert.That(splice.GetText()).IsEqualTo("Hello World");
        await Assert.That(splice.Range).IsEqualTo(0..11);
    }

    [Test]
    [Arguments("Hello World", 0, 5, "Hello")]
    [Arguments("Hello World", 6, 5, "World")]
    public async Task Constructor_WithStartAndLength_CreatesValidInstance(string text, int start, int length, string expected)
    {
        // Arrange & Act
        var splice = new StringSplice(new StringSlice(text), start, length);

        // Assert
        await Assert.That(splice.GetText()).IsEqualTo(expected);
        await Assert.That(splice.Range).IsEqualTo(start..(start + length));
    }

    [Test]
    [Arguments(-1, 5)]
    [Arguments(0, -1)]
    [Arguments(12, 1)]
    [Arguments(0, 12)]
    public void Constructor_WithInvalidStartAndLength_ThrowsArgumentOutOfRangeException(int start, int length)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new StringSplice(new StringSlice("Hello World"), start, length));
    }

    [Test]
    public async Task SetReplacementText_ValidText_UpdatesReplacement()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(0, 5);

        // Act
        part!.SetReplacementText("Hi");

        // Assert
        await Assert.That(part.GetReplacementText()).IsEqualTo("Hi");
        await Assert.That(splice.ToString()).IsEqualTo("Hi World");
    }

    [Test]
    public void SetReplacementText_AfterBuilder_ThrowsInvalidOperationException()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(0, 5);
        part!.GetReplacementBuilder();

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            part.SetReplacementText("Hi"));
    }

    [Test]
    public void GetReplacementBuilder_AfterText_ThrowsInvalidOperationException()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(0, 5);
        part!.SetReplacementText("Hi");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            part.GetReplacementBuilder());
    }

    [Test]
    [Arguments(0, 5, true)]
    [Arguments(-1, 5, false)]
    [Arguments(0, -1, false)]
    [Arguments(12, 1, false)]
    [Arguments(0, 12, false)]
    public async Task IsRangeValid_ChecksRangeValidity(int start, int length, bool expected)
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var isValid = splice.IsRangeValid(start, length);

        // Assert
        await Assert.That(isValid).IsEqualTo(expected);
    }

    [Test]
    public async Task CreatePart_ValidRange_CreatesNewPart()
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var part = splice.CreatePart(0, 5);

        // Assert
        await Assert.That(part).IsNotNull();
        await Assert.That(part.GetText()).IsEqualTo("Hello");
    }

    [Test]
    public async Task CreatePart_OverlappingRanges_ReturnsNull2()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        splice.CreatePart(0, 5);

        // Act
        var overlappingPart = splice.CreatePart(3, 4);

        // Assert
        await Assert.That(overlappingPart).IsNull();
    }

    [Test]
    public async Task CreatePart_MultipleNonOverlappingParts_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Assert
        await Assert.That(part1).IsNotNull();
        await Assert.That(part2).IsNotNull();
        await Assert.That(part1.GetText()).IsEqualTo("Hello");
        await Assert.That(part2.GetText()).IsEqualTo("World");
    }

    [Test]
    public async Task ToString_WithReplacements_ReturnsModifiedText()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Act
        part1!.SetReplacementText("Hi");
        part2!.SetReplacementText("Everyone");

        // Assert
        await Assert.That(splice.ToString()).IsEqualTo("Hi Everyone");
    }

    [Test]
    public async Task ToString_WithEmptyReplacement_RemovesText()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(5, 1);

        // Act
        part!.SetReplacementText("");

        // Assert
        await Assert.That(splice.ToString()).IsEqualTo("HelloWorld");
    }

    [Test]
    public async Task CreatePart_ZeroLength_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part = splice.CreatePart(5, 0);

        // Assert
        await Assert.That(part).IsNotNull();
        await Assert.That(part.GetText()).IsEqualTo("");
        part!.SetReplacementText("!");
        await Assert.That(splice.ToString()).IsEqualTo("Hello! World");
    }

    [Test]
    public async Task CreatePart_MultipleZeroLengthAtSamePosition_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part1 = splice.CreatePart(5, 0);
        var part2 = splice.CreatePart(5, 0);
        var part3 = splice.CreatePart(5, 0);

        // Assert
        await Assert.That(part1).IsNotNull();
        await Assert.That(part2).IsNotNull();
        await Assert.That(part3).IsNotNull();

        part1!.SetReplacementText("1");
        part2!.SetReplacementText("2");
        part3!.SetReplacementText("3");
        
        await Assert.That(splice.ToString()).IsEqualTo("Hello123 World");
    }

    [Test]
    public async Task GetArrayPart_ReturnsAllParts()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Act
        var parts = splice.GetArrayPart();

        // Assert
        await Assert.That(parts).IsNotNull();
        await Assert.That(parts.Length).IsEqualTo(2);
        await Assert.That(parts[0].GetText()).IsEqualTo("Hello");
        await Assert.That(parts[1].GetText()).IsEqualTo("World");
    }

    [Test]
    public async Task GetArrayPart_WithNoParts_ReturnsNull()
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var parts = splice.GetArrayPart();

        // Assert
        await Assert.That(parts).IsNull();
    }
}