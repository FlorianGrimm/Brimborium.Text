namespace Brimborium.Gerede;

public static partial class BGParser {
    public static IBGParser<T> Initial<T>(
        T value
    ) => new BGParserInitialValue<T>(value);

    // Token
    public static IBGParser<T> Token<T>(
        IBGTokenizer<T> tokenizer
    ) => new BGParserTokenizer<T>(tokenizer);

    public static IBGParser<R> TokenT<T,R>(
        IBGTokenizer<T> tokenizer,
        IBGTokenizerResultTransform<T, R> selectResult
    ) => new BGParserTokenizerSelect<T, R>(tokenizer, selectResult);

    public static IBGParser<R> TokenT<T, R>(
        IBGTokenizer<T> tokenizer,
        Func<BGToken<T>, StringRange, R> selector
    ) => new BGParserTokenizerSelect<T, R>(
        tokenizer, 
        new BGTokenizerResultTransformDelegate<T, R>(selector));

    // Or (list overload; the binary fluent overload is provided by the extension block below)
    public static IBGParser<T> Or<T>(
        IEnumerable<IBGParser<T>> listParser
    ) => new BGParserOr<T>(listParser);

    public static IBGParser<T> Or<T>(
      params IBGParser<T>[] listParser
    ) => new BGParserOr<T>(listParser);


    public static IBGParser<T> Lazy<T>(
        Lazy<IBGParser<T>> lazyParser
    ) => new BGParserLazy<T>(lazyParser);

    public static IBGParser<T> Refer<T>(
        Func<IBGParser<T>?> getParser
    ) => new BGParserRefer<T>(getParser);


    extension<T>(IBGTokenizer<T> tokenizer) {
        public IBGParser<T> Parser() {
            return new BGParserTokenizer<T>(tokenizer);
        }
    }

    extension<T1>(IBGParser<T1> parser) {
        public IBGParser<R> PNext<T2, R>(
            IBGParser<T2> nextParser,
            IBGParserSequenceResult<T1, T2, R> selectResult
        ) => new BGParserSequence<T1, T2, R>(parser, nextParser, selectResult);

        public IBGParser<T1> POr(
            IBGParser<T1> alternative
        ) => new BGParserOr<T1>([parser, alternative]);

        public IBGParser<R> PAggregate<R>(
            IBGParserResultAggregate<T1, R> selectResult
        ) => new BGParserAggregate<T1, R>(parser, selectResult);

        public IBGParser<R> PAggregate<R>(
            Func<R> create,
            Func<R, T1, StringRange, R> aggregate
        ) => new BGParserAggregate<T1, R>(parser, new BGParserResultAggregateDelegate<T1, R>(create, aggregate));

        public IBGParser<R> PRepeat<R>(
            int minRepeat,
            int maxRepeat,
            IBGParserResultRepeat<T1, R> selectResult
        ) => new BGParserRepeat<T1, R>(parser, minRepeat, maxRepeat, selectResult);

        public IBGParser<R> PCapture<R>(
            IBGTokenizerResultCaptureTransform<T1, R> tokenizerResultCapture
        ) => new BGParserCapture<T1, R>(parser, tokenizerResultCapture);

        public IBGParser<R> PCapture<R>(
            Func<T1, StringRange, R> transform
        ) => new BGParserCapture<T1, R>(parser, new BGTokenizerResultCaptureDelegate<T1, R>(transform));
    }
}

public readonly struct BGResult<T>(
    StringRange match,
    T Value
    ) {
    public readonly StringRange Match = match;
    public readonly T Value = Value;
}

public record struct BGParserInput(
    StringRange Input,
    BGTokenList TokenList
    ) {
    public readonly BGParserInput With(StringRange next) {
        return new(next, TokenList);
    }
}

public interface IBGParser {
    bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<BGVoid> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next
        );
}


public interface IBGParser<T> {
    bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next
        );
}

public interface IBGParserTokenizerResult<T> {
    T Select(StringRange match);
}

public interface IBGParserResultRepeat<T, R> {
    R Select(IEnumerable<BGResult<T>> items, StringRange match);
}

public interface IBGParserResultAggregate<T, R> {
    R Create();
    R Aggregate(R accumulator, T currentValue, StringRange match);
}

