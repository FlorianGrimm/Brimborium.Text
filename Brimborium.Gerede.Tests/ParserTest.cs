using Brimborium.Text;

namespace Brimborium.Gerede;

public class ParserTest {
    [Test]
    public async Task BGTokenizerAcceptCharTest() {
        BGTokenizerAcceptChar<bool> tokenizer = new('A', true);
        BGParserToken<bool> parser = new(tokenizer);
        {
            BGParserInput inputA = new(new("A"), new());
            var succ = parser.Parse(inputA, out var ok, out var err, out var next);
            await Assert.That(succ).IsTrue();
            //await Assert.That(ok).IsTrue();
            //await Assert.That(err).IsTrue();
        }
        {
            BGParserInput inputB = new(new("B"), new());
            var succ = parser.Parse(inputB, out var ok, out var err, out var next);
            await Assert.That(succ).IsFalse();
        }
    }
    [Test]
    public async Task BGTokenizerAcceptStringTest() {
        BGTokenizerAcceptString<bool> tokenizer = new("A", true);
        BGParserToken<bool> parser = new(tokenizer);
        {
            BGParserInput inputA = new(new("A"), new());
            var succ = parser.Parse(inputA, out var ok, out var err, out var next);
            await Assert.That(succ).IsTrue();
            //await Assert.That(ok).IsTrue();
            //await Assert.That(err).IsTrue();
        }
        {
            BGParserInput inputAA = new(new("AA"), new());
            var succ = parser.Parse(inputAA, out var ok, out var err, out var next);
            await Assert.That(succ).IsTrue();
            //await Assert.That(ok).IsTrue();
            //await Assert.That(err).IsTrue();
        }
        {
            BGParserInput inputB = new(new("BA"), new());
            var succ = parser.Parse(inputB, out var ok, out var err, out var next);
            await Assert.That(succ).IsFalse();
        }
    }
    [Test]
    public async Task BGParserRepeatTest() {
        BGTokenizerAcceptString<bool> tokenizer = new("AB", true);
        BGParserToken<bool> parserToken = new(tokenizer);
        BGParserRepeat<int, bool> parser = new(parserToken, 1, 3,
            new BGFactoryAggregationDelegation<int, bool>(
                create: () => 0,
                aggregate: (_, current) => current + 1
                ));
        {
            BGParserInput inputA = new(new("AB"), new());

            var succ = parser.Parse(inputA, out var ok, out var err, out var next);
            await Assert.That(succ).IsTrue();
            await Assert.That(ok.Value).IsEqualTo(1);
            //await Assert.That(err).IsTrue();
        }
        {
            BGParserInput inputAA = new(new("ABABABAB"), new());
            var succ = parser.Parse(inputAA, out var ok, out var err, out var next);
            await Assert.That(succ).IsTrue();
            await Assert.That(ok.Value).IsEqualTo(3);
            //await Assert.That(err).IsTrue();
        }
        {
            BGParserInput inputB = new(new("BB"), new());
            var succ = parser.Parse(inputB, out var ok, out var err, out var next);
            await Assert.That(succ).IsFalse();
        }
    }
}