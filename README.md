# Brimborium.Text

Helpers like SubString

## Brimborium.Text.SubString

A memory lightweight variation to create substrings, without copy the string value itself.
It's public readonly struct, other than ReadOnlySpan<char>, you can pass it around also in async methods.

## Brimborium.Text.StringBuilderPool

Microsoft.Extensions.ObjectPool does not provide a default instance.

## Brimborium.Text.StringSplice

StringSplice allows to replace parts of a string.

```CSharp
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