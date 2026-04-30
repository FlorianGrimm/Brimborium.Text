namespace Brimborium.Gerede;

public sealed class BGTokenizerExceptEOF : IBGTokenizer {
    public bool TryGetToken(StringRange value, out StringRange next) {
        if (value.IsEmpty) {
            next = value;
            return false;
        } else {
            next= value.Substring(1);
            return true;
        }
    }
}

public sealed class BGTokenizerExceptEOF<T> : IBGTokenizer<T> {
    public BGTokenizerExceptEOF(
        IBGTokenizerResultAccept<T> selectResult
        ) {
        this.SelectResult = selectResult;
    }

    public IBGTokenizerResultAccept<T> SelectResult { get; }

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
