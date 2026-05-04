namespace Brimborium.Gerede;

public sealed class BGTokenizerNext<T1, T2, R>(
        IBGTokenizer<T1> tokenizer1,
        IBGTokenizer<T2> tokenizer2,
        IBGTokenizerResultTransform<T1, T2, R> selectResult
    ) : IBGTokenizer<R> {
    public IBGTokenizer<T1> Tokenizer1 { get; } = tokenizer1;
    public IBGTokenizer<T2> Tokenizer2 { get; } = tokenizer2;
    public IBGTokenizerResultTransform<T1, T2, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<R> token, out StringRange next) {
        if (!this.Tokenizer1.TryGetToken(value, out var firstToken, out var afterFirst)) {
            token = default;
            next = value;
            return false;
        }
        if (!this.Tokenizer2.TryGetToken(afterFirst, out var nextToken, out var afterNext)) {
            token = default;
            next = value;
            return false;
        }
        {
            var match = value.Substring(0, afterNext.Start - value.Start);
            token = new BGToken<R>(match, this.SelectResult.Select(firstToken, nextToken, match));
            next = afterNext;
            return true;
        }
    }
}