namespace Brimborium.Text;

public class MutableStringSliceTests {
    [Fact]
    public void ExplicitConversion_ToStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);

        // Act
        StringSlice immutable = (StringSlice)mutable;

        // Assert
        Assert.Equal("Hello", immutable.ToString());
        Assert.Equal(mutable.Text, immutable.Text);
        Assert.Equal(mutable.Range, immutable.Range);
    }

    [Fact]
    public void ExplicitConversion_FromStringSlice_Works() {
        // Arrange
        var stringSlice = new StringSlice("Hello World", 0..5);

        // Act
        var mutable = (MutableStringSlice)stringSlice;

        // Assert
        Assert.Equal("Hello", mutable.ToString());
        Assert.Equal(stringSlice.Text, mutable.Text);
        Assert.Equal(stringSlice.Range, mutable.Range);
    }

    [Fact]
    public void EqualityOperator_WithStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);
        var stringSlice = new StringSlice("Hello World", 0..5);
        var differentStringSlice = new StringSlice("Hello World", 1..6);

        // Assert
        Assert.True(mutable == stringSlice);
        Assert.True(stringSlice == mutable);
        Assert.False(mutable == differentStringSlice);
        Assert.False(differentStringSlice == mutable);
        Assert.False(mutable == (MutableStringSlice?)null);
        Assert.False(null == stringSlice);
    }

    [Fact]
    public void InequalityOperator_WithStringSlice_Works() {
        // Arrange
        var mutable = new MutableStringSlice("Hello World", 0..5);
        var stringSlice = new StringSlice("Hello World", 0..5);
        var differentStringSlice = new StringSlice("Hello World", 1..6);

        // Assert
        Assert.False(mutable != stringSlice);
        Assert.False(stringSlice != mutable);
        Assert.True(mutable != differentStringSlice);
        Assert.True(differentStringSlice != mutable);
        Assert.True(mutable != (MutableStringSlice?)null);
        Assert.True(null != stringSlice);
    }

    [Fact]
    public void AsSpan_ReturnsCorrectSpan() {
        // Arrange
        var slice = new MutableStringSlice("Hello World", 0..5);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal("Hello", new string(span));
        Assert.Equal(5, span.Length);
    }

    [Fact]
    public void AsSpan_WithMiddleRange_ReturnsCorrectSpan() {
        // Arrange
        var slice = new MutableStringSlice("Hello World", 6..11);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal("World", new string(span));
        Assert.Equal(5, span.Length);
    }

    [Fact]
    public void AsSpan_WithEmptyRange_ReturnsEmptySpan() {
        // Arrange
        var slice = new MutableStringSlice("Hello", 2..2);
        
        // Act
        var span = slice.AsSpan();
        
        // Assert
        Assert.Equal(0, span.Length);
        Assert.Equal("", new string(span));
    }
}
