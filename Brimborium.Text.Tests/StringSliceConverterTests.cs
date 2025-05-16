using System.Text;
using System.Text.Json;

namespace Brimborium.Text;

public class StringSliceConverterTests {
    [Fact]
    public void TestSerializeWithStringSliceConverter() {
        var sut = new StringSlice("abc");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        var act = System.Text.Json.JsonSerializer.Serialize(sut, options);
        Assert.Equal(@"""abc""", act);
    }

    [Fact]
    public void TestSerializeWithoutStringSliceConverter() {
        var sut = new StringSlice("abc");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };

        var act = System.Text.Json.JsonSerializer.Serialize(sut, options);
        Assert.Equal("""{"Text":"abc","Range":{"Start":{"Value":0,"IsFromEnd":false},"End":{"Value":3,"IsFromEnd":false}}}""", act);
    }

    [Fact]
    public void TestDeserializeWithStringSliceConverter() {
        {
            var sut = @"""abc""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };
            options.Converters.Add(new StringSliceConverter());

            var act = System.Text.Json.JsonSerializer.Deserialize<StringSlice>(sut, options);
            Assert.Equal("abc", act.ToString());
        }
        {
            var sut = """{"Text":"abc"}""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };
            options.Converters.Add(new StringSliceConverter());

            var act = System.Text.Json.JsonSerializer.Deserialize<StringSlice>(sut, options);
            Assert.Equal("abc", act.ToString());
        }
    }

    [Fact]
    public void TestDeserializeWithoutStringSliceConverter() {
        {
            var sut = """{"Text":"abc"}""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };

            var act = System.Text.Json.JsonSerializer.Serialize<StringSlice>(sut, options);

            Assert.NotEqual("abc", act.ToString());
        }
    }
    

    
    [Fact]
    public void Serialize_StringSlice_UsesDirectStringFormat()
    {
        // Arrange
        var slice = new StringSlice("test data");

        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var json = JsonSerializer.Serialize(slice, options);

        // Assert
        Assert.Equal(@"""test data""", json);
    }


    [Theory]
    [InlineData("\"test data\"", "test data")]
    [InlineData("\"\"", "")]  // Empty string
    [InlineData("\"  \"", "  ")] // Whitespace
    [InlineData("\"\\\"escaped\\\"\"", "\"escaped\"")] // Escaped quotes
    public void Deserialize_DirectStringFormat_Success(string json, string expected)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        Assert.Equal(expected, slice.ToString());
    }

    [Theory]
    [InlineData("""{"Text":"test data"}""", "test data")]
    [InlineData("""{"Text":""}""", "")] // Empty string
    [InlineData("""{"Text":"  "}""", "  ")] // Whitespace
    [InlineData("""{"Text":"\"escaped\""}""", "\"escaped\"")] // Escaped quotes
    public void Deserialize_ObjectFormat_Success(string json, string expected)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        Assert.Equal(expected, slice.ToString());
    }

    [Fact]
    public void Deserialize_NullValue_ReturnsEmptySlice()
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Arrange
        var json = "null";

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        Assert.Equal(string.Empty, slice.ToString());
    }

    [Theory]
    [InlineData("{}")]  // Empty object
    [InlineData("""{"WrongProperty":"value"}""")] // Wrong property name
    [InlineData("42")] // Number instead of string
    [InlineData("true")] // Boolean instead of string
    [InlineData("""{"Text":42}""")] // Number in Text property
    [InlineData("""{"Text":true}""")] // Boolean in Text property
    public void Deserialize_InvalidFormat_ThrowsJsonException(string json)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act & Assert
        Assert.Throws<JsonException>(() => 
            JsonSerializer.Deserialize<StringSlice>(json, options));
    }

    [Fact]
    public void SerializeDeserialize_RoundTrip_PreservesValue()
    {
        // Arrange
        var original = new StringSlice("test data");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        Assert.Equal(original.ToString(), deserialized.ToString());
    }
}
