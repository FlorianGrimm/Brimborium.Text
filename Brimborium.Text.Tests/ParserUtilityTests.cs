using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Brimborium.Text;

public class ParserUtilityTests {
    [Theory]
    [InlineData("MacroTest", "Macro", "Test", true)]
    [InlineData("macro test", "MACRO", " test", true)]  // Case insensitive by default
    [InlineData("Test", "Macro", "Test", false)]       // No match
    [InlineData("", "Test", "", false)]                // Empty source
    [InlineData("Test", "", "Test", true)]             // Empty search string
    [InlineData("MacroMacroTest", "Macro", "MacroTest", true)] // Multiple occurrences
    public void TrimLeftText_Various_Scenarios(string input, string lookingFor, string expected, bool expectedResult) {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftText(ref text, lookingFor.AsSpan());

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expected, text.ToString());
    }

    [Fact]
    public void TrimLeftText_CaseSensitive_DoesNotTrim() {
        // Arrange
        var text = new StringSlice("macroTest");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "MACRO".AsSpan(), StringComparison.Ordinal);

        // Assert
        Assert.False(result);
        Assert.Equal("macroTest", text.ToString());
    }

    [Fact]
    public void TrimLeftText_LongerSearchString_ReturnsFalse() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "TestLonger".AsSpan());

        // Assert
        Assert.False(result);
        Assert.Equal("Test", text.ToString());
    }

    [Fact]
    public void TrimLeftText_ExactMatch_ReturnsEmpty() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimLeftText(ref text, "Test".AsSpan());

        // Assert
        Assert.True(result);
        Assert.Equal("", text.ToString());
    }

    [Theory]
    [InlineData("TestMacro", "Macro", "Test", true)]
    [InlineData("test macro", "MACRO", "test ", true)]  // Case insensitive by default
    [InlineData("Test", "Macro", "Test", false)]       // No match
    [InlineData("", "Test", "", false)]                // Empty source
    [InlineData("Test", "", "Test", true)]             // Empty search string
    [InlineData("TestMacroMacro", "Macro", "TestMacro", true)] // Multiple occurrences
    public void TrimRightText_Various_Scenarios(string input, string lookingFor, string expected, bool expectedResult) {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightText(ref text, lookingFor.AsSpan());

        // Assert
        Assert.Equal(expectedResult, result);
        Assert.Equal(expected, text.ToString());
    }

    [Fact]
    public void TrimRightText_CaseSensitive_DoesNotTrim() {
        // Arrange
        var text = new StringSlice("macroTest");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "TEST".AsSpan(), StringComparison.Ordinal);

        // Assert
        Assert.False(result);
        Assert.Equal("macroTest", text.ToString());
    }

    [Fact]
    public void TrimRightText_LongerSearchString_ReturnsFalse() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "TestLonger".AsSpan());

        // Assert
        Assert.False(result);
        Assert.Equal("Test", text.ToString());
    }

    [Fact]
    public void TrimRightText_ExactMatch_ReturnsEmpty() {
        // Arrange
        var text = new StringSlice("Test");

        // Act
        var result = ParserUtility.TrimRightText(ref text, "Test".AsSpan());

        // Assert
        Assert.True(result);
        Assert.Equal("", text.ToString());
    }

    [Theory]
    [InlineData("  abc", "abc", true)]       // Basic whitespace
    [InlineData("\tabc", "abc", true)]       // Tab character
    [InlineData("abc", "abc", false)]        // No whitespace
    [InlineData("", "", false)]              // Empty string
    [InlineData(" \t abc", "abc", true)]     // Mixed whitespace
    [InlineData("\r abc", "\r abc", false)]  // Newline - should not trim
    [InlineData("\n abc", "\n abc", false)]  // Newline - should not trim
    [InlineData(" \r\n abc", "\r\n abc", true)] // Should only trim space before newline
    public void TrimLeftWhitespaceNotNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftWhitespaceNotNewLine(ref text);

        // Assert
        Assert.Equal(expected, text.ToString());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("   Hello", "Hello", true)]           // Basic whitespace
    [InlineData("\t\tHello", "Hello", true)]          // Tabs
    [InlineData("\r\nHello", "Hello", true)]          // Windows newline
    [InlineData("\nHello", "Hello", true)]            // Unix newline
    [InlineData("\r\n\t Hello", "Hello", true)]       // Mixed whitespace
    [InlineData("Hello", "Hello", false)]             // No whitespace
    [InlineData("", "", false)]                       // Empty string
    [InlineData(" ", "", true)]                       // Only whitespace
    public void TrimLeftWhitespaceWithNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimLeftWhitespaceWithNewLine(ref text);

        // Assert
        Assert.Equal(expected, text.ToString());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("abc   ", "abc", true)]           // Basic whitespace trimming
    [InlineData("abc\t  ", "abc", true)]          // Mixed spaces and tabs
    [InlineData("abc\n  ", "abc\n", true)]        // Preserves newline and following spaces
    [InlineData("abc\r\t", "abc\r", true)]        // Preserves carriage return and following tab
    [InlineData("abc", "abc", false)]             // No whitespace to trim
    [InlineData("   ", "", true)]                 // All whitespace
    [InlineData("", "", false)]                   // Empty string
    [InlineData("abc\u2000", "abc", true)]        // Unicode whitespace
    public void TrimRightWhitespaceNotNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightWhitespaceNotNewLine(ref text);

        // Assert
        Assert.Equal(expected, text.ToString());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData("abc   \t\r\n", "abc", true)]      // Mixed whitespace at end
    [InlineData("abc", "abc", false)]               // No whitespace
    [InlineData("   ", "", true)]                   // All whitespace
    [InlineData("", "", false)]                     // Empty string
    [InlineData("abc\r\n", "abc", true)]           // Windows line ending
    [InlineData("abc\n", "abc", true)]             // Unix line ending
    [InlineData("abc\t  ", "abc", true)]           // Tabs and spaces
    public void TrimRightWhitespaceWithNewLine_Various_Scenarios(string input, string expected, bool expectedResult)
    {
        // Arrange
        var text = new StringSlice(input);

        // Act
        var result = ParserUtility.TrimRightWhitespaceWithNewLine(ref text);

        // Assert
        Assert.Equal(expected, text.ToString());
        Assert.Equal(expectedResult, result);
    }

    [Theory]
    [InlineData(' ', true)]      // Space
    [InlineData('\t', true)]     // Tab
    [InlineData('\r', false)]    // Carriage return
    [InlineData('\n', false)]    // Line feed
    [InlineData('\u2000', true)] // En Quad (Unicode whitespace)
    [InlineData('a', false)]     // Regular character
    [InlineData('1', false)]     // Number
    [InlineData('_', false)]     // Symbol
    public void IsWhitespaceNotNewLine_ValidatesCharacters(char input, bool expected)
    {
        // Act
        var result = ParserUtility.IsWhitespaceNotNewLine(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData(' ', true)]      // Space
    [InlineData('\t', true)]     // Tab
    [InlineData('\r', true)]     // Carriage return
    [InlineData('\n', true)]     // Line feed
    [InlineData('\u2000', true)] // En Quad (Unicode whitespace)
    [InlineData('a', false)]     // Regular character
    [InlineData('1', false)]     // Number
    [InlineData('_', false)]     // Underscore
    [InlineData('\0', false)]    // Null character
    public void IsWhitespaceWithNewLine_ValidatesCharacters(char input, bool expected)
    {
        // Act
        var result = ParserUtility.IsWhitespaceWithNewLine(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("abc   ", 6, 3)]       // Basic case with spaces at end
    [InlineData("abc\t  ", 6, 3)]      // Mixed whitespace
    [InlineData("abc", 3, 3)]          // No whitespace
    [InlineData("   abc", 3, 0)]       // Stop at non-whitespace
    [InlineData("", 0, 0)]             // Empty string
    [InlineData("abc  \ndef", 5, 3)]   // Stop at newline
    public void GotoLeftWhileWhitespaceNotNewLine_Various_Scenarios(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoLeftWhileWhitespaceNotNewLine(input, startIndex);

        // Assert
        Assert.Equal(expectedIndex, result);
    }

    [Theory]
    [InlineData("   abc", 0, 3)]       // Basic whitespace at start
    [InlineData("abc   def", 3, 6)]    // Whitespace in middle
    [InlineData("abc", 0, 0)]          // No whitespace at start
    [InlineData("abc\ndef", 3, 3)]     // Newline should not be counted
    [InlineData("abc\t  def", 3, 6)]   // Mixed whitespace types
    [InlineData("   ", 0, 3)]          // All whitespace
    [InlineData("", 0, 0)]             // Empty string
    public void GotoRightWhileWhitespaceNotNewLine_Various_Scenarios(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoRightWhileWhitespaceNotNewLine(input, startIndex);

        // Assert
        Assert.Equal(expectedIndex, result);
    }

    [Theory]
    [InlineData("abc\r\ndef", 3, 5)]      // CRLF
    [InlineData("abc\rdef", 3, 4)]        // CR only
    [InlineData("abc\ndef", 3, 4)]        // LF only
    [InlineData("abcdef", 3, 3)]          // No newline
    [InlineData("abc\r\n", 3, 5)]         // CRLF at end
    [InlineData("", 0, 0)]                // Empty string
    [InlineData("abc", 5, 5)]             // Index beyond string length
    public void GotoRightIfNewline_HandlesVariousNewlines(string input, int startIndex, int expectedIndex)
    {
        // Act
        var result = ParserUtility.GotoRightIfNewline(input, startIndex);

        // Assert
        Assert.Equal(expectedIndex, result);
    }

    [Theory]
    [InlineData("abc", "abc", true)]  // Identical strings
    [InlineData("abc ", " abc", true)]  // Different whitespace, same content
    [InlineData("abc\ndef", "abc\ndef", true)]  // Multiple lines, identical
    [InlineData("abc\r\ndef", "abc\ndef", true)]  // Different line endings
    [InlineData("abc\n  def", "abc\ndef", true)]  // Different indentation
    [InlineData("abc\n\ndef", "abc\ndef", true)]  // Empty line difference
    [InlineData("abc", "def", false)]  // Different content
    [InlineData("abc\ndef", "abc\nxyz", false)]  // Different content in second line
    [InlineData("", "", true)]  // Empty strings
    [InlineData("  ", "  ", true)]  // Only whitespace
    [InlineData("abc\n", "abc", true)]  // Trailing newline difference
    public void EqualsLines_Various_Scenarios(string left, string right, bool expected)
    {
        // Arrange
        var leftSlice = new StringSlice(left);
        var rightSlice = new StringSlice(right);

        // Act
        var result = ParserUtility.EqualsLines(leftSlice, rightSlice);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void EqualsLines_WithMixedWhitespace_ReturnsTrue()
    {
        // Arrange
        var left = new StringSlice("  line1  \n\t  line2\t\t");
        var right = new StringSlice("line1\n  line2");

        // Act
        var result = ParserUtility.EqualsLines(left, right);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void EqualsLines_WithPartialSlices_ComparesCorrectly()
    {
        // Arrange
        var left = new StringSlice("prefix line1 \n line2 suffix", 7..19);  // "line1 \n line"
        var right = new StringSlice("line1\nline", 0..10);

        // Act
        var result = ParserUtility.EqualsLines(left, right);

        // Assert
        Assert.True(result);
    }
}
