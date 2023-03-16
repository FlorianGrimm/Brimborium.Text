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
        Assert.Equal("""{"Text":"abc"}""", act);
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
}
