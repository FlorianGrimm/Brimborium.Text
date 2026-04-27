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

}
