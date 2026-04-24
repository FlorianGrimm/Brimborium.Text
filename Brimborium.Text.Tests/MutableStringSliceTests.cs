namespace Brimborium.Text;

public class MutableStringSliceTests {
    [Test]
    public async Task ExplicitConversion_ToStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);

        // Act
        StringSlice immutable = (StringSlice)mutable;

        // Assert
        await Assert.That(immutable.ToString()).IsEqualTo("Hello");
        await Assert.That(immutable.Text).IsEqualTo(mutable.Text);
        await Assert.That(immutable.Range).IsEqualTo(mutable.Range);
    }

    [Test]
    public async Task ExplicitConversion_FromStringSlice_Works() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var mutable = (MutableStringSlice)stringSlice;

        // Assert
        await Assert.That(mutable.ToString()).IsEqualTo("Hello");
        await Assert.That(mutable.Text).IsEqualTo(stringSlice.Text);
        await Assert.That(mutable.Range).IsEqualTo(stringSlice.Range);
    }

    [Test]
    public async Task EqualityOperator_WithStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);
        var stringSlice = new StringSlice("Hello World", 0..5);
        var differentStringSlice = new StringSlice("Hello World", 1..6);

        // Assert
        await Assert.That(mutable == stringSlice).IsTrue();
        await Assert.That(stringSlice == mutable).IsTrue();
        await Assert.That(mutable == differentStringSlice).IsFalse();
        await Assert.That(differentStringSlice == mutable).IsFalse();
        await Assert.That(mutable == (MutableStringSlice?)null).IsFalse();
        await Assert.That(null == stringSlice).IsFalse();
    }

    [Test]
    public async Task InequalityOperator_WithStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);
        var stringSlice = new StringSlice("Hello World", 0..5);
        var differentStringSlice = new StringSlice("Hello World", 1..6);

        // Assert
        await Assert.That(mutable != stringSlice).IsFalse();
        await Assert.That(stringSlice != mutable).IsFalse();
        await Assert.That(mutable != differentStringSlice).IsTrue();
        await Assert.That(differentStringSlice != mutable).IsTrue();
        await Assert.That(mutable != (MutableStringSlice?)null).IsTrue();
        await Assert.That(null != stringSlice).IsTrue();
    }

    [Test]
    public async Task AsSpan_ReturnsCorrectSpan() {
        // Arrange
        var slice = new MutableStringSlice("Hello World", 0..5);
        
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
        var slice = new MutableStringSlice("Hello World", 6..11);
        
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
        var slice = new MutableStringSlice("Hello", 2..2);
        
        // Act
        var span = slice.AsSpan();

        // Assert
        var length = span.Length;
        await Assert.That(new string(span)).IsEqualTo("");
        await Assert.That(length).IsEqualTo(0);
    }
}