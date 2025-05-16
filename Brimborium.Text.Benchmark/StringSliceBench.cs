using BenchmarkDotNet.Attributes;

namespace Brimborium.Text;

public class StringSliceBench {
    
    private string text=string.Empty;

    [GlobalSetup]
    public void Setup() {
        this.text = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
    }

    [Benchmark]
    public void BenchSystemString() {
        var subString = text;
        var limit=subString.Length/2;
        for (var i = 0; i < limit; i++) {
            var subString2 = subString.Substring(i, i);
            if (subString2.Length != i) { throw new Exception(); }
        }
    }

    [Benchmark]
    public void BenchStringSlice() {
        var subString = new StringSlice(text);
        var limit = subString.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subString2 = subString.Substring(i, i);
            if (subString2.Length != i) { throw new Exception(); }
        }
    }


    [Benchmark]
    public void BenchStringAsSpan() {
        var subString = new StringSlice(text);
        var limit = subString.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subString2 = subString.AsSpan().Slice(i, i);
            if (subString2.Length != i) { throw new Exception(); }
        }
    }
}

