namespace Brimborium.Text;

/// <summary>
/// Tests for the ImmutableStringSlice class.
/// </summary>
public class ImmutableStringSliceTests {
    [Fact]
    public void Constructor_WithValidInput_CreatesInstance() {
        // Arrange & Act
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Assert
        Assert.Equal("Hello World", slice.Text);
        Assert.Equal(0..5, slice.Range);
    }

    [Fact]
    public void Constructor_WithNullText_ThrowsArgumentNullException() {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ImmutableStringSlice(null!, 0..5));
    }

    [Theory]
    [InlineData(-1, 5)]  // Invalid start
    [InlineData(0, 12)]  // Length too long
    [InlineData(6, 6)]   // Start beyond text length
    public void Constructor_WithInvalidRange_ThrowsArgumentOutOfRangeException(int start, int end) {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new ImmutableStringSlice("Hello", start..end));
    }

    [Fact]
    public void Deconstruct_ReturnsCorrectValues() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var (text, range) = slice;

        // Assert
        Assert.Equal("Hello World", text);
        Assert.Equal(0..5, range);
    }

    [Fact]
    public void GetSlice_ReturnsEquivalentStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        Assert.Equal("Hello", stringSlice.ToString());
    }

    [Fact]
    public void ExplicitConversion_ToStringSlice_Works() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        StringSlice stringSlice = (StringSlice)immutableSlice;

        // Assert
        Assert.Equal("Hello", stringSlice.ToString());
    }

    [Fact]
    public void ToString_ReturnsCorrectSlice() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var result = slice.ToString();

        // Assert
        Assert.Equal("Hello", result);
    }

    [Fact]
    public void ToString_CachesResult() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var result1 = slice.ToString();
        var result2 = slice.ToString();

        // Assert
        Assert.Same(result1, result2); // Should return the same string instance
    }

    [Fact]
    public void Equals_WithSameContent_ReturnsTrue() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        Assert.True(slice1.Equals(slice2));
    }

    [Fact]
    public void Equals_WithDifferentContent_ReturnsFalse() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 1..6);

        // Act & Assert
        Assert.False(slice1.Equals(slice2));
    }

    [Fact]
    public void Equals_WithNull_ReturnsFalse() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        Assert.False(slice.Equals(null));
    }

    [Fact]
    public void Equals_WithSameInstance_ReturnsTrue() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        Assert.True(slice.Equals(slice));
    }

    [Fact]
    public void GetHashCode_ReturnsSameValueForEqualContent() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var hash1 = slice1.GetHashCode();
        var hash2 = slice2.GetHashCode();

        // Assert
        Assert.Equal(hash1, hash2);
    }

    [Fact]
    public void GetHashCode_ReturnsDifferentValueForDifferentContent() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 1..6);

        // Act
        var hash1 = slice1.GetHashCode();
        var hash2 = slice2.GetHashCode();

        // Assert
        Assert.NotEqual(hash1, hash2);
    }

    [Fact]
    public void EqualityComparer_ComparesCorrectly() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);
        var slice3 = new ImmutableStringSlice("Hello World", 1..6);

        // Act
        var comparer = slice1 as IEqualityComparer<ImmutableStringSlice>;

        // Assert
        Assert.True(comparer.Equals(slice1, slice2));
        Assert.False(comparer.Equals(slice1, slice3));
        Assert.False(comparer.Equals(slice1, null));
        Assert.False(comparer.Equals(null, slice1));
        Assert.True(comparer.Equals(null, null));
    }

    [Fact]
    public void EqualityComparer_GetHashCode_ThrowsForNull() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IEqualityComparer<ImmutableStringSlice> comparer = slice;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => comparer.GetHashCode(null!));
    }

    [Fact]
    public void DifferentStrings_SameContent_AreEqual() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello Different", 0..5);

        // Act & Assert
        Assert.True(slice1.Equals(slice2));
        Assert.Equal(slice1.GetHashCode(), slice2.GetHashCode());
    }

    [Fact]
    public void AsSpan_ReturnsCorrectSpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal("Hello", new string(span));
        Assert.Equal(5, span.Length);
    }

    [Fact]
    public void AsSpan_WithMiddleRange_ReturnsCorrectSpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 6..11);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal("World", new string(span));
        Assert.Equal(5, span.Length);
    }

    [Fact]
    public void AsSpan_WithEmptyRange_ReturnsEmptySpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello", 2..2);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal(0, span.Length);
        Assert.Equal("", new string(span));
    }

    [Fact]
    public void AsStringSlice_ReturnsEquivalentStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        Assert.Equal("Hello", stringSlice.ToString());
        Assert.Equal(immutableSlice.Text, stringSlice.Text);
        Assert.Equal(immutableSlice.Range, stringSlice.Range);
    }

    [Fact]
    public void AsStringSlice_WithEmptySlice_ReturnsEmptyStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello", 2..2);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        Assert.Equal("", stringSlice.ToString());
        Assert.Equal(0, stringSlice.Length);
        Assert.Equal(2, stringSlice.Range.Start.Value);
        Assert.Equal(2, stringSlice.Range.End.Value);
    }

    [Fact]
    public void AsStringSlice_WithMiddleRange_ReturnsCorrectStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 6..11);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        Assert.Equal("World", stringSlice.ToString());
        Assert.Equal(5, stringSlice.Length);
        Assert.Equal(6, stringSlice.Range.Start.Value);
        Assert.Equal(11, stringSlice.Range.End.Value);
    }

    [Fact]
    public void AsStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var immutableSlice = new ImmutableStringSlice(originalText, 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        Assert.Same(originalText, stringSlice.Text);  // Verifies same string instance is referenced
    }

    [Fact]
    public void AsStringSlice_AndGetSlice_ReturnEquivalentResults() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice1 = immutableSlice.AsStringSlice();
        var stringSlice2 = immutableSlice.AsStringSlice();

        // Assert
        Assert.Equal(stringSlice1.ToString(), stringSlice2.ToString());
        Assert.Equal(stringSlice1.Text, stringSlice2.Text);
        Assert.Equal(stringSlice1.Range, stringSlice2.Range);
    }

    [Fact]
    public void Length_ReturnsCorrectValue() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        Assert.Equal(5, slice.Length);
    }

    [Fact]
    public void Length_WithEmptySlice_ReturnsZero() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello", 2..2);

        // Act & Assert
        Assert.Equal(0, slice.Length);
    }

    [Fact]
    public void Length_WithFullString_ReturnsFullLength() {
        // Arrange
        var text = "Hello World";
        var slice = new ImmutableStringSlice(text, 0..text.Length);

        // Act & Assert
        Assert.Equal(text.Length, slice.Length);
    }

    [Fact]
    public void Length_WithMiddleRange_ReturnsCorrectLength() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 6..11);

        // Act & Assert
        Assert.Equal(5, slice.Length);
    }

    [Fact]
    public void Length_MatchesSpanLength() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 3..8);

        // Act & Assert
        Assert.Equal(slice.AsSpan().Length, slice.Length);
    }

    [Fact]
    public void AsMutableStringSlice_ReturnsEquivalentMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        Assert.Equal("Hello", mutableSlice.ToString());
        Assert.Equal(immutableSlice.Text, mutableSlice.Text);
        Assert.Equal(immutableSlice.Range, mutableSlice.Range);
    }

    [Fact]
    public void AsMutableStringSlice_WithEmptySlice_ReturnsEmptyMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello", 2..2);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        Assert.Equal("", mutableSlice.ToString());
        Assert.Equal(0, mutableSlice.Length);
        Assert.Equal(2, mutableSlice.Range.Start.Value);
        Assert.Equal(2, mutableSlice.Range.End.Value);
    }

    [Fact]
    public void AsMutableStringSlice_WithMiddleRange_ReturnsCorrectMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 6..11);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        Assert.Equal("World", mutableSlice.ToString());
        Assert.Equal(5, mutableSlice.Length);
        Assert.Equal(6, mutableSlice.Range.Start.Value);
        Assert.Equal(11, mutableSlice.Range.End.Value);
    }

    [Fact]
    public void AsMutableStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var immutableSlice = new ImmutableStringSlice(originalText, 0..5);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        Assert.Same(originalText, mutableSlice.Text);  // Verifies same string instance is referenced
    }

    [Fact]
    public void AsMutableStringSlice_AllowsRangeModification() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Act
        mutableSlice.Range = 6..11;

        // Assert
        Assert.Equal("World", mutableSlice.ToString());
        Assert.Equal("Hello", immutableSlice.ToString()); // Original remains unchanged
    }
}
