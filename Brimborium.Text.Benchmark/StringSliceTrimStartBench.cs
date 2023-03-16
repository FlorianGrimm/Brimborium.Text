using BenchmarkDotNet.Attributes;

namespace Brimborium.Text;

public class StringSliceTrimStartBench {
    
    private string text=string.Empty;

    const int LengthSpace = 10;
    const int LengthA = 1000;
    const int NumberOfIterations = 1000;

    [GlobalSetup]
    public void Setup() {
        this.text = new string(' ', LengthSpace) + new string('a', LengthA);
    }

    [Benchmark]
    public void BenchSystemString() {
        var subString = text;
        for (var i = 0; i < NumberOfIterations; i++) {
            var subString2 = subString.TrimStart();
            if (subString2.Length != LengthA) { throw new Exception(); }
        }
    }

    [Benchmark]
    public void BenchStringSlice() {
        var subString = new StringSlice(text);
        for (var i = 0; i < NumberOfIterations; i++) {
            var subString2 = subString.TrimStart();
            if (subString2.Length != LengthA) { throw new Exception(); }
        }
    }

}

