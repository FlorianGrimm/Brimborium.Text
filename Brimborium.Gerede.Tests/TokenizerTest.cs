using Brimborium.Text;

using System;
using System.Collections.Generic;
using System.Text;

namespace Brimborium.Gerede.Tests;

public class TokenizerTests {
    [Test]
    public async Task AcceptString_Matches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.AcceptString("abc", true);
        var result=tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(true);
        await Assert.That(next.ToString()).IsEquatableTo("def");
    }

    [Test]
    public async Task AcceptString_Not_Matches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.AcceptString("123", true);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEquatableTo("abcdef");
    }

    [Test]
    public async Task Optional_Matches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.Optional(
            BGTokenizer.AcceptString("abc", true), 
            false);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(true);
        await Assert.That(next.ToString()).IsEquatableTo("def");
    }



    [Test]
    public async Task Optional_NotMatches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.Optional(
            BGTokenizer.AcceptString("123", true),
            false);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(false);
        await Assert.That(next.ToString()).IsEquatableTo("abcdef");
    }

    [Test]
    public async Task AcceptEOF_Empty_Test() {
        StringRange input = new(string.Empty);
        var tokenizer = BGTokenizer.AcceptEOF(true);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(true);
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptEOF_NonEmpty_Test() {
        StringRange input = new("abc");
        var tokenizer = BGTokenizer.AcceptEOF(true);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEquatableTo("abc");
    }

    [Test]
    public async Task AcceptChar_Matches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.AcceptChar('a', 1);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(1);
        await Assert.That(token.Match.ToString()).IsEquatableTo("a");
        await Assert.That(next.ToString()).IsEquatableTo("bcdef");
    }

    [Test]
    public async Task AcceptChar_NotMatches_Test() {
        StringRange input = new("abcdef");
        var tokenizer = BGTokenizer.AcceptChar('z', 1);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEquatableTo("abcdef");
    }

    [Test]
    public async Task AcceptChar_Empty_Test() {
        StringRange input = new(string.Empty);
        var tokenizer = BGTokenizer.AcceptChar('a', 1);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptCharSet_Matches_Test() {
        StringRange input = new("cab");
        var tokenizer = BGTokenizer.AcceptCharSet(new[] { 'a', 'b', 'c' }, 'X');
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo('X');
        await Assert.That(token.Match.ToString()).IsEquatableTo("c");
        await Assert.That(next.ToString()).IsEquatableTo("ab");
    }

    [Test]
    public async Task AcceptCharSet_NotMatches_Test() {
        StringRange input = new("zab");
        var tokenizer = BGTokenizer.AcceptCharSet(new[] { 'a', 'b', 'c' }, 'X');
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEquatableTo("zab");
    }

    [Test]
    public async Task AcceptCharSet_Empty_Test() {
        StringRange input = new(string.Empty);
        var tokenizer = BGTokenizer.AcceptCharSet(new[] { 'a', 'b' }, 'X');
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptString_Empty_Test() {
        StringRange input = new(string.Empty);
        var tokenizer = BGTokenizer.AcceptString("abc", true);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.IsEmpty).IsTrue();
    }

    [Test]
    public async Task AcceptString_OnSubRange_Test() {
        var input = new StringRange("xyzabcdef", 3, 9); // "abcdef"
        var tokenizer = BGTokenizer.AcceptString("abc", true);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Match.ToString()).IsEquatableTo("abc");
        await Assert.That(next.ToString()).IsEquatableTo("def");
    }

    [Test]
    public async Task Optional_OnEmpty_Test() {
        StringRange input = new(string.Empty);
        var tokenizer = BGTokenizer.Optional(
            BGTokenizer.AcceptString("abc", true),
            false);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(false);
        await Assert.That(next.IsEmpty).IsTrue();
    }

    private static BGFactoryAggregationDelegation<int, char> CountAggregation()
        => new(() => 0, (_, sum) => sum + 1);

    [Test]
    public async Task Repeat_ZeroOrMore_NoMatch_Test() {
        StringRange input = new("xyz");
        var counter = CountAggregation();
        var tokenizer = BGTokenizer.Repeat(
            BGTokenizer.AcceptChar('a', 'a'),
            minElements: 0,
            maxElements: 5,
            factory: counter,
            aggregation: counter);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(0);
        await Assert.That(token.Match.ToString()).IsEquatableTo(string.Empty);
        await Assert.That(next.ToString()).IsEquatableTo("xyz");
    }

    [Test]
    public async Task Repeat_ZeroOrMore_Matches_Test() {
        StringRange input = new("aaabc");
        var counter = CountAggregation();
        var tokenizer = BGTokenizer.Repeat(
            BGTokenizer.AcceptChar('a', 'a'),
            minElements: 0,
            maxElements: 10,
            factory: counter,
            aggregation: counter);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(3);
        await Assert.That(token.Match.ToString()).IsEquatableTo("aaa");
        await Assert.That(next.ToString()).IsEquatableTo("bc");
    }

    [Test]
    public async Task Repeat_AtLeastOne_WithMatches_Test() {
        StringRange input = new("aaabc");
        var counter = CountAggregation();
        var tokenizer = BGTokenizer.Repeat(
            BGTokenizer.AcceptChar('a', 'a'),
            minElements: 1,
            maxElements: 10,
            factory: counter,
            aggregation: counter);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(3);
        await Assert.That(token.Match.ToString()).IsEquatableTo("aaa");
        await Assert.That(next.ToString()).IsEquatableTo("bc");
    }

    [Test]
    public async Task Repeat_AtLeastOne_NoMatches_Test() {
        StringRange input = new("xyz");
        var counter = CountAggregation();
        var tokenizer = BGTokenizer.Repeat(
            BGTokenizer.AcceptChar('a', 'a'),
            minElements: 1,
            maxElements: 10,
            factory: counter,
            aggregation: counter);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsFalse();
        await Assert.That(next.ToString()).IsEquatableTo("xyz");
    }

    [Test]
    public async Task Repeat_RespectsMaxElements_Test() {
        StringRange input = new("aaaaa");
        var counter = CountAggregation();
        var tokenizer = BGTokenizer.Repeat(
            BGTokenizer.AcceptChar('a', 'a'),
            minElements: 0,
            maxElements: 2,
            factory: counter,
            aggregation: counter);
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(2);
        await Assert.That(token.Match.ToString()).IsEquatableTo("aa");
        await Assert.That(next.ToString()).IsEquatableTo("aaa");
    }

    [Test]
    public async Task Or_Matches_Test() {
        StringRange input = new("xyz123");
        var tokenizer = BGTokenizer.Or([
            BGTokenizer.AcceptString("abc", 1),
            BGTokenizer.AcceptString("def", 2),
            BGTokenizer.AcceptString("xyz", 3)]);
        
        var result = tokenizer.TryGetToken(input, out var token, out var next);
        await Assert.That(result).IsTrue();
        await Assert.That(token.Value).IsEquatableTo(3);
        await Assert.That(next.ToString()).IsEquatableTo("123");
    }

}
