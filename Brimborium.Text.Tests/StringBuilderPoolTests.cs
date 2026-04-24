namespace Brimborium.Text;

public class StringBuilderPoolTests {
    [Fact]
    public void EnsureThatTheReusedStringBuilderIsEmpty() {
        // Arrange
        // using a new StringBuilderPool() to avoid concurrent interference
        var stringBuilderPool = new StringBuilderPool();
        var stringBuilder = stringBuilderPool.Get();

        // Act
        stringBuilder.Append("test");
        stringBuilderPool.Return(stringBuilder);
        var stringBuilderReused = stringBuilderPool.Get();

        // Assert
        // ensure the reused StringBuilder is the same instance as the original
        Assert.Same(stringBuilder, stringBuilderReused);

        // ensure the reused StringBuilder is empty
        Assert.Equal(0, stringBuilderReused.Length);
        Assert.Equal("", stringBuilderReused.ToString());
    }
 }
