namespace Brimborium.Text;

/// <summary>
/// Tests for the ImmutableStringSlice class.
/// </summary>
public class ImmutableStringSliceTests {
    [Test]
    public async Task Constructor_WithValidInput_CreatesInstance() {
        // Arrange & Act
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Assert
        await Assert.That(slice.Text).IsEqualTo("Hello World");
        await Assert.That(slice.Range).IsEqualTo(0..5);
    }

    [Test]
    public void Constructor_WithNullText_ThrowsArgumentNullException() {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentNullException>(() => new ImmutableStringSlice(null!, 0..5));
    }

    [Test]
    [Arguments(-1, 5)]  // Invalid start
    [Arguments(0, 12)]  // Length too long
    [Arguments(6, 6)]   // Start beyond text length
    public void Constructor_WithInvalidRange_ThrowsArgumentOutOfRangeException(int start, int end) {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentOutOfRangeException>(() => 
            new ImmutableStringSlice("Hello", start..end));
    }

    [Test]
    public async Task Deconstruct_ReturnsCorrectValues() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var (text, range) = slice;

        // Assert
        await Assert.That(text).IsEqualTo("Hello World");
        await Assert.That(range).IsEqualTo(0..5);
    }

    [Test]
    public async Task GetSlice_ReturnsEquivalentStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice.ToString()).IsEqualTo("Hello");
    }

    [Test]
    public async Task ExplicitConversion_ToStringSlice_Works() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        StringSlice stringSlice = (StringSlice)immutableSlice;

        // Assert
        await Assert.That(stringSlice.ToString()).IsEqualTo("Hello");
    }

    [Test]
    public async Task ToString_ReturnsCorrectSlice() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var result = slice.ToString();

        // Assert
        await Assert.That(result).IsEqualTo("Hello");
    }

    [Test]
    public async Task ToString_CachesResult() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var result1 = slice.ToString();
        var result2 = slice.ToString();

        // Assert
        await Assert.That(result2).IsSameReferenceAs(result1); // Should return the same string instance
    }

    [Test]
    public async Task Equals_WithSameContent_ReturnsTrue() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        await Assert.That(slice1.Equals(slice2)).IsTrue();
    }

    [Test]
    public async Task Equals_WithDifferentContent_ReturnsFalse() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 1..6);

        // Act & Assert
        await Assert.That(slice1.Equals(slice2)).IsFalse();
    }

    [Test]
    public async Task Equals_WithNull_ReturnsFalse() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        await Assert.That(slice.Equals(null)).IsFalse();
    }

    [Test]
    public async Task Equals_WithSameInstance_ReturnsTrue() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        await Assert.That(slice.Equals(slice)).IsTrue();
    }

    [Test]
    public async Task GetHashCode_ReturnsSameValueForEqualContent() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var hash1 = slice1.GetHashCode();
        var hash2 = slice2.GetHashCode();

        // Assert
        await Assert.That(hash2).IsEqualTo(hash1);
    }

    [Test]
    public async Task GetHashCode_ReturnsDifferentValueForDifferentContent() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 1..6);

        // Act
        var hash1 = slice1.GetHashCode();
        var hash2 = slice2.GetHashCode();

        // Assert
        await Assert.That(hash2).IsNotEqualTo(hash1);
    }

    [Test]
    public async Task EqualityComparer_ComparesCorrectly() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello World", 0..5);
        var slice3 = new ImmutableStringSlice("Hello World", 1..6);

        // Act
        var comparer = slice1 as IEqualityComparer<ImmutableStringSlice>;

        // Assert
        await Assert.That(comparer.Equals(slice1, slice2)).IsTrue();
        await Assert.That(comparer.Equals(slice1, slice3)).IsFalse();
        await Assert.That(comparer.Equals(slice1, null)).IsFalse();
        await Assert.That(comparer.Equals(null, slice1)).IsFalse();
        await Assert.That(comparer.Equals(null, null)).IsTrue();
    }

    [Test]
    public void EqualityComparer_GetHashCode_ThrowsForNull() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);
#pragma warning disable CA1859 // Use concrete types when possible for improved performance
        IEqualityComparer<ImmutableStringSlice> comparer = slice;
#pragma warning restore CA1859 // Use concrete types when possible for improved performance

        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => comparer.GetHashCode(null!));
    }

    [Test]
    public async Task DifferentStrings_SameContent_AreEqual() {
        // Arrange
        var slice1 = new ImmutableStringSlice("Hello World", 0..5);
        var slice2 = new ImmutableStringSlice("Hello Different", 0..5);

        // Act & Assert
        await Assert.That(slice1.Equals(slice2)).IsTrue();
        await Assert.That(slice2.GetHashCode()).IsEqualTo(slice1.GetHashCode());
    }

    [Test]
    public async Task AsSpan_ReturnsCorrectSpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        var length = span.Length;
        await Assert.That(new string(span)).IsEqualTo("Hello");
        await Assert.That(length).IsEqualTo(5);
    }

    [Test]
    public async Task AsSpan_WithMiddleRange_ReturnsCorrectSpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 6..11);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        var length = span.Length;
        await Assert.That(new string(span)).IsEqualTo("World");
        await Assert.That(length).IsEqualTo(5);
    }

    [Test]
    public async Task AsSpan_WithEmptyRange_ReturnsEmptySpan() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello", 2..2);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        var length = span.Length;
        await Assert.That(new string(span)).IsEqualTo("");
        await Assert.That(length).IsEqualTo(0);
    }

    [Test]
    public async Task AsStringSlice_ReturnsEquivalentStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice.ToString()).IsEqualTo("Hello");
        await Assert.That(stringSlice.Text).IsEqualTo(immutableSlice.Text);
        await Assert.That(stringSlice.Range).IsEqualTo(immutableSlice.Range);
    }

    [Test]
    public async Task AsStringSlice_WithEmptySlice_ReturnsEmptyStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello", 2..2);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice.ToString()).IsEqualTo("");
        await Assert.That(stringSlice.Length).IsEqualTo(0);
        await Assert.That(stringSlice.Range.Start.Value).IsEqualTo(2);
        await Assert.That(stringSlice.Range.End.Value).IsEqualTo(2);
    }

    [Test]
    public async Task AsStringSlice_WithMiddleRange_ReturnsCorrectStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 6..11);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice.ToString()).IsEqualTo("World");
        await Assert.That(stringSlice.Length).IsEqualTo(5);
        await Assert.That(stringSlice.Range.Start.Value).IsEqualTo(6);
        await Assert.That(stringSlice.Range.End.Value).IsEqualTo(11);
    }

    [Test]
    public async Task AsStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var immutableSlice = new ImmutableStringSlice(originalText, 0..5);

        // Act
        var stringSlice = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice.Text).IsSameReferenceAs(originalText);  // Verifies same string instance is referenced
    }

    [Test]
    public async Task AsStringSlice_AndGetSlice_ReturnEquivalentResults() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var stringSlice1 = immutableSlice.AsStringSlice();
        var stringSlice2 = immutableSlice.AsStringSlice();

        // Assert
        await Assert.That(stringSlice2.ToString()).IsEqualTo(stringSlice1.ToString());
        await Assert.That(stringSlice2.Text).IsEqualTo(stringSlice1.Text);
        await Assert.That(stringSlice2.Range).IsEqualTo(stringSlice1.Range);
    }

    [Test]
    public async Task Length_ReturnsCorrectValue() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 0..5);

        // Act & Assert
        await Assert.That(slice.Length).IsEqualTo(5);
    }

    [Test]
    public async Task Length_WithEmptySlice_ReturnsZero() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello", 2..2);

        // Act & Assert
        await Assert.That(slice.Length).IsEqualTo(0);
    }

    [Test]
    public async Task Length_WithFullString_ReturnsFullLength() {
        // Arrange
        var text = "Hello World";
        var slice = new ImmutableStringSlice(text, 0..text.Length);

        // Act & Assert
        await Assert.That(slice.Length).IsEqualTo(text.Length);
    }

    [Test]
    public async Task Length_WithMiddleRange_ReturnsCorrectLength() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 6..11);

        // Act & Assert
        await Assert.That(slice.Length).IsEqualTo(5);
    }

    [Test]
    public async Task Length_MatchesSpanLength() {
        // Arrange
        var slice = new ImmutableStringSlice("Hello World", 3..8);

        // Act & Assert
        await Assert.That(slice.Length).IsEqualTo(slice.AsSpan().Length);
    }

    [Test]
    public async Task AsMutableStringSlice_ReturnsEquivalentMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        await Assert.That(mutableSlice.ToString()).IsEqualTo("Hello");
        await Assert.That(mutableSlice.Text).IsEqualTo(immutableSlice.Text);
        await Assert.That(mutableSlice.Range).IsEqualTo(immutableSlice.Range);
    }

    [Test]
    public async Task AsMutableStringSlice_WithEmptySlice_ReturnsEmptyMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello", 2..2);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        await Assert.That(mutableSlice.ToString()).IsEqualTo("");
        await Assert.That(mutableSlice.Length).IsEqualTo(0);
        await Assert.That(mutableSlice.Range.Start.Value).IsEqualTo(2);
        await Assert.That(mutableSlice.Range.End.Value).IsEqualTo(2);
    }

    [Test]
    public async Task AsMutableStringSlice_WithMiddleRange_ReturnsCorrectMutableStringSlice() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 6..11);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        await Assert.That(mutableSlice.ToString()).IsEqualTo("World");
        await Assert.That(mutableSlice.Length).IsEqualTo(5);
        await Assert.That(mutableSlice.Range.Start.Value).IsEqualTo(6);
        await Assert.That(mutableSlice.Range.End.Value).IsEqualTo(11);
    }

    [Test]
    public async Task AsMutableStringSlice_PreservesOriginalText() {
        // Arrange
        var originalText = "Hello World";
        var immutableSlice = new ImmutableStringSlice(originalText, 0..5);

        // Act
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Assert
        await Assert.That(mutableSlice.Text).IsSameReferenceAs(originalText);  // Verifies same string instance is referenced
    }

    [Test]
    public async Task AsMutableStringSlice_AllowsRangeModification() {
        // Arrange
        var immutableSlice = new ImmutableStringSlice("Hello World", 0..5);
        var mutableSlice = immutableSlice.AsMutableStringSlice();

        // Act
        mutableSlice.Range = 6..11;

        // Assert
        await Assert.That(mutableSlice.ToString()).IsEqualTo("World");
        await Assert.That(immutableSlice.ToString()).IsEqualTo("Hello"); // Original remains unchanged
    }
}