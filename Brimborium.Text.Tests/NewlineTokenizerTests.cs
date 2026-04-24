namespace Brimborium.Text;

public class NewlineTokenizerTests {
    [Test]
    public async Task Tokenize_EmptyString_ReturnsEmptyList() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = StringSlice.Empty;
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens).IsEmpty();
    }
    
    [Test]
    public async Task Tokenize_SingleWord_ReturnsWordToken() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Hello");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens).HasSingleItem();
        var token = result.ListTokens[0];
        await Assert.That(token.Text.ToString()).IsEqualTo("Hello");
        await Assert.That(token.Kind).IsEqualTo(NewlineToken.Word);
    }
    
    [Test]
    public async Task Tokenize_SingleNewline_ReturnsNewlineToken() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        
        // Act & Assert - LF
        {
            var input = new StringSlice("\n");
            var result = tokenizer.Tokenize(input);
            
            await Assert.That(result.ListTokens).HasSingleItem();
            var token = result.ListTokens[0];
            await Assert.That(token.Text.ToString()).IsEqualTo("\n");
            await Assert.That(token.Kind).IsEqualTo(NewlineToken.Newline);
        }
        
        // Act & Assert - CR
        {
            var input = new StringSlice("\r");
            var result = tokenizer.Tokenize(input);
            
            await Assert.That(result.ListTokens).HasSingleItem();
            var token = result.ListTokens[0];
            await Assert.That(token.Text.ToString()).IsEqualTo("\r");
            await Assert.That(token.Kind).IsEqualTo(NewlineToken.Newline);
        }
        
        // Act & Assert - CRLF
        {
            var input = new StringSlice("\r\n");
            var result = tokenizer.Tokenize(input);
            
            await Assert.That(result.ListTokens).HasSingleItem();
            var token = result.ListTokens[0];
            await Assert.That(token.Text.ToString()).IsEqualTo("\r\n");
            await Assert.That(token.Kind).IsEqualTo(NewlineToken.Newline);
        }
    }
    
    [Test]
    public async Task Tokenize_WordAndNewline_ReturnsBothTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Hello\n");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens.Count).IsEqualTo(2);

        var wordToken = result.ListTokens[0];
        await Assert.That(wordToken.Text.ToString()).IsEqualTo("Hello");
        await Assert.That(wordToken.Kind).IsEqualTo(NewlineToken.Word);

        var newlineToken = result.ListTokens[1];
        await Assert.That(newlineToken.Text.ToString()).IsEqualTo("\n");
        await Assert.That(newlineToken.Kind).IsEqualTo(NewlineToken.Newline);
    }
    
    [Test]
    public async Task Tokenize_NewlineAndWord_ReturnsBothTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("\nHello");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens.Count).IsEqualTo(2);

        var newlineToken = result.ListTokens[0];
        await Assert.That(newlineToken.Text.ToString()).IsEqualTo("\n");
        await Assert.That(newlineToken.Kind).IsEqualTo(NewlineToken.Newline);

        var wordToken = result.ListTokens[1];
        await Assert.That(wordToken.Text.ToString()).IsEqualTo("Hello");
        await Assert.That(wordToken.Kind).IsEqualTo(NewlineToken.Word);
    }
    
    [Test]
    public async Task Tokenize_MultipleNewlines_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("\r\n\n\r");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens.Count).IsEqualTo(3);

        await Assert.That(result.ListTokens[0].Text.ToString()).IsEqualTo("\r\n");
        await Assert.That(result.ListTokens[0].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[1].Text.ToString()).IsEqualTo("\n");
        await Assert.That(result.ListTokens[1].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[2].Text.ToString()).IsEqualTo("\r");
        await Assert.That(result.ListTokens[2].Kind).IsEqualTo(NewlineToken.Newline);
    }
    
    [Test]
    public async Task Tokenize_ComplexText_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var input = new StringSlice("Line1\r\nLine2\nLine3\rLine4");
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens.Count).IsEqualTo(7);

        await Assert.That(result.ListTokens[0].Text.ToString()).IsEqualTo("Line1");
        await Assert.That(result.ListTokens[0].Kind).IsEqualTo(NewlineToken.Word);

        await Assert.That(result.ListTokens[1].Text.ToString()).IsEqualTo("\r\n");
        await Assert.That(result.ListTokens[1].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[2].Text.ToString()).IsEqualTo("Line2");
        await Assert.That(result.ListTokens[2].Kind).IsEqualTo(NewlineToken.Word);

        await Assert.That(result.ListTokens[3].Text.ToString()).IsEqualTo("\n");
        await Assert.That(result.ListTokens[3].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[4].Text.ToString()).IsEqualTo("Line3");
        await Assert.That(result.ListTokens[4].Kind).IsEqualTo(NewlineToken.Word);

        await Assert.That(result.ListTokens[5].Text.ToString()).IsEqualTo("\r");
        await Assert.That(result.ListTokens[5].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[6].Text.ToString()).IsEqualTo("Line4");
        await Assert.That(result.ListTokens[6].Kind).IsEqualTo(NewlineToken.Word);
    }
    
    [Test]
    public async Task Tokenize_WithPartialStringSlice_ReturnsCorrectTokens() {
        // Arrange
        var tokenizer = new NewlineTokenizer();
        var fullText = "Prefix\nLine1\r\nLine2\nSuffix";
        var input = new StringSlice(fullText, 7..19); // "Line1\r\nLine2"
        
        // Act
        var result = tokenizer.Tokenize(input);
        
        // Assert
        await Assert.That(result.ListTokens.Count).IsEqualTo(3);

        await Assert.That(result.ListTokens[0].Text.ToString()).IsEqualTo("Line1");
        await Assert.That(result.ListTokens[0].Kind).IsEqualTo(NewlineToken.Word);

        await Assert.That(result.ListTokens[1].Text.ToString()).IsEqualTo("\r\n");
        await Assert.That(result.ListTokens[1].Kind).IsEqualTo(NewlineToken.Newline);

        await Assert.That(result.ListTokens[2].Text.ToString()).IsEqualTo("Line2");
        await Assert.That(result.ListTokens[2].Kind).IsEqualTo(NewlineToken.Word);
    }

    [Test]
    public async Task Tokenize_And_Join() { 
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
        await Assert.That(actual.ToString()).IsEqualTo("  Hello\n  World\r\n  This is a test.\n");
    }
}