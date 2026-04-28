using System.Diagnostics.CodeAnalysis;

namespace Brimborium.Gerede;

public static class BGTokenizer {
    public static BGTokenizerAcceptEOF<T> AcceptEOF<T>(
        T acceptValue
    ) => new BGTokenizerAcceptEOF<T>(acceptValue);

    public static BGTokenizerAcceptChar<T> AcceptChar<T>(
        char acceptChar,
        T acceptValue
    ) => new BGTokenizerAcceptChar<T>(acceptChar, acceptValue);

    public static BGTokenizerAcceptCharSet<T> AcceptCharSet<T>(
        IEnumerable<char> acceptCharSet,
        T acceptValue
    ) => new BGTokenizerAcceptCharSet<T>(acceptCharSet, acceptValue);

    public static BGTokenizerAcceptCharSet<BGVoid> AcceptCharSet(
        IEnumerable<char> acceptCharSet
    ) => new BGTokenizerAcceptCharSet<BGVoid>(acceptCharSet, new BGVoid());

    public static BGTokenizerPredicate<T> Predicate<T>(
        Func<char, bool> predicate,
        T acceptValue
    ) => new BGTokenizerPredicate<T>(predicate, acceptValue);


    public static BGTokenizerPredicate<BGVoid> Predicate(
        Func<char, bool> predicate
    ) => new BGTokenizerPredicate<BGVoid>(predicate, new BGVoid());

    public static BGTokenizerAcceptString<T> AcceptString<T>(
        string acceptText,
        T acceptValue
    ) => new BGTokenizerAcceptString<T>(acceptText, acceptValue);

    public static BGTokenizerAcceptString<BGVoid> AcceptString(
        string acceptText
    ) => new BGTokenizerAcceptString<BGVoid>(acceptText, new BGVoid());

    public static BGTokenizerRepeat<TResult, TInner> Repeat<TResult, TInner>(
        IBGTokenizer<TInner> tokenizer,
        int minElements,
        int maxElements,
        IBGFactoryAggregation<TResult, TInner> factoryAggregation
    ) => new BGTokenizerRepeat<TResult, TInner>(tokenizer, minElements, maxElements, factoryAggregation, factoryAggregation);

    public static BGTokenizerRepeat<TResult, TInner> Repeat<TResult, TInner>(
        IBGTokenizer<TInner> tokenizer,
        int minElements,
        int maxElements,
        IBGFactory<TResult> factory,
        IBGResultAggregation<TResult, TInner> aggregation
    ) => new BGTokenizerRepeat<TResult, TInner>(tokenizer, minElements, maxElements, factory, aggregation);


    public static BGTokenizerRepeat<BGVoid, BGVoid> Repeat(
        IBGTokenizer<BGVoid> tokenizer,
        int minElements,
        int maxElements
    ) => new BGTokenizerRepeat<BGVoid, BGVoid>(tokenizer, minElements, maxElements, BGFactoryAggregation.BGVoid());


    public static BGTokenizerOptional<T> Optional<T>(
        IBGTokenizer<T> tokenizer,
        T otherwiseValue
    ) => new BGTokenizerOptional<T>(tokenizer, otherwiseValue);

    public static BGTokenizerOr<T> Or<T>(
        IEnumerable<IBGTokenizer<T>> listTokenizer
    ) => new BGTokenizerOr<T>(listTokenizer);

    public static BGTokenizerListCombine<TResult, TInner> Combine<TResult, TInner>(
        IEnumerable<IBGTokenizer<TInner>> listTokenizer,
        IBGTokenizerListCombiner<TResult, TInner> selectResult
    ) => new BGTokenizerListCombine<TResult, TInner>(listTokenizer, selectResult);

    public static IBGTokenizer<TResult> Sequence<TResult, T1, T2>(
        IBGTokenizer<T1> tokenizer1,
        IBGTokenizer<T2> tokenizer2,
        IBGTokenizerCombiner<TResult, T1, T2> combiner
    ) => new BGTokenizerSequence<TResult, T1, T2>(tokenizer1, tokenizer2, combiner);

    public static IBGTokenizer<TResult> Sequence<TResult, T1, T2, T3>(
        IBGTokenizer<T1> tokenizer1,
        IBGTokenizer<T2> tokenizer2,
        IBGTokenizer<T3> tokenizer3,
        IBGTokenizerCombiner<TResult, T1, T2, T3> combiner
    ) => new BGTokenizerSequence<TResult, T1, T2, T3>(tokenizer1, tokenizer2, tokenizer3, combiner);
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
