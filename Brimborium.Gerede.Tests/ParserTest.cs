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

    [Test]
    public async Task BGParser_Token_Factory_Match_Test() {
        var parser = BGParser.Token(new BGTokenizerAcceptString<int>("hello", 42));
        BGParserInput input = new(new("hello world"), new());
        var succ = parser.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(42);
        await Assert.That(ok.Match.ToString()).IsEqualTo("hello");
        await Assert.That(next.Input.ToString()).IsEqualTo(" world");
    }

    [Test]
    public async Task BGParser_Token_Factory_NoMatch_Test() {
        var parser = BGParser.Token(new BGTokenizerAcceptString<int>("xyz", 1));
        BGParserInput input = new(new("abc"), new());
        var succ = parser.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("abc");
    }

    [Test]
    public async Task BGParser_Repeat_MinZero_NoMatch_Succeeds_Test() {
        var inner = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var counter = new BGFactoryAggregationDelegation<int, int>(
            create: () => 0,
            aggregate: (_, sum) => sum + 1);
        var parser = BGParser.Repeat(inner, minElements: 0, maxElements: 5, counter);
        BGParserInput input = new(new("xyz"), new());
        var succ = parser.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(0);
        await Assert.That(next.Input.ToString()).IsEqualTo("xyz");
    }

    [Test]
    public async Task BGParser_Repeat_RespectsMaxElements_Test() {
        var inner = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var counter = new BGFactoryAggregationDelegation<int, int>(
            create: () => 0,
            aggregate: (_, sum) => sum + 1);
        var parser = BGParser.Repeat(inner, minElements: 0, maxElements: 2, counter);
        BGParserInput input = new(new("aaaaa"), new());
        var succ = parser.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(2);
        await Assert.That(next.Input.ToString()).IsEqualTo("aaa");
    }

    [Test]
    public async Task BGParser_Repeat_FactoryAggregationOverload_Test() {
        var inner = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var factory = new BGFactoryAggregationDelegation<int, int>(
            create: () => 100,
            aggregate: (v, sum) => sum + v);
        var parser = BGParser.Repeat<int, int>(
            inner, minElements: 1, maxElements: 5,
            factory: factory, aggregation: factory);
        BGParserInput input = new(new("aaab"), new());
        var succ = parser.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(103);
        await Assert.That(next.Input.ToString()).IsEqualTo("b");
    }

    [Test]
    public async Task BGParser_Sequence2_Success_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var seq = BGParser.Sequence(p1, p2,
            new BGCombiner<int, int, int>((a, b) => a * 10 + b));
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(12);
        await Assert.That(next.Input.ToString()).IsEqualTo("cdef");
    }

    [Test]
    public async Task BGParser_Sequence2_FirstFails_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('x', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var seq = BGParser.Sequence(p1, p2,
            new BGCombiner<int, int, int>((a, b) => a + b));
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("abcdef");
    }

    [Test]
    public async Task BGParser_Sequence2_SecondFails_RewindsInput_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('x', 2));
        var seq = BGParser.Sequence(p1, p2,
            new BGCombiner<int, int, int>((a, b) => a + b));
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("abcdef");
    }

    [Test]
    public async Task BGParser_Sequence3_Success_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var p3 = BGParser.Token(new BGTokenizerAcceptChar<int>('c', 3));
        var seq = BGParser.Sequence(p1, p2, p3,
            new BGCombiner<int, int, int, int>((a, b, c) => a + b + c));
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEqualTo(6);
        await Assert.That(next.Input.ToString()).IsEqualTo("def");
    }

    [Test]
    public async Task BGParser_Sequence3_LastFails_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var p3 = BGParser.Token(new BGTokenizerAcceptChar<int>('z', 3));
        var seq = BGParser.Sequence(p1, p2, p3,
            new BGCombiner<int, int, int, int>((a, b, c) => a + b + c));
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("abcdef");
    }

    [Test]
    public async Task BGParser_Or_Success_1_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var p3 = BGParser.Token(new BGTokenizerAcceptChar<int>('z', 3));
        var seq = BGParser.Or([p1, p2, p3]);
        BGParserInput input = new(new("abcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEquivalentTo(1);
        await Assert.That(next.Input.ToString()).IsEqualTo("bcdef");
    }

    [Test]
    public async Task BGParser_Or_Success_3_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var p3 = BGParser.Token(new BGTokenizerAcceptChar<int>('z', 3));
        var seq = BGParser.Or([p1, p2, p3]);
        BGParserInput input = new(new("zbcdef"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEquivalentTo(3);
        await Assert.That(next.Input.ToString()).IsEqualTo("bcdef");
    }

    [Test]
    public async Task BGParser_Or_Fails_Test() {
        var p1 = BGParser.Token(new BGTokenizerAcceptChar<int>('a', 1));
        var p2 = BGParser.Token(new BGTokenizerAcceptChar<int>('b', 2));
        var p3 = BGParser.Token(new BGTokenizerAcceptChar<int>('z', 3));
        var seq = BGParser.Or([p1, p2, p3]);
        BGParserInput input = new(new("123"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsFalse();
        await Assert.That(next.Input.ToString()).IsEqualTo("123");
    }

    [Test]
    public async Task IsLetterTest() {
        List<CharRange> result = new List<CharRange>();
        bool started = false;
        char firstChar = '\0';
        char lastChar = '\0';
        for (char c = char.MinValue; c < char.MaxValue; c++) {
            if (char.IsLetter(c)) {
                if (!started) {
                    started = true;
                    firstChar = c;
                    lastChar = c;
                } else {
                    lastChar = c;
                }
            } else {
                if (started) {
                    result.Add(new(firstChar, lastChar));
                    started = false;
                }
            }
        }
        var resultAsString = string.Join(", ", result.Select(i => i.ToString()));
        await Assert.That(resultAsString).IsEqualTo("'A'..'Z', 'a'..'z', 'ª'..'ª', 'µ'..'µ', 'º'..'º', 'À'..'Ö', 'Ø'..'ö', 'ø'..'ˁ', 'ˆ'..'ˑ', 'ˠ'..'ˤ', 'ˬ'..'ˬ', 'ˮ'..'ˮ', 'Ͱ'..'ʹ', 'Ͷ'..'ͷ', 'ͺ'..'ͽ', 'Ϳ'..'Ϳ', 'Ά'..'Ά', 'Έ'..'Ί', 'Ό'..'Ό', 'Ύ'..'Ρ', 'Σ'..'ϵ', 'Ϸ'..'ҁ', 'Ҋ'..'ԯ', 'Ա'..'Ֆ', 'ՙ'..'ՙ', 'ՠ'..'ֈ', 'א'..'ת', 'ׯ'..'ײ', 'ؠ'..'ي', 'ٮ'..'ٯ', 'ٱ'..'ۓ', 'ە'..'ە', 'ۥ'..'ۦ', 'ۮ'..'ۯ', 'ۺ'..'ۼ', 'ۿ'..'ۿ', 'ܐ'..'ܐ', 'ܒ'..'ܯ', 'ݍ'..'ޥ', 'ޱ'..'ޱ', 'ߊ'..'ߪ', 'ߴ'..'ߵ', 'ߺ'..'ߺ', 'ࠀ'..'ࠕ', 'ࠚ'..'ࠚ', 'ࠤ'..'ࠤ', 'ࠨ'..'ࠨ', 'ࡀ'..'ࡘ', 'ࡠ'..'ࡪ', 'ࡰ'..'ࢇ', 'ࢉ'..'ࢎ', 'ࢠ'..'ࣉ', 'ऄ'..'ह', 'ऽ'..'ऽ', 'ॐ'..'ॐ', 'क़'..'ॡ', 'ॱ'..'ঀ', 'অ'..'ঌ', 'এ'..'ঐ', 'ও'..'ন', 'প'..'র', 'ল'..'ল', 'শ'..'হ', 'ঽ'..'ঽ', 'ৎ'..'ৎ', 'ড়'..'ঢ়', 'য়'..'ৡ', 'ৰ'..'ৱ', 'ৼ'..'ৼ', 'ਅ'..'ਊ', 'ਏ'..'ਐ', 'ਓ'..'ਨ', 'ਪ'..'ਰ', 'ਲ'..'ਲ਼', 'ਵ'..'ਸ਼', 'ਸ'..'ਹ', 'ਖ਼'..'ੜ', 'ਫ਼'..'ਫ਼', 'ੲ'..'ੴ', 'અ'..'ઍ', 'એ'..'ઑ', 'ઓ'..'ન', 'પ'..'ર', 'લ'..'ળ', 'વ'..'હ', 'ઽ'..'ઽ', 'ૐ'..'ૐ', 'ૠ'..'ૡ', 'ૹ'..'ૹ', 'ଅ'..'ଌ', 'ଏ'..'ଐ', 'ଓ'..'ନ', 'ପ'..'ର', 'ଲ'..'ଳ', 'ଵ'..'ହ', 'ଽ'..'ଽ', 'ଡ଼'..'ଢ଼', 'ୟ'..'ୡ', 'ୱ'..'ୱ', 'ஃ'..'ஃ', 'அ'..'ஊ', 'எ'..'ஐ', 'ஒ'..'க', 'ங'..'ச', 'ஜ'..'ஜ', 'ஞ'..'ட', 'ண'..'த', 'ந'..'ப', 'ம'..'ஹ', 'ௐ'..'ௐ', 'అ'..'ఌ', 'ఎ'..'ఐ', 'ఒ'..'న', 'ప'..'హ', 'ఽ'..'ఽ', 'ౘ'..'ౚ', 'ౝ'..'ౝ', 'ౠ'..'ౡ', 'ಀ'..'ಀ', 'ಅ'..'ಌ', 'ಎ'..'ಐ', 'ಒ'..'ನ', 'ಪ'..'ಳ', 'ವ'..'ಹ', 'ಽ'..'ಽ', 'ೝ'..'ೞ', 'ೠ'..'ೡ', 'ೱ'..'ೲ', 'ഄ'..'ഌ', 'എ'..'ഐ', 'ഒ'..'ഺ', 'ഽ'..'ഽ', 'ൎ'..'ൎ', 'ൔ'..'ൖ', 'ൟ'..'ൡ', 'ൺ'..'ൿ', 'අ'..'ඖ', 'ක'..'න', 'ඳ'..'ර', 'ල'..'ල', 'ව'..'ෆ', 'ก'..'ะ', 'า'..'ำ', 'เ'..'ๆ', 'ກ'..'ຂ', 'ຄ'..'ຄ', 'ຆ'..'ຊ', 'ຌ'..'ຣ', 'ລ'..'ລ', 'ວ'..'ະ', 'າ'..'ຳ', 'ຽ'..'ຽ', 'ເ'..'ໄ', 'ໆ'..'ໆ', 'ໜ'..'ໟ', 'ༀ'..'ༀ', 'ཀ'..'ཇ', 'ཉ'..'ཬ', 'ྈ'..'ྌ', 'က'..'ဪ', 'ဿ'..'ဿ', 'ၐ'..'ၕ', 'ၚ'..'ၝ', 'ၡ'..'ၡ', 'ၥ'..'ၦ', 'ၮ'..'ၰ', 'ၵ'..'ႁ', 'ႎ'..'ႎ', 'Ⴀ'..'Ⴥ', 'Ⴧ'..'Ⴧ', 'Ⴭ'..'Ⴭ', 'ა'..'ჺ', 'ჼ'..'ቈ', 'ቊ'..'ቍ', 'ቐ'..'ቖ', 'ቘ'..'ቘ', 'ቚ'..'ቝ', 'በ'..'ኈ', 'ኊ'..'ኍ', 'ነ'..'ኰ', 'ኲ'..'ኵ', 'ኸ'..'ኾ', 'ዀ'..'ዀ', 'ዂ'..'ዅ', 'ወ'..'ዖ', 'ዘ'..'ጐ', 'ጒ'..'ጕ', 'ጘ'..'ፚ', 'ᎀ'..'ᎏ', 'Ꭰ'..'Ᏽ', 'ᏸ'..'ᏽ', 'ᐁ'..'ᙬ', 'ᙯ'..'ᙿ', 'ᚁ'..'ᚚ', 'ᚠ'..'ᛪ', 'ᛱ'..'ᛸ', 'ᜀ'..'ᜑ', 'ᜟ'..'ᜱ', 'ᝀ'..'ᝑ', 'ᝠ'..'ᝬ', 'ᝮ'..'ᝰ', 'ក'..'ឳ', 'ៗ'..'ៗ', 'ៜ'..'ៜ', 'ᠠ'..'ᡸ', 'ᢀ'..'ᢄ', 'ᢇ'..'ᢨ', 'ᢪ'..'ᢪ', 'ᢰ'..'ᣵ', 'ᤀ'..'ᤞ', 'ᥐ'..'ᥭ', 'ᥰ'..'ᥴ', 'ᦀ'..'ᦫ', 'ᦰ'..'ᧉ', 'ᨀ'..'ᨖ', 'ᨠ'..'ᩔ', 'ᪧ'..'ᪧ', 'ᬅ'..'ᬳ', 'ᭅ'..'ᭌ', 'ᮃ'..'ᮠ', 'ᮮ'..'ᮯ', 'ᮺ'..'ᯥ', 'ᰀ'..'ᰣ', 'ᱍ'..'ᱏ', 'ᱚ'..'ᱽ', 'ᲀ'..'ᲊ', 'Ა'..'Ჺ', 'Ჽ'..'Ჿ', 'ᳩ'..'ᳬ', 'ᳮ'..'ᳳ', 'ᳵ'..'ᳶ', 'ᳺ'..'ᳺ', 'ᴀ'..'ᶿ', 'Ḁ'..'ἕ', 'Ἐ'..'Ἕ', 'ἠ'..'ὅ', 'Ὀ'..'Ὅ', 'ὐ'..'ὗ', 'Ὑ'..'Ὑ', 'Ὓ'..'Ὓ', 'Ὕ'..'Ὕ', 'Ὗ'..'ώ', 'ᾀ'..'ᾴ', 'ᾶ'..'ᾼ', 'ι'..'ι', 'ῂ'..'ῄ', 'ῆ'..'ῌ', 'ῐ'..'ΐ', 'ῖ'..'Ί', 'ῠ'..'Ῥ', 'ῲ'..'ῴ', 'ῶ'..'ῼ', 'ⁱ'..'ⁱ', 'ⁿ'..'ⁿ', 'ₐ'..'ₜ', 'ℂ'..'ℂ', 'ℇ'..'ℇ', 'ℊ'..'ℓ', 'ℕ'..'ℕ', 'ℙ'..'ℝ', 'ℤ'..'ℤ', 'Ω'..'Ω', 'ℨ'..'ℨ', 'K'..'ℭ', 'ℯ'..'ℹ', 'ℼ'..'ℿ', 'ⅅ'..'ⅉ', 'ⅎ'..'ⅎ', 'Ↄ'..'ↄ', 'Ⰰ'..'ⳤ', 'Ⳬ'..'ⳮ', 'Ⳳ'..'ⳳ', 'ⴀ'..'ⴥ', 'ⴧ'..'ⴧ', 'ⴭ'..'ⴭ', 'ⴰ'..'ⵧ', 'ⵯ'..'ⵯ', 'ⶀ'..'ⶖ', 'ⶠ'..'ⶦ', 'ⶨ'..'ⶮ', 'ⶰ'..'ⶶ', 'ⶸ'..'ⶾ', 'ⷀ'..'ⷆ', 'ⷈ'..'ⷎ', 'ⷐ'..'ⷖ', 'ⷘ'..'ⷞ', 'ⸯ'..'ⸯ', '々'..'〆', '〱'..'〵', '〻'..'〼', 'ぁ'..'ゖ', 'ゝ'..'ゟ', 'ァ'..'ヺ', 'ー'..'ヿ', 'ㄅ'..'ㄯ', 'ㄱ'..'ㆎ', 'ㆠ'..'ㆿ', 'ㇰ'..'ㇿ', '㐀'..'䶿', '一'..'ꒌ', 'ꓐ'..'ꓽ', 'ꔀ'..'ꘌ', 'ꘐ'..'ꘟ', 'ꘪ'..'ꘫ', 'Ꙁ'..'ꙮ', 'ꙿ'..'ꚝ', 'ꚠ'..'ꛥ', 'ꜗ'..'ꜟ', 'Ꜣ'..'ꞈ', 'Ꞌ'..'ꟍ', 'Ꟑ'..'ꟑ', 'ꟓ'..'ꟓ', 'ꟕ'..'Ƛ', 'ꟲ'..'ꠁ', 'ꠃ'..'ꠅ', 'ꠇ'..'ꠊ', 'ꠌ'..'ꠢ', 'ꡀ'..'ꡳ', 'ꢂ'..'ꢳ', 'ꣲ'..'ꣷ', 'ꣻ'..'ꣻ', 'ꣽ'..'ꣾ', 'ꤊ'..'ꤥ', 'ꤰ'..'ꥆ', 'ꥠ'..'ꥼ', 'ꦄ'..'ꦲ', 'ꧏ'..'ꧏ', 'ꧠ'..'ꧤ', 'ꧦ'..'ꧯ', 'ꧺ'..'ꧾ', 'ꨀ'..'ꨨ', 'ꩀ'..'ꩂ', 'ꩄ'..'ꩋ', 'ꩠ'..'ꩶ', 'ꩺ'..'ꩺ', 'ꩾ'..'ꪯ', 'ꪱ'..'ꪱ', 'ꪵ'..'ꪶ', 'ꪹ'..'ꪽ', 'ꫀ'..'ꫀ', 'ꫂ'..'ꫂ', 'ꫛ'..'ꫝ', 'ꫠ'..'ꫪ', 'ꫲ'..'ꫴ', 'ꬁ'..'ꬆ', 'ꬉ'..'ꬎ', 'ꬑ'..'ꬖ', 'ꬠ'..'ꬦ', 'ꬨ'..'ꬮ', 'ꬰ'..'ꭚ', 'ꭜ'..'ꭩ', 'ꭰ'..'ꯢ', '가'..'힣', 'ힰ'..'ퟆ', 'ퟋ'..'ퟻ', '豈'..'舘', '並'..'龎', 'ﬀ'..'ﬆ', 'ﬓ'..'ﬗ', 'יִ'..'יִ', 'ײַ'..'ﬨ', 'שׁ'..'זּ', 'טּ'..'לּ', 'מּ'..'מּ', 'נּ'..'סּ', 'ףּ'..'פּ', 'צּ'..'ﮱ', 'ﯓ'..'ﴽ', 'ﵐ'..'ﶏ', 'ﶒ'..'ﷇ', 'ﷰ'..'ﷻ', 'ﹰ'..'ﹴ', 'ﹶ'..'ﻼ', 'Ａ'..'Ｚ', 'ａ'..'ｚ', 'ｦ'..'ﾾ', 'ￂ'..'ￇ', 'ￊ'..'ￏ', 'ￒ'..'ￗ', 'ￚ'..'ￜ'");
    }
    public record CharRange(char From, char To) {
        public override string ToString() {
            return $"'{From}'..'{To}'";
        }
    }

    [Test]
    public async Task T() {
        var TokenizerWhitespace = BGTokenizer.Repeat(
            BGTokenizer.AcceptCharSet(" \t\r\n"), 0, 1024);
        var TokenizerCommentMultiLineStart = BGTokenizer.Sequence(
                BGTokenizer.AcceptString("/*"),
                TokenizerWhitespace,
                new BGTokenizerCombinerDelegate<BGVoid, BGVoid, BGVoid>(
                    (_, _, _) => new BGVoid())
            );
        var TokenizerCommentMultiLineEnd = BGTokenizer.Sequence(
                TokenizerWhitespace,
                BGTokenizer.AcceptString("*/"),
                new BGTokenizerCombinerDelegate<BGVoid, BGVoid, BGVoid>((_, _, _) => new BGVoid())
            );

        var TokenizerPrefixMacro = BGTokenizer.AcceptString("#Macro:");
        var TokenizerName = BGTokenizer.Combine<string, BGVoid>(
            [
                BGTokenizer.Predicate(char.IsLetter),
                BGTokenizer.Repeat(
                    BGTokenizer.Predicate(char.IsLetterOrDigit),
                    1, 1024)
            ],
            new BGTokenizerListCombiner<string, BGVoid>(
                (_, match) => match.ToString()));

        //var ParserCommentMultiLineStart = BGParser.Sequence(
        //        BGParser.Token(TokenizerCommentMultiLineStart),
        //        BGParser.Token(TokenizerWhitespace),
        //        (_,_)=>);
        //var ParserCommentMultiLineStart = BGParser.Sequence(
        //        BGParser.Token(TokenizerWhitespace),
        //        BGParser.Token(TokenizerCommentMultiLineStop));

        var seq =
            BGParser.Sequence(
                BGParser.Token(TokenizerCommentMultiLineStart),
                BGParser.Token(TokenizerPrefixMacro),
                BGParser.Token(TokenizerName),
                BGParser.Token(TokenizerCommentMultiLineEnd),
                combine: BGCombine.Delegate<string, BGVoid, BGVoid, string, BGVoid>((_, _, name, _) => name));
        BGParserInput input = new(new("#Macro:ABC"), new());
        var succ = seq.Parse(input, out var ok, out var err, out var next);
        await Assert.That(succ).IsTrue();
        await Assert.That(ok.Value).IsEquivalentTo("ABC");
        await Assert.That(next.Input.ToString()).IsEqualTo("");
    }


    //[Test]
    //public async Task ChainTest() {
    //    var p = BGParser.StartWith(
    //        BGParser.Token(BGTokenizer.AcceptString("1", 1))
    //    ).Then(
    //        //BGParser.Token(BGTokenizer.AcceptString("+", "+"))
    //        BGParser.Token(BGTokenizer.AcceptString("2", 2))
    //    ).EndsWith(
    //        static (a, b) => (a + b)
    //    );
    //    BGParserInput input = new("12",new());
    //    var success = p.Parse(input, out var match, out var error, out var next);
    //    await Assert.That(success).IsTrue();
    //}
}
