namespace Brimborium.Text;

public class ParserUtilityTests {
    [Test]
    [Arguments("MacroTest", "Macro", "Test", true)]
    [Arguments("macro test", "MACRO", " test", true)]  // Case insensitive by default
    [Arguments("Test", "Macro", "Test", false)]       // No match
    [Arguments("", "Test", "", false)]                // Empty source
    [Arguments("Test", "", "Test", true)]             // Empty search string
    [Arguments("MacroMacroTest", "Macro", "MacroTest", true)] // Multiple occurrences
    public async Task TrimLeftText_Various_Scenarios(string input, string lookingFor, string expected, bool expectedResult) {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftText(ref text, lookingFor.AsSpan());

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
        await Assert.That(text.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task TrimLeftText_CaseSensitive_DoesNotTrim() {
        // Arrange
        var text = new StringSlice("macroTest");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "MACRO".AsSpan(), StringComparison.Ordinal);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(text.ToString()).IsEqualTo("macroTest");
    }

    [Test]
    public async Task TrimLeftText_LongerSearchString_ReturnsFalse() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "TestLonger".AsSpan());

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(text.ToString()).IsEqualTo("Test");
    }

    [Test]
    public async Task TrimLeftText_ExactMatch_ReturnsEmpty() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "Test".AsSpan());

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(text.ToString()).IsEqualTo("");
    }

    [Test]
    [Arguments("TestMacro", "Macro", "Test", true)]
    [Arguments("test macro", "MACRO", "test ", true)]  // Case insensitive by default
    [Arguments("Test", "Macro", "Test", false)]       // No match
    [Arguments("", "Test", "", false)]                // Empty source
    [Arguments("Test", "", "Test", true)]             // Empty search string
    [Arguments("TestMacroMacro", "Macro", "TestMacro", true)] // Multiple occurrences
    public async Task TrimRightText_Various_Scenarios(string input, string lookingFor, string expected, bool expectedResult) {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightText(ref text, lookingFor.AsSpan());

        // Assert
        await Assert.That(result).IsEqualTo(expectedResult);
        await Assert.That(text.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task TrimRightText_CaseSensitive_DoesNotTrim() {
        // Arrange
        var text = new StringSlice("macroTest");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "TEST".AsSpan(), StringComparison.Ordinal);

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(text.ToString()).IsEqualTo("macroTest");
    }

    [Test]
    public async Task TrimRightText_LongerSearchString_ReturnsFalse() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "TestLonger".AsSpan());

        // Assert
        await Assert.That(result).IsFalse();
        await Assert.That(text.ToString()).IsEqualTo("Test");
    }

    [Test]
    public async Task TrimRightText_ExactMatch_ReturnsEmpty() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "Test".AsSpan());

        // Assert
        await Assert.That(result).IsTrue();
        await Assert.That(text.ToString()).IsEqualTo("");
    }

    [Test]
    [Arguments("  abc", "abc", true)]       // Basic whitespace
    [Arguments("\tabc", "abc", true)]       // Tab character
    [Arguments("abc", "abc", false)]        // No whitespace
    [Arguments("", "", false)]              // Empty string
    [Arguments(" \t abc", "abc", true)]     // Mixed whitespace
    [Arguments("\r abc", "\r abc", false)]  // Newline - should not trim
    [Arguments("\n abc", "\n abc", false)]  // Newline - should not trim
    [Arguments(" \r\n abc", "\r\n abc", true)] // Should only trim space before newline
    public async Task TrimLeftWhitespaceNotNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftWhitespaceNotNewLine(ref text);

        // Assert
        await Assert.That(text.ToString()).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("   Hello", "Hello", true)]           // Basic whitespace
    [Arguments("\t\tHello", "Hello", true)]          // Tabs
    [Arguments("\r\nHello", "Hello", true)]          // Windows newline
    [Arguments("\nHello", "Hello", true)]            // Unix newline
    [Arguments("\r\n\t Hello", "Hello", true)]       // Mixed whitespace
    [Arguments("Hello", "Hello", false)]             // No whitespace
    [Arguments("", "", false)]                       // Empty string
    [Arguments(" ", "", true)]                       // Only whitespace
    public async Task TrimLeftWhitespaceWithNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftWhitespaceWithNewLine(ref text);

        // Assert
        await Assert.That(text.ToString()).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("abc   ", "abc", true)]           // Basic whitespace trimming
    [Arguments("abc\t  ", "abc", true)]          // Mixed spaces and tabs
    [Arguments("abc\n  ", "abc\n", true)]        // Preserves newline and following spaces
    [Arguments("abc\r\t", "abc\r", true)]        // Preserves carriage return and following tab
    [Arguments("abc", "abc", false)]             // No whitespace to trim
    [Arguments("   ", "", true)]                 // All whitespace
    [Arguments("", "", false)]                   // Empty string
    [Arguments("abc\u2000", "abc", true)]        // Unicode whitespace
    public async Task TrimRightWhitespaceNotNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightWhitespaceNotNewLine(ref text);

        // Assert
        await Assert.That(text.ToString()).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments("abc   \t\r\n", "abc", true)]      // Mixed whitespace at end
    [Arguments("abc", "abc", false)]               // No whitespace
    [Arguments("   ", "", true)]                   // All whitespace
    [Arguments("", "", false)]                     // Empty string
    [Arguments("abc\r\n", "abc", true)]           // Windows line ending
    [Arguments("abc\n", "abc", true)]             // Unix line ending
    [Arguments("abc\t  ", "abc", true)]           // Tabs and spaces
    public async Task TrimRightWhitespaceWithNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightWhitespaceWithNewLine(ref text);

        // Assert
        await Assert.That(text.ToString()).IsEqualTo(expected);
        await Assert.That(result).IsEqualTo(expectedResult);
    }

    [Test]
    [Arguments(' ', true)]      // Space
    [Arguments('\t', true)]     // Tab
    [Arguments('\r', false)]    // Carriage return
    [Arguments('\n', false)]    // Line feed
    [Arguments('\u2000', true)] // En Quad (Unicode whitespace)
    [Arguments('a', false)]     // Regular character
    [Arguments('1', false)]     // Number
    [Arguments('_', false)]     // Symbol
    public async Task IsWhitespaceNotNewLine_ValidatesCharacters(char input, bool expected)
    {
        // Act
        var result = ParserUtility.IsWhitespaceNotNewLine(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments(' ', true)]      // Space
    [Arguments('\t', true)]     // Tab
    [Arguments('\r', true)]     // Carriage return
    [Arguments('\n', true)]     // Line feed
    [Arguments('\u2000', true)] // En Quad (Unicode whitespace)
    [Arguments('a', false)]     // Regular character
    [Arguments('1', false)]     // Number
    [Arguments('_', false)]     // Underscore
    [Arguments('\0', false)]    // Null character
    public async Task IsWhitespaceWithNewLine_ValidatesCharacters(char input, bool expected)
    {
        // Act
        var result = ParserUtility.IsWhitespaceWithNewLine(input);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    [Arguments("abc   ", 6, 3)]       // Basic case with spaces at end
    [Arguments("abc\t  ", 6, 3)]      // Mixed whitespace
    [Arguments("abc", 3, 3)]          // No whitespace
    [Arguments("   abc", 3, 0)]       // Stop at non-whitespace
    [Arguments("", 0, 0)]             // Empty string
    [Arguments("abc  \ndef", 5, 3)]   // Stop at newline
    public async Task GotoLeftWhileWhitespaceNotNewLine_Various_Scenarios(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoLeftWhileWhitespaceNotNewLine(input, startIndex);

        // Assert
        await Assert.That(result).IsEqualTo(expectedIndex);
    }

    [Test]
    [Arguments("   abc", 0, 3)]       // Basic whitespace at start
    [Arguments("abc   def", 3, 6)]    // Whitespace in middle
    [Arguments("abc", 0, 0)]          // No whitespace at start
    [Arguments("abc\ndef", 3, 3)]     // Newline should not be counted
    [Arguments("abc\t  def", 3, 6)]   // Mixed whitespace types
    [Arguments("   ", 0, 3)]          // All whitespace
    [Arguments("", 0, 0)]             // Empty string
    public async Task GotoRightWhileWhitespaceNotNewLine_Various_Scenarios(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoRightWhileWhitespaceNotNewLine(input, startIndex);

        // Assert
        await Assert.That(result).IsEqualTo(expectedIndex);
    }

    [Test]
    [Arguments("abc\r\ndef", 3, 5)]      // CRLF
    [Arguments("abc\rdef", 3, 4)]        // CR only
    [Arguments("abc\ndef", 3, 4)]        // LF only
    [Arguments("abcdef", 3, 3)]          // No newline
    [Arguments("abc\r\n", 3, 5)]         // CRLF at end
    [Arguments("", 0, 0)]                // Empty string
    [Arguments("abc", 5, 5)]             // Index beyond string length
    public async Task GotoRightIfNewline_HandlesVariousNewlines(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoRightIfNewline(input, startIndex);

        // Assert
        await Assert.That(result).IsEqualTo(expectedIndex);
    }

    [Test]
    [Arguments("abc", "abc", true)]  // Identical strings
    [Arguments("abc ", " abc", true)]  // Different whitespace, same content
    [Arguments("abc\ndef", "abc\ndef", true)]  // Multiple lines, identical
    [Arguments("abc\r\ndef", "abc\ndef", true)]  // Different line endings
    [Arguments("abc\n  def", "abc\ndef", true)]  // Different indentation
    [Arguments("abc\n\ndef", "abc\ndef", true)]  // Empty line difference
    [Arguments("abc", "def", false)]  // Different content
    [Arguments("abc\ndef", "abc\nxyz", false)]  // Different content in second line
    [Arguments("", "", true)]  // Empty strings
    [Arguments("  ", "  ", true)]  // Only whitespace
    [Arguments("abc\n", "abc", true)]  // Trailing newline difference
    public async Task EqualsLines_Various_Scenarios(string left, string right, bool expected)
    {
        // Arrange
        var leftSlice = new StringSlice(left);
        var rightSlice = new StringSlice(right);

        // Act
        var result = ParserUtility.EqualsLines(leftSlice, rightSlice);

        // Assert
        await Assert.That(result).IsEqualTo(expected);
    }

    [Test]
    public async Task EqualsLines_WithMixedWhitespace_ReturnsTrue()
    {
        // Arrange
        var left = new StringSlice("  line1  \n\t  line2\t\t");
        var right = new StringSlice("line1\n  line2");

        // Act
        var result = ParserUtility.EqualsLines(left, right);

        // Assert
        await Assert.That(result).IsTrue();
    }

    [Test]
    public async Task EqualsLines_WithPartialSlices_ComparesCorrectly()
    {
        // Arrange
        var left = new StringSlice("prefix line1 \n line2 suffix", 7..19);  // "line1 \n line"
        var right = new StringSlice("line1\nline", 0..10);

        // Act
        var result = ParserUtility.EqualsLines(left, right);

        // Assert
        await Assert.That(result).IsTrue();
    }
}