using System.Text;

namespace Brimborium.Text;

public class StringSpliceTests {
    [Fact()]
    public void StringSpliceTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new StringSplice(new StringSlice("abc"));
            Assert.Equal("abc", sut.ToString());
        }
        {
            var sut = new StringSplice(new StringSlice("abc"), 1, 1);
            Assert.Equal("b", sut.ToString());
        }
        {
            var sut = new StringSplice(new StringSlice("abc"), 1..3);
            Assert.Equal("bc", sut.ToString());
        }
    }

    [Fact()]
    public void AsSubStringTest() {
        {
            var sut = new StringSplice(new StringSlice("abcdef", 1..3));
            var act = sut.AsSubString();
            Assert.Equal("bc", act.ToString());
        }
    }

    [Fact()]
    public void GetTextTest() {
        {
            var sut = new StringSplice(new StringSlice("abcdef", 1..3));
            Assert.Equal("bc", sut.GetText());
        }
    }

    [Fact()]
    public void SetGetReplacementTextTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.Equal("b", part.ToString());

            part.SetReplacementText("B");
            Assert.Equal("B", part.GetReplacementText());
            Assert.Equal("b", part.ToString());

            Assert.Equal("aBc", sut.BuildReplacement());
            Assert.Equal("aBc", sut.ToString());
        }
    }

    [Fact()]
    public void SetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var sb = new StringBuilder();
            part.SetReplacementBuilder(sb);
            Assert.Same(sb, part.GetReplacementBuilder());
            sb.Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            part.SetReplacementText("BBB");
            Assert.Throws<InvalidOperationException>(() => part.GetReplacementBuilder());
        }
    }

    [Fact()]
    public void GetReplacementBuilderTest() {
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());
        }
        {
            var sut = new StringSplice("abc");
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            Assert.Throws<InvalidOperationException>(() => part.SetReplacementText("BBB"));
        }
    }

    [Fact()]
    public void GetArrayPartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            var part2 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            var part3 = sut.CreatePart(2..2) ?? throw new InvalidOperationException();
            Assert.NotNull(sut.GetArrayPart());
            Assert.Same(part0, sut.GetArrayPart()![0]);
            Assert.Same(part1, sut.GetArrayPart()![1]);
            Assert.Same(part2, sut.GetArrayPart()![2]);
            Assert.Same(part3, sut.GetArrayPart()![3]);
        }
    }

    [Fact()]
    public void IsRangeValidTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal(true, sut.IsRangeValid(1, 2));
            Assert.Equal(false, sut.IsRangeValid(1, 42));
        }
    }

    [Fact()]
    public void CreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.CreatePart(0..1) ?? throw new InvalidOperationException();
            var part1 = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
        }
        {
            // overlap
            var sut = new StringSplice("abc");
            Assert.NotNull(sut.CreatePart(0..2));
            Assert.Null(sut.CreatePart(1..2));
        }

        {
            // append for zero length
            var sut = new StringSplice("abc");
            Assert.NotNull(sut.CreatePart(0..2));
            Assert.NotNull(sut.CreatePart(2..2));
            Assert.NotNull(sut.CreatePart(2..2));
            Assert.Equal(3, sut.GetArrayPart()!.Length);
        }
    }


    [Fact()]
    public void GetOrCreatePartTest() {
        {
            var sut = new StringSplice("abc");
            var part0 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1, 2) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            Assert.NotSame(part0, part1);
            Assert.Same(part0, part2);
            Assert.NotSame(part1, part2);
        }
    }

    [Fact()]
    public void GetLstPartInRangeTest() {
        {
            var sut = new StringSplice("abcdef");
            var part0 = sut.GetOrCreatePart(0, 1) ?? throw new InvalidOperationException();
            var part1 = sut.GetOrCreatePart(1, 1) ?? throw new InvalidOperationException();
            var part2 = sut.GetOrCreatePart(2, 1) ?? throw new InvalidOperationException();
            var part4 = sut.GetOrCreatePart(3, 1) ?? throw new InvalidOperationException();
            var act = sut.GetLstPartInRange(1, 2).ToList();
            Assert.Equal(2, act.Count);
            Assert.Same(part1, act[0]);
            Assert.Same(part2, act[1]);
        }
    }

    [Fact()]
    public void BuildReplacementTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.Equal("ac", sut.ToString());

            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            Assert.Equal("aBBc", sut.BuildReplacement());
            Assert.Equal("aBBc", sut.ToString());

            part.GetReplacementBuilder().Append("bBB");
            Assert.Equal("aBBbBBc", sut.BuildReplacement());
        }

        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();

            Assert.Equal("ac", sut.BuildReplacement());
        }
    }

    [Fact()]
    public void BuildReplacementStringBuilderTest() {
        {
            var sut = new StringSplice("abc");
            Assert.Equal("abc", sut.ToString());
            var part = sut.CreatePart(1..2) ?? throw new InvalidOperationException();
            Assert.NotNull(part.GetReplacementBuilder());
            part.GetReplacementBuilder().Append("BB");
            var sb = new StringBuilder();
            sut.BuildReplacementStringBuilder(sb);
            Assert.Equal("aBBc", sb.ToString());
            Assert.Equal("aBBc", sut.ToString());
        }
    }

    [Fact]
    public void T001CreatePartUp() {
        var sut = new StringSplice("Hello World");

        var part22 = sut.CreatePart(2, 2);
        Assert.Equal("ll", part22?.GetText());

        var part81 = sut.CreatePart(8, 1);
        Assert.Equal("r", part81?.GetText());

        Assert.Equal("2..4;8..9", string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }
    [Fact]
    public void T002CreatePartDown() {
        var sut = new StringSplice("Hello World");

        var part81 = sut.CreatePart(8, 1);
        Assert.Equal("r", part81?.GetText());

        var part22 = sut.CreatePart(2, 2);
        Assert.Equal("ll", part22?.GetText());

        Assert.Equal("2..4;8..9", string.Join(";",
            sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }

    [Fact]
    public void T003CreatePartUp() {
        var sut = new StringSplice("Hello World");
        var act = new List<StringSplice>();
        foreach (int start in new int[] { 0, 8, 6, 4 }) {
            var p = sut.CreatePart(start, 2);
            if (p is null) {
                throw new Exception("p is null");
            }

            act.Add(p);
        }
        Assert.Equal("0..2;4..6;6..8;8..10", string.Join(";", sut.GetArrayPart()?.Select(item => item.Range.ToString())
            ?? throw new Exception("sut.GetArrayPart() is null")));
        Assert.Equal("Heo Worl", string.Join("", sut.GetArrayPart()?.Select(item => item.GetText())
            ?? throw new Exception("sut.GetArrayPart() is null")));
    }

    [Fact]
    public void T004CreatePartOverlapUp() {
        var sut = new StringSplice("Hello World");

        Assert.NotNull(sut.CreatePart(4, 4));
        Assert.Null(sut.CreatePart(4, 4));
        Assert.Null(sut.CreatePart(2, 4));
    }

    [Fact]
    public void T005CreatePartLength() {
        var sut = new StringSplice("Hello World");
        Assert.Null(sut.CreatePart(4, 10));
    }

    [Fact]
    public void T006GetOrCreatePart() {
        var sut = new StringSplice("Hello World");
        var a = sut.GetOrCreatePart(4, 4);
        Assert.NotNull(a);
        Assert.Equal(4, a.Range.Start);
        Assert.Equal(8, a.Range.End);
        Assert.Equal(4, a.Length);

        var b = sut.GetOrCreatePart(4, 4);
        Assert.NotNull(b);

        Assert.Same(a, b);

        var c = sut.GetOrCreatePart(4, 40);
        Assert.Null(c);

    }



    [Fact]
    public void T011BuildReplacement() {
        var sut = new StringSplice("Hello World");
        var part = sut.CreatePart(1, 1);
        if (part is null) { throw new Exception("part is null"); }
        part.GetReplacementBuilder().Append("a");
        Assert.Equal("Hallo World", sut.BuildReplacement());
        var part2 = sut.CreatePart(sut.Length, 0);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.GetReplacementBuilder().Append("!");
        Assert.Equal("Hallo World!", sut.BuildReplacement());
    }

    [Fact]
    public void T012BuildReplacement() {
        var sut = new StringSplice("123BC");
        //var sutRange = new Range(1, new Index(1, true),
        var part1 = sut.CreatePart(1..^1);
        if (part1 is null) { throw new Exception("part is null"); }
        Assert.Equal("23B", part1.GetText());

        var part2 = part1.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        Assert.Equal("2AB", part1.BuildReplacement());
        Assert.Equal("12ABC", sut.BuildReplacement());
    }

    [Fact]
    public void T013BuildReplacement() {
        var sut = new StringSplice("123BC");

        var part = new StringSplice(sut.AsSubString(), 1..^1);
        if (part is null) { throw new Exception("part is null"); }
        Assert.Equal("23B", part.GetText());

        var part2 = part.CreatePart(1, 1);
        if (part2 is null) { throw new Exception("part2 is null"); }
        part2.SetReplacementText("A");

        Assert.Equal("2AB", part.BuildReplacement());

        Assert.Equal("123BC", sut.BuildReplacement());
    }

    [Fact]
    public void T014BuildReplacement() {
        var sut = new StringSplice("15");

        sut.CreatePart(1, 0)?.SetReplacementText("2");
        sut.CreatePart(1, 0)?.SetReplacementText("3");
        sut.CreatePart(1, 0)?.SetReplacementText("4");

        Assert.Equal("12345", sut.BuildReplacement());
    }

    

     [Fact]
    public void Constructor_WithString_CreatesValidInstance()
    {
        // Arrange & Act
        var splice = new StringSplice("Hello World");

        // Assert
        Assert.Equal("Hello World", splice.GetText());
        Assert.Equal(0..11, splice.Range);
    }

    [Fact]
    public void Constructor_WithStringSlice_CreatesValidInstance()
    {
        // Arrange
        var slice = new StringSlice("Hello World");

        // Act
        var splice = new StringSplice(slice);

        // Assert
        Assert.Equal("Hello World", splice.GetText());
        Assert.Equal(0..11, splice.Range);
    }

    [Theory]
    [InlineData("Hello World", 0, 5, "Hello")]
    [InlineData("Hello World", 6, 5, "World")]
    public void Constructor_WithStartAndLength_CreatesValidInstance(string text, int start, int length, string expected)
    {
        // Arrange & Act
        var splice = new StringSplice(new StringSlice(text), start, length);

        // Assert
        Assert.Equal(expected, splice.GetText());
        Assert.Equal(start..(start + length), splice.Range);
    }

    [Theory]
    [InlineData(-1, 5)]
    [InlineData(0, -1)]
    [InlineData(12, 1)]
    [InlineData(0, 12)]
    public void Constructor_WithInvalidStartAndLength_ThrowsArgumentOutOfRangeException(int start, int length)
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new StringSplice(new StringSlice("Hello World"), start, length));
    }

    [Fact]
    public void SetReplacementText_ValidText_UpdatesReplacement()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(0, 5);

        // Act
        part!.SetReplacementText("Hi");

        // Assert
        Assert.Equal("Hi", part.GetReplacementText());
        Assert.Equal("Hi World", splice.ToString());
    }

    [Fact]
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

    [Fact]
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

    [Theory]
    [InlineData(0, 5, true)]
    [InlineData(-1, 5, false)]
    [InlineData(0, -1, false)]
    [InlineData(12, 1, false)]
    [InlineData(0, 12, false)]
    public void IsRangeValid_ChecksRangeValidity(int start, int length, bool expected)
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var isValid = splice.IsRangeValid(start, length);

        // Assert
        Assert.Equal(expected, isValid);
    }

    [Fact]
    public void CreatePart_ValidRange_CreatesNewPart()
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var part = splice.CreatePart(0, 5);

        // Assert
        Assert.NotNull(part);
        Assert.Equal("Hello", part.GetText());
    }

    [Fact]
    public void CreatePart_OverlappingRanges_ReturnsNull()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        splice.CreatePart(0, 5);

        // Act
        var overlappingPart = splice.CreatePart(3, 4);

        // Assert
        Assert.Null(overlappingPart);
    }

    [Fact]
    public void CreatePart_MultipleNonOverlappingParts_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Assert
        Assert.NotNull(part1);
        Assert.NotNull(part2);
        Assert.Equal("Hello", part1.GetText());
        Assert.Equal("World", part2.GetText());
    }

    [Fact]
    public void ToString_WithReplacements_ReturnsModifiedText()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Act
        part1!.SetReplacementText("Hi");
        part2!.SetReplacementText("Everyone");

        // Assert
        Assert.Equal("Hi Everyone", splice.ToString());
    }

    [Fact]
    public void ToString_WithEmptyReplacement_RemovesText()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part = splice.CreatePart(5, 1);

        // Act
        part!.SetReplacementText("");

        // Assert
        Assert.Equal("HelloWorld", splice.ToString());
    }

    [Fact]
    public void CreatePart_ZeroLength_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part = splice.CreatePart(5, 0);

        // Assert
        Assert.NotNull(part);
        Assert.Equal("", part.GetText());
        part!.SetReplacementText("!");
        Assert.Equal("Hello! World", splice.ToString());
    }

    [Fact]
    public void CreatePart_MultipleZeroLengthAtSamePosition_Success()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        
        // Act
        var part1 = splice.CreatePart(5, 0);
        var part2 = splice.CreatePart(5, 0);
        var part3 = splice.CreatePart(5, 0);

        // Assert
        Assert.NotNull(part1);
        Assert.NotNull(part2);
        Assert.NotNull(part3);
        
        part1!.SetReplacementText("1");
        part2!.SetReplacementText("2");
        part3!.SetReplacementText("3");
        
        Assert.Equal("Hello123 World", splice.ToString());
    }

    [Fact]
    public void GetArrayPart_ReturnsAllParts()
    {
        // Arrange
        var splice = new StringSplice("Hello World");
        var part1 = splice.CreatePart(0, 5);
        var part2 = splice.CreatePart(6, 5);

        // Act
        var parts = splice.GetArrayPart();

        // Assert
        Assert.NotNull(parts);
        Assert.Equal(2, parts.Length);
        Assert.Equal("Hello", parts[0].GetText());
        Assert.Equal("World", parts[1].GetText());
    }

    [Fact]
    public void GetArrayPart_WithNoParts_ReturnsNull()
    {
        // Arrange
        var splice = new StringSplice("Hello World");

        // Act
        var parts = splice.GetArrayPart();

        // Assert
        Assert.Null(parts);
    }
}
