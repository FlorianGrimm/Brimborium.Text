using Brimborium.Text;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Brimborium.Gerede;

/// <summary>
/// Factory
/// </summary>
public static class BGParser {
    public static BGParserToken<T> Token<T>(
            IBGTokenizer<T> tokenizer
        ) => new BGParserToken<T>(
            tokenizer
        );

    //public static BGChain<T> StartWith<T>(
    //    IBGParser<T> parser
    //) => new BGChain<T>(
    //    parser
    //);

    public static BGParserRepeat<TResult, TInner> Repeat<TResult, TInner>(
            IBGParser<TInner> parserInner,
            int minElements,
            int maxElements,
            IBGFactoryAggregation<TResult, TInner> factoryAggregation
        ) => new BGParserRepeat<TResult, TInner>(
            parserInner,
            minElements,
            maxElements,
            factoryAggregation
        );

    public static BGParserRepeat<TResult, TInner> Repeat<TResult, TInner>(
            IBGParser<TInner> parserInner,
            int minElements,
            int maxElements,
            IBGFactory<TResult> factory,
            IBGResultAggregation<TResult, TInner> aggregation
        ) => new BGParserRepeat<TResult, TInner>(
            parserInner,
            minElements,
            maxElements,
            factory,
            aggregation
        );

    public static BGParserOr<T> Or<T>(
        IEnumerable<IBGParser<T>> listParser
    ) => new BGParserOr<T>(
         listParser
    );

    public static BGParserSequence<TResult, T1, T2> Sequence<TResult, T1, T2>(
            IBGParser<T1> parser1,
            IBGParser<T2> parser2,
            IBGCombiner<TResult, T1, T2> combine
        ) => new BGParserSequence<TResult, T1, T2>(
            parser1,
            parser2,
            combine
        );

    public static BGParserSequence<TResult, T1, T2, T3> Sequence<TResult, T1, T2, T3>(
            IBGParser<T1> parser1,
            IBGParser<T2> parser2,
            IBGParser<T3> parser3,
            IBGCombiner<TResult, T1, T2, T3> combine
        ) => new BGParserSequence<TResult, T1, T2, T3>(
            parser1,
            parser2,
            parser3,
            combine
        );

    public static BGParserSequence<TResult, T1, T2, T3, T4> Sequence<TResult, T1, T2, T3, T4>(
            IBGParser<T1> parser1,
            IBGParser<T2> parser2,
            IBGParser<T3> parser3,
            IBGParser<T4> parser4,
            IBGCombiner<TResult, T1, T2, T3, T4> combine
        ) => new BGParserSequence<TResult, T1, T2, T3, T4>(
            parser1,
            parser2,
            parser3,
            parser4,
            combine
        );

    public static BGParserSequence<TResult, T1, T2, T3, T4, T5> Sequence<TResult, T1, T2, T3, T4, T5>(
            IBGParser<T1> parser1,
            IBGParser<T2> parser2,
            IBGParser<T3> parser3,
            IBGParser<T4> parser4,
            IBGParser<T5> parser5,
            IBGCombiner<TResult, T1, T2, T3, T4, T5> combine
        ) => new BGParserSequence<TResult, T1, T2, T3, T4, T5>(
            parser1,
            parser2,
            parser3,
            parser4,
            parser5,
            combine
        );

    public static BGParserSequence<TResult, T1, T2, T3, T4, T5, T6> Sequence<TResult, T1, T2, T3, T4, T5, T6>(
            IBGParser<T1> parser1,
            IBGParser<T2> parser2,
            IBGParser<T3> parser3,
            IBGParser<T4> parser4,
            IBGParser<T5> parser5,
            IBGParser<T6> parser6,
            IBGCombiner<TResult, T1, T2, T3, T4, T5, T6> combine
        ) => new BGParserSequence<TResult, T1, T2, T3, T4, T5, T6>(
            parser1,
            parser2,
            parser3,
            parser4,
            parser5,
            parser6,
            combine
        );
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

public interface IBGParser<T> {
    bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next
        );
}
