using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brimborium.Text;
public class WhitespaceTokenizerTests {
    [Fact]
    public void Tokenize_EmptyString_ReturnsEmptyList() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = StringSlice.Empty;

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        Assert.Empty(result.ListTokens);
    }

    [Fact]
    public void Tokenize_OneSpace_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice(" ");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        Assert.Equal<List<ITokenStringSlice<WhitespaceToken>>>(
            new List<ITokenStringSlice<WhitespaceToken>>() { 
                new WhitespaceTokenStringSlice(" ", WhitespaceToken.Whitespace)
            }, result.ListTokens);
    }

    [Fact]
    public void Tokenize_TwoSpace_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice("  ");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        Assert.Equal<List<ITokenStringSlice<WhitespaceToken>>>(
            new List<ITokenStringSlice<WhitespaceToken>>() {
                new WhitespaceTokenStringSlice("  ", WhitespaceToken.Whitespace)
            }, result.ListTokens);
    }


    [Fact]
    public void Tokenize_TwoSpaceWord_ReturnsSuccess() {
        // Arrange
        var tokenizer = new WhitespaceTokenizer();
        var input = new StringSlice("  abc");

        // Act
        var result = tokenizer.Tokenize(input);

        // Assert
        Assert.Equal<List<ITokenStringSlice<WhitespaceToken>>>(
            new List<ITokenStringSlice<WhitespaceToken>>() {
                new WhitespaceTokenStringSlice(input.Substring(0, 2), WhitespaceToken.Whitespace),
                new WhitespaceTokenStringSlice(input.Substring(2, 3), WhitespaceToken.Word)
            }, result.ListTokens);
    }

}
