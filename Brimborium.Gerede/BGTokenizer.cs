using System.Diagnostics.CodeAnalysis;

namespace Brimborium.Gerede;

public static class BGTokenizer {
    public static BGTokenizerAcceptEOF<T> AcceptEOF<T>(
        T acceptValue
    ) => new BGTokenizerAcceptEOF<T>(
        acceptValue
    );

    public static BGTokenizerAcceptChar<T> AcceptChar<T>(
        char acceptChar,
        T acceptValue
    ) => new BGTokenizerAcceptChar<T>(
        acceptChar,
        acceptValue
    );

    public static BGTokenizerAcceptCharSet<T> AcceptCharSet<T>(
        char[] acceptCharSet,
        T acceptValue
    ) => new BGTokenizerAcceptCharSet<T>(
        acceptCharSet,
        acceptValue
    );

    public static BGTokenizerAcceptString<T> AcceptString<T>(
        string acceptText,
        T acceptValue
    ) => new BGTokenizerAcceptString<T>(
        acceptText,
        acceptValue
    );

    public static BGTokenizerRepeat<TResult, TInner> Repeat<TResult, TInner>(
        IBGTokenizer<TInner> tokenizer,
        int minElements,
        int maxElements,
        IBGFactory<TResult> factory,
        IBGResultAggregation<TResult, TInner> aggregation
    ) => new BGTokenizerRepeat<TResult, TInner>(
        tokenizer,
        minElements,
        maxElements,
        factory,
        aggregation
    );

    public static BGTokenizerOptional<T> Optional<T>(
        IBGTokenizer<T> tokenizer,
        T otherwiseValue
    ) => new BGTokenizerOptional<T>(
        tokenizer,
        otherwiseValue
    );
}

public readonly struct BGToken<T> {
    public readonly StringRange Match;
    public readonly T Value;

    public BGToken(
        StringRange match,
        T Value
        ) {
        this.Match = match;
        this.Value = Value;
    }
}

public interface IBGTokenizer<T> {
    bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next);
}
