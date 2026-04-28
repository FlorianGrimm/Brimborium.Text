namespace Brimborium.Gerede;

public interface IBGTokenizerCombiner<TResult, T1, T2, T3> {
    TResult Aggregate(
        in BGToken<T1> token1,
        in BGToken<T2> token2,
        in BGToken<T3> token3,
        StringRange match);
}

public class BGTokenizerCombinerDelegate<TResult, T1, T2, T3>
    : IBGTokenizerCombiner<TResult, T1, T2, T3> {
    public readonly Func<
        BGToken<T1>,
        BGToken<T2>,
        BGToken<T3>,
        StringRange,
        TResult> SelectResult;

    public BGTokenizerCombinerDelegate(
            Func<
                BGToken<T1>,
                BGToken<T2>,
                BGToken<T3>,
                StringRange,
                TResult> selectResult
        ) {
        this.SelectResult = selectResult;
    }

    public TResult Aggregate(
        in BGToken<T1> token1,
        in BGToken<T2> token2,
        in BGToken<T3> token3,
        StringRange match
        ) {
        return this.SelectResult(token1, token2, token3, match);
    }
}


public class BGTokenizerSequence<TResult, T1, T2, T3>
    : IBGTokenizer<TResult> {
    public readonly IBGTokenizer<T1> Tokenizer1;
    public readonly IBGTokenizer<T2> Tokenizer2;
    public readonly IBGTokenizer<T3> Tokenizer3;
    public readonly IBGTokenizerCombiner<TResult, T1, T2, T3> Combiner;

    public BGTokenizerSequence(
        IBGTokenizer<T1> tokenizer1,
        IBGTokenizer<T2> tokenizer2,
        IBGTokenizer<T3> tokenizer3,
        IBGTokenizerCombiner<TResult, T1, T2, T3> combiner
        ) {
        this.Tokenizer1 = tokenizer1;
        this.Tokenizer2 = tokenizer2;
        this.Tokenizer3 = tokenizer3;
        this.Combiner = combiner;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<TResult> token,
        out StringRange next) {
        if (this.Tokenizer1.TryGetToken(value, out var token1, out var next1)) {
            if (this.Tokenizer2.TryGetToken(next1, out var token2, out var next2)) {
                if (this.Tokenizer3.TryGetToken(next2, out var token3, out var next3)) {
                    var match = value.SubString(0, next3.End - value.Start);
                    var result = this.Combiner.Aggregate(token1, token2, token3, match);
                    token = new BGToken<TResult>(match, result);
                    next = next3;
                    return true;
                } else {
                    token = default;
                    next = next3;
                    return false;
                }
            } else {
                token = default;
                next = next2;
                return false;
            }
        } else {
            token = default;
            next = next1;
            return false;
        }
    }
}
