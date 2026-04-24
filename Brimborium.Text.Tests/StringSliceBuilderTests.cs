namespace Brimborium.Text;

public class StringSliceBuilderTests {
    [Test]
    public async Task Constructor_CreatesEmptyBuilder() {
        // Arrange & Act
        var builder = new StringSliceBuilder();

        // Assert
        await Assert.That(builder.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    public async Task Append_String_AddsSliceCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();

        // Act
        builder.Append("Hello").Append(" ").Append("World");

        // Assert
        await Assert.That(builder.ToString()).IsEqualTo("Hello World");
    }

    [Test]
    public async Task Append_StringSlice_AddsSliceCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();
        var slice1 = new StringSlice("Hello World", 0..5);  // "Hello"
        var slice2 = new StringSlice("Hello World", 6..);   // "World"

        // Act
        builder.Append(slice1).Append(" ").Append(slice2);

        // Assert
        await Assert.That(builder.ToString()).IsEqualTo("Hello World");
    }

    [Test]
    public async Task Append_EmptySlice_IsIgnored() {
        // Arrange
        var builder = new StringSliceBuilder();
        var emptySlice = new StringSlice("abc", 1..1);  // empty slice

        // Act
        builder.Append("Hello").Append(emptySlice).Append("World");

        // Assert
        await Assert.That(builder.ToString()).IsEqualTo("HelloWorld");
    }

    [Test]
    public async Task Append_MultipleSlices_PreservesOrder() {
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
        await Assert.That(builder.ToString()).IsEqualTo("12345");
    }

    [Test]
    public async Task ToString_WithLargeInput_HandlesCapacityCorrectly() {
        // Arrange
        var builder = new StringSliceBuilder();
        var largeString = new string('x', 1000);
        var slice = new StringSlice(largeString);

        // Act
        builder.Append(slice);
        var result = builder.ToString();

        // Assert
        await Assert.That(result).IsEqualTo(largeString);
        await Assert.That(result.Length).IsEqualTo(1000);
    }

    [Test]
    public async Task ToString_MultipleCalls_ReturnsSameResult() {
        // Arrange
        var builder = new StringSliceBuilder();
        builder.Append("Hello").Append(" ").Append("World");

        // Act
        var result1 = builder.ToString();
        var result2 = builder.ToString();

        // Assert
        await Assert.That(result2).IsEqualTo(result1);
        await Assert.That(result1).IsEqualTo("Hello World");
    }

    [Test]
    [Arguments(new[] { "Hello", " ", "World" }, "Hello World")]
    [Arguments(new[] { "", "Test", "" }, "Test")]
    [Arguments(new string[] { }, "")]
    public async Task Append_VariousInputs_ProducesExpectedResult(string[] inputs, string expected) {
        // Arrange
        var builder = new StringSliceBuilder();

        // Act
        foreach (var input in inputs) {
            builder.Append(input);
        }

        // Assert
        await Assert.That(builder.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task Append_MixedSlicesAndStrings_ConcatenatesCorrectly() {
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
        await Assert.That(builder.ToString()).IsEqualTo("Hello World!");
    }
}