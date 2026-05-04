namespace Brimborium.Gerede;


public sealed class BGTokenizerExceptEOF<T>(
        IBGTokenizerResultCreate<T> selectResult
    ) : IBGTokenizer<T> {
    public IBGTokenizerResultCreate<T> SelectResult { get; } = selectResult;

    public bool TryGetToken(
        StringRange value, 
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (value.IsEmpty) {
            next = value;
            token = default;
            return false;
        } else {
            next = value.Substring(1);
            var tokenMatch = value.Substring(0, 1);
            var tokenValue = this.SelectResult.Select(tokenMatch);
            token = new(tokenMatch, tokenValue);
            return true;
        }
    }
}
