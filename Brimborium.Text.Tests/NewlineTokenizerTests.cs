namespace Brimborium.Text;

public class NewlineTokenizerTests {
    [Fact]
    public void Tokenize_EmptyString_ReturnsEmptyList() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = StringSlice.Empty;
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Empty(result.ListTokens);
    }
    
    [Fact]
    public void Tokenize_SingleWord_ReturnsWordToken() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Hello");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Single(result.ListTokens);
        var token = result.ListTokens[0];
        Assert.Equal("Hello", token.Text.ToString());
        Assert.Equal(NewlineToken.Word, token.Kind);
    }
    
    [Fact]
    public void Tokenize_SingleNewline_ReturnsNewlineToken() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        
        // Act & Assert - LF
        {
            var input = new StringSlice("\n");
            var result = tokenizer.Tokenize(input);
            
            Assert.Single(result.ListTokens);
            var token = result.ListTokens[0];
            Assert.Equal("\n", token.Text.ToString());
            Assert.Equal(NewlineToken.Newline, token.Kind);
        }
        
        // Act & Assert - CR
        {
            var input = new StringSlice("\r");
            var result = tokenizer.Tokenize(input);
            
            Assert.Single(result.ListTokens);
            var token = result.ListTokens[0];
            Assert.Equal("\r", token.Text.ToString());
            Assert.Equal(NewlineToken.Newline, token.Kind);
        }
        
        // Act & Assert - CRLF
        {
            var input = new StringSlice("\r\n");
            var result = tokenizer.Tokenize(input);
            
            Assert.Single(result.ListTokens);
            var token = result.ListTokens[0];
            Assert.Equal("\r\n", token.Text.ToString());
            Assert.Equal(NewlineToken.Newline, token.Kind);
        }
    }
    
    [Fact]
    public void Tokenize_WordAndNewline_ReturnsBothTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Hello\n");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Equal(2, result.ListTokens.Count);
        
        var wordToken = result.ListTokens[0];
        Assert.Equal("Hello", wordToken.Text.ToString());
        Assert.Equal(NewlineToken.Word, wordToken.Kind);
        
        var newlineToken = result.ListTokens[1];
        Assert.Equal("\n", newlineToken.Text.ToString());
        Assert.Equal(NewlineToken.Newline, newlineToken.Kind);
    }
    
    [Fact]
    public void Tokenize_NewlineAndWord_ReturnsBothTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("\nHello");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Equal(2, result.ListTokens.Count);
        
        var newlineToken = result.ListTokens[0];
        Assert.Equal("\n", newlineToken.Text.ToString());
        Assert.Equal(NewlineToken.Newline, newlineToken.Kind);
        
        var wordToken = result.ListTokens[1];
        Assert.Equal("Hello", wordToken.Text.ToString());
        Assert.Equal(NewlineToken.Word, wordToken.Kind);
    }
    
    [Fact]
    public void Tokenize_MultipleNewlines_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("\r\n\n\r");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Equal(3, result.ListTokens.Count);
        
        Assert.Equal("\r\n", result.ListTokens[0].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[0].Kind);
        
        Assert.Equal("\n", result.ListTokens[1].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[1].Kind);
        
        Assert.Equal("\r", result.ListTokens[2].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[2].Kind);
    }
    
    [Fact]
    public void Tokenize_ComplexText_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Line1\r\nLine2\nLine3\rLine4");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Equal(7, result.ListTokens.Count);
        
        Assert.Equal("Line1", result.ListTokens[0].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[0].Kind);
        
        Assert.Equal("\r\n", result.ListTokens[1].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[1].Kind);
        
        Assert.Equal("Line2", result.ListTokens[2].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[2].Kind);
        
        Assert.Equal("\n", result.ListTokens[3].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[3].Kind);
        
        Assert.Equal("Line3", result.ListTokens[4].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[4].Kind);
        
        Assert.Equal("\r", result.ListTokens[5].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[5].Kind);
        
        Assert.Equal("Line4", result.ListTokens[6].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[6].Kind);
    }
    
    [Fact]
    public void Tokenize_WithPartialStringSlice_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var fullText = "Prefix\nLine1\r\nLine2\nSuffix";
        var input = new StringSlice(fullText, 7..19); // "Line1\r\nLine2"
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        Assert.Equal(3, result.ListTokens.Count);
        
        Assert.Equal("Line1", result.ListTokens[0].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[0].Kind);
        
        Assert.Equal("\r\n", result.ListTokens[1].Text.ToString());
        Assert.Equal(NewlineToken.Newline, result.ListTokens[1].Kind);
        
        Assert.Equal("Line2", result.ListTokens[2].Text.ToString());
        Assert.Equal(NewlineToken.Word, result.ListTokens[2].Kind);
    }

    [Fact]
    public void Tokenize_And_Join() { 
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Hello\nWorld\r\nThis is a test.\n");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        StringSliceBuilder builder = new StringSliceBuilder();
        foreach (var token in result.ListTokens) {
            if (token.Kind == NewlineToken.Word) {
                builder.Append("  ");
                builder.Append(token.Text);
            } else {
                builder.Append(token.Text);
            }
        }
        // Assert
        var actual = builder.ToString();
        Assert.Equal("  Hello\n  World\r\n  This is a test.\n", actual.ToString());
    }
}
