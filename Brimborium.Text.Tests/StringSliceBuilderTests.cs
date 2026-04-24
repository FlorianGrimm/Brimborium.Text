namespace Brimborium.Text;

public class StringSliceBuilderTests {
    [Fact]
    public void Constructor_CreatesEmptyBuilder() {
        // Arrange & Act
        var builder = new StringSliceBuilder();

        // Assert
        Assert.Equal(string.Empty, builder.ToString());
    }

    [Fact]
    public void Append_String_AddsSliceCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();

        // Act
        builder.Append("Hello").Append(" ").Append("World");

        // Assert
        Assert.Equal("Hello World", builder.ToString());
    }

    [Fact]
    public void Append_StringSlice_AddsSliceCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();
        var slice1 = new StringSlice("Hello World", 0..5);  // "Hello"
        var slice2 = new StringSlice("Hello World", 6..);   // "World"

        // Act
        builder.Append(slice1).Append(" ").Append(slice2);

        // Assert
        Assert.Equal("Hello World", builder.ToString());
    }

    [Fact]
    public void Append_EmptySlice_IsIgnored() {
        // Arrange
        var builder = new StringSliceBuilder();
        var emptySlice = new StringSlice("abc", 1..1);  // empty slice

        // Act
        builder.Append("Hello").Append(emptySlice).Append("World");

        // Assert
        Assert.Equal("HelloWorld", builder.ToString());
    }

    [Fact]
    public void Append_MultipleSlices_PreservesOrder() {
        // Arrange
        var builder = new StringSliceBuilder();
        var slices = new[] {
            new StringSlice("12345", 0..2),  // "12"
            new StringSlice("12345", 2..4),  // "34"
            new StringSlice("12345", 4..5)   // "5"
        };

        // Act
        foreach (var slice in slices) {
            builder.Append(slice);
        }

        // Assert
        Assert.Equal("12345", builder.ToString());
    }

    [Fact]
    public void ToString_WithLargeInput_HandlesCapacityCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();
        var largeString = new string('x', 1000);
        var slice = new StringSlice(largeString);

        // Act
        builder.Append(slice);
        var result = builder.ToString();

        // Assert
        Assert.Equal(largeString, result);
        Assert.Equal(1000, result.Length);
    }

    [Fact]
    public void ToString_MultipleCalls_ReturnsSameResult() {
        // Arrange
        var builder = new StringSliceBuilder();
        builder.Append("Hello").Append(" ").Append("World");

        // Act
        var result1 = builder.ToString();
        var result2 = builder.ToString();

        // Assert
        Assert.Equal(result1, result2);
        Assert.Equal("Hello World", result1);
    }

    [Theory]
    [InlineData(new[] { "Hello", " ", "World" }, "Hello World")]
    [InlineData(new[] { "", "Test", "" }, "Test")]
    [InlineData(new string[] { }, "")]
    public void Append_VariousInputs_ProducesExpectedResult(string[] inputs, string expected) {
        // Arrange
        var builder = new StringSliceBuilder();

        // Act
        foreach (var input in inputs) {
            builder.Append(input);
        }

        // Assert
        Assert.Equal(expected, builder.ToString());
    }

    [Fact]
    public void Append_MixedSlicesAndStrings_ConcatenatesCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();
        var slice = new StringSlice("Hello World", 0..5);  // "Hello"

        // Act
        builder
            .Append(slice)        // "Hello"
            .Append(" ")          // space
            .Append("World")      // "World"
            .Append(new StringSlice("!!!!", 0..1));  // "!"

        // Assert
        Assert.Equal("Hello World!", builder.ToString());
    }
}