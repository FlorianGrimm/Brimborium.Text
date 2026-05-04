namespace Brimborium.Gerede;

public sealed class BGTokenizerCapture<T1, R>(
        IBGTokenizer<T1> tokenizer,
        IBGTokenizerResultTransform<T1, R> selectResult
    ) : IBGTokenizer<R> {
    public IBGTokenizer<T1> Tokenizer { get; } = tokenizer;

    public IBGTokenizerResultTransform<T1, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<R> token,
        out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out var token1, out var innerNext)) {
            var tokenMatch = value.Substring(0, innerNext.Start - value.Start);
            var tokenValue = this.SelectResult.Select(token1, tokenMatch);
            token = new BGToken<R>(tokenMatch, tokenValue);
            next = innerNext;
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}
