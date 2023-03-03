# Brimborium.Text

Utility/Helpers like StringSlice

## Brimborium.Text.StringSlice

A memory lightweight variation to create substrings, without copy the string value itself.
It's public readonly struct, other than ReadOnlySpan<char>, you can pass it around also in async methods.


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
    
```
|            Method |     Mean |     Error |    StdDev |   Gen0 | Allocated |
|------------------ |---------:|----------:|----------:|-------:|----------:|
| BenchSystemString | 4.963 μs | 0.0981 μs | 0.1049 μs | 5.0430 |   63336 B |
|  BenchStringSlice | 2.513 μs | 0.0408 μs | 0.0382 μs |      - |         - |



## Brimborium.Text.StringBuilderPool

Microsoft.Extensions.ObjectPool does not provide a default instance.

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

```
 dotnet run --configuration Release -- --memory
```