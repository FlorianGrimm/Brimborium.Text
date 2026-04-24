namespace Brimborium.Text;

public class StringBuilderPoolTests {
    [Test]
    public async Task EnsureThatTheReusedStringBuilderIsEmpty() {
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
        await Assert.That(stringBuilderReused).IsSameReferenceAs(stringBuilder);

        // ensure the reused StringBuilder is empty
        await Assert.That(stringBuilderReused.Length).IsEqualTo(0);
        await Assert.That(stringBuilderReused.ToString()).IsEqualTo("");
    }
 }