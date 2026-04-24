# Brimborium.Text

Utility/Helpers like StringSlice

## Brimborium.Text.StringSlice

A memory lightweight variation to create substrings, without copy the string value itself.
It's similar to ReadOnlySpan&lt;char&gt;, but it's based on a string and a Range.
It's public readonly struct, other than ReadOnlySpan&lt;char&gt;, you can pass it around also in async methods.
It is slower than ReadOnlySpan&lt;char&gt;

```C#
var helloWorld = new StringSlice("Hello World!");
var hello = helloWorld.Substring(0, 5);
var world = helloWorld.Substring(6, 5);
```

A benchmark:

```C#

    
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
```

The result:

| Method            | Mean        | Error     | StdDev      | Median      | Gen0    | Allocated |
|------------------ |------------:|----------:|------------:|------------:|--------:|----------:|
| BenchSystemString | 10,792.5 ns | 439.01 ns | 1,231.04 ns | 10,288.5 ns | 15.1367 |   63336 B |
| BenchStringSlice  |  3,413.5 ns |  66.89 ns |    84.60 ns |  3,373.1 ns |       - |         - |
| BenchStringAsSpan |    225.1 ns |   4.40 ns |     5.41 ns |    224.4 ns |       - |         - |

## Brimborium.Text.StringBuilderPool

A Microsoft.Extensions.ObjectPool variation to reuse StringBuilder.

## Brimborium.Text.StringSplice

StringSplice allows to replace parts of a string.

```C#
    var sut = new StringSplice("abc");
    var part = sut.CreatePart(1..2)!;
    
    var result0 = sut.ToString();
    // "ac"

    part.SetReplacementText("B");
    var result1 = sut.ToString();
    // "aBc"

    part.SetReplacementText("");
    var result2 = sut.ToString();
    // "ac"

```

## Test

```cmd
dotnet test
```

## Benchmark

```cmd
 dotnet run --project Brimborium.Text.Benchmark --configuration Release -- --memory
 dotnet run --project Brimborium.Text.Benchmark --configuration Release -- --warmupCount 2 --iterationCount 2
```