

namespace Brimborium.Gerede;

public static class BGParser {
    // Token
    public static IBGParser<BGVoid> Token(
        IBGTokenizer tokenizer
    ) => new BGParserTokenizer(tokenizer);

    public static IBGParser<T> Token<T>(
        IBGTokenizer<T> tokenizer
    ) => new BGParserTokenizer<T>(tokenizer);

    // Or (list overload; the binary fluent overload is provided by the extension block below)
    public static IBGParser<T> Or<T>(
        IEnumerable<IBGParser<T>> listParser
    ) => new BGParserOr<T>(listParser);

    extension<T>(IBGTokenizer<T> tokenizer) {
        public IBGParser<T> Parser() {
            return new BGParserTokenizer<T>(tokenizer);
        }
    }

    extension(IBGParser<BGVoid> parser) {
        public IBGParser<BGVoid> Next(
            IBGParser<BGVoid> nextParser
        ) => throw new NotImplementedException(); //new BGParserNext<BGVoid, N, N>(parser, nextParser, selectResult);

        public IBGParser<N> Next<N>(
            IBGParser<N> nextParser
        //) => new BGParserNext<BGVoid, N, N>(parser, nextParser, selectResult);
        ) => throw new NotImplementedException();

        public IBGParser<N> Next<N>(
            IBGParser<N> nextParser,
            Func<StringRange, N, N> x
        //) => new BGParserNext<BGVoid, N, N>(parser, nextParser, selectResult);
        ) => throw new NotImplementedException();
    }

    extension<T>(IBGParser<T> parser) {
        public IBGParser<T> Next(
            IBGParser<BGVoid> nextParser
        ) => throw new NotImplementedException();

        public IBGParser<R> Next<N, R>(
            IBGParser<N> nextParser,
            IBGParserResultNext<T, N, R> selectResult
        ) => new BGParserNext<T, N, R>(parser, nextParser, selectResult);

        public IBGParser<T> Or(
            IBGParser<T> alternative
        ) => new BGParserOr<T>(new[] { parser, alternative });

        public IBGParser<R> Repeat<R>(
            int minRepeat,
            int maxRepeat,
            IBGParserResultRepeat<T, R> selectResult
        ) => new BGParserRepeat<T, R>(parser, minRepeat, maxRepeat, selectResult);
    }
}


public readonly struct BGResult<T> {
    public readonly StringRange Match;
    public readonly T Value;

    public BGResult(
        StringRange match,
        T Value
        ) {
        this.Match = match;
        this.Value = Value;
    }
}

public record struct BGParserInput(
    StringRange Input,
    BGTokenList TokenList
    ) {
    public BGParserInput With(StringRange next) {
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

public interface IBGParserResultNext<T, N, R> {
    R Select(BGResult<T> first, BGResult<N> next, StringRange match);
}

public interface IBGParserResultRepeat<T, R> {
    R Select(IReadOnlyList<BGResult<T>> items, StringRange match);
}
