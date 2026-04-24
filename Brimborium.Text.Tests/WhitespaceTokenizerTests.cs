namespace Brimborium.Text;
public class WhitespaceTokenizerTests {
    [Test]
    public async Task Tokenize_EmptyString_ReturnsEmptyList() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = StringSlice.Empty;

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        await Assert.That(result.ListTokens).IsEmpty();
    }

    [Test]
    public async Task Tokenize_OneSpace_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice(" ");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        await Assert.That(result.ListTokens).IsEquivalentTo(new List<ITokenStringSlice<WhitespaceToken>>() { 
                new WhitespaceTokenStringSlice(" ", WhitespaceToken.Whitespace)
            });
    }

    [Test]
    public async Task Tokenize_TwoSpace_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice("  ");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        await Assert.That(result.ListTokens).IsEquivalentTo(new List<ITokenStringSlice<WhitespaceToken>>() {
                new WhitespaceTokenStringSlice("  ", WhitespaceToken.Whitespace)
            });
    }


    [Test]
    public async Task Tokenize_TwoSpaceWord_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice("  abc");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        await Assert.That(result.ListTokens).IsEquivalentTo(new List<ITokenStringSlice<WhitespaceToken>>() {
                new WhitespaceTokenStringSlice(input.Substring(0, 2), WhitespaceToken.Whitespace),
                new WhitespaceTokenStringSlice(input.Substring(2, 3), WhitespaceToken.Word)
            });
    }

}