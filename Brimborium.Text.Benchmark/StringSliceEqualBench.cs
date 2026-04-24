using BenchmarkDotNet.Attributes;

namespace Brimborium.Text;

public class StringSliceEqualBench {

    private string textA = string.Empty;
    private string textB = string.Empty;

    [GlobalSetup]
    public void Setup() {
        this.textA = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
        this.textB = "012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789012345678901234567890123456789";
    }

    [Benchmark]
    public void BenchSystemString() {
        var subStringA = textA;
        var subStringB = textB;
        var limit = subStringB.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subStringA2 = subStringA.Substring(i, i);
            var subStringB2 = subStringB.Substring(i, i);
            if (!subStringA2.Equals(subStringB2, StringComparison.OrdinalIgnoreCase)) { throw new Exception(); }
        }
    }

    [Benchmark]
    public void BenchStringSlice() {
        var subStringA = new StringSlice(textA);
        var subStringB = new StringSlice(textB);
        var limit = subStringA.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subStringA2 = subStringA.Substring(i, i);
            var subStringB2 = subStringB.Substring(i, i);
            if (!subStringA2.Equals(subStringB2, StringComparison.OrdinalIgnoreCase)) { throw new Exception(); }
        }
    }


    [Benchmark]
    public void BenchStringAsSpan() {
        var subStringA = new StringSlice(textA);
        var subStringB = new StringSlice(textB);
        var limit = subStringA.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subStringA2 = subStringA.Substring(i, i);
            var subStringB2 = subStringB.Substring(i, i);
            if (!subStringA2.Equals(subStringB2, StringComparison.OrdinalIgnoreCase)) { throw new Exception(); }
        }
    }
}

