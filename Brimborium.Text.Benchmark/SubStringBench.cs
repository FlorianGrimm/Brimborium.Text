using BenchmarkDotNet.Attributes;

namespace Brimborium.Text;

public class SubStringBench {
    
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
    public void BenchSubString() {
        var subString = new SubString(text);
        var limit = subString.Length / 2;
        for (var i = 0; i < limit; i++) {
            var subString2 = subString.GetSubString(i, i);
            if (subString2.Length != i) { throw new Exception(); }
        }
    }

}
