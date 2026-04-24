using System.Text.Json;

namespace Brimborium.Text;

public class StringSliceConverterTests {
    [Test]
    public async Task TestSerializeWithStringSliceConverter() {
        var sut = new StringSlice("abc");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        var act = System.Text.Json.JsonSerializer.Serialize(sut, options);
        await Assert.That(act).IsEqualTo(@"""abc""");
    }

    [Test]
    public async Task TestSerializeWithoutStringSliceConverter() {
        var sut = new StringSlice("abc");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };

        var act = System.Text.Json.JsonSerializer.Serialize(sut, options);
        await Assert.That(act).IsEqualTo("""{"Text":"abc","Range":{"Start":{"Value":0,"IsFromEnd":false},"End":{"Value":3,"IsFromEnd":false}}}""");
    }

    [Test]
    public async Task TestDeserializeWithStringSliceConverter() {
        {
            var sut = @"""abc""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };
            options.Converters.Add(new StringSliceConverter());

            var act = System.Text.Json.JsonSerializer.Deserialize<StringSlice>(sut, options);
            await Assert.That(act.ToString()).IsEqualTo("abc");
        }
        {
            var sut = """{"Text":"abc"}""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };
            options.Converters.Add(new StringSliceConverter());

            var act = System.Text.Json.JsonSerializer.Deserialize<StringSlice>(sut, options);
            await Assert.That(act.ToString()).IsEqualTo("abc");
        }
    }

    [Test]
    public async Task TestDeserializeWithoutStringSliceConverter() {
        {
            var sut = """{"Text":"abc"}""";
            JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = false };

            var act = System.Text.Json.JsonSerializer.Serialize<StringSlice>(sut, options);

            await Assert.That(act.ToString()).IsNotEqualTo("abc");
        }
    }
    

    
    [Test]
    public async Task Serialize_StringSlice_UsesDirectStringFormat()
    {
        // Arrange
        var slice = new StringSlice("test data");

        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var json = JsonSerializer.Serialize(slice, options);

        // Assert
        await Assert.That(json).IsEqualTo(@"""test data""");
    }


    [Test]
    [Arguments("\"test data\"", "test data")]
    [Arguments("\"\"", "")]  // Empty string
    [Arguments("\"  \"", "  ")] // Whitespace
    [Arguments("\"\\\"escaped\\\"\"", "\"escaped\"")] // Escaped quotes
    public async Task Deserialize_DirectStringFormat_Success(string json, string expected)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        await Assert.That(slice.ToString()).IsEqualTo(expected);
    }

    [Test]
    [Arguments("""{"Text":"test data"}""", "test data")]
    [Arguments("""{"Text":""}""", "")] // Empty string
    [Arguments("""{"Text":"  "}""", "  ")] // Whitespace
    [Arguments("""{"Text":"\"escaped\""}""", "\"escaped\"")] // Escaped quotes
    public async Task Deserialize_ObjectFormat_Success(string json, string expected)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        await Assert.That(slice.ToString()).IsEqualTo(expected);
    }

    [Test]
    public async Task Deserialize_NullValue_ReturnsEmptySlice()
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Arrange
        var json = "null";

        // Act
        var slice = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        await Assert.That(slice.ToString()).IsEqualTo(string.Empty);
    }

    [Test]
    [Arguments("{}")]  // Empty object
    [Arguments("""{"WrongProperty":"value"}""")] // Wrong property name
    [Arguments("42")] // Number instead of string
    [Arguments("true")] // Boolean instead of string
    [Arguments("""{"Text":42}""")] // Number in Text property
    [Arguments("""{"Text":true}""")] // Boolean in Text property
    public void Deserialize_InvalidFormat_ThrowsJsonException(string json)
    {
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act & Assert
        Assert.Throws<JsonException>(() => 
            JsonSerializer.Deserialize<StringSlice>(json, options));
    }

    [Test]
    public async Task SerializeDeserialize_RoundTrip_PreservesValue()
    {
        // Arrange
        var original = new StringSlice("test data");
        JsonSerializerOptions options = new(System.Text.Json.JsonSerializerOptions.Default) { WriteIndented = true };
        options.Converters.Add(new StringSliceConverter());

        // Act
        var json = JsonSerializer.Serialize(original, options);
        var deserialized = JsonSerializer.Deserialize<StringSlice>(json, options);

        // Assert
        await Assert.That(deserialized.ToString()).IsEqualTo(original.ToString());
    }
}