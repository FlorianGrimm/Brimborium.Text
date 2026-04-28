namespace Brimborium.Gerede;

public class BGTokenizerCapture<T> : IBGTokenizer<T> {
    public BGTokenizerCapture(
        IBGTokenizer tokenizer,
        IBGTokenizerResultAccept<T> selectResult
    ) {
        this.Tokenizer = tokenizer;
        this.SelectResult = selectResult;
    }

    public IBGTokenizer Tokenizer { get; }

    public IBGTokenizerResultAccept<T> SelectResult { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out var innerNext)) {
            var tokenMatch = value.SubString(0, innerNext.Start - value.Start);
            var tokenValue = this.SelectResult.Select(tokenMatch);
            token = new BGToken<T>(tokenMatch, tokenValue);
            next = innerNext;
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}


public class BGTokenizerCapture<T, I> : IBGTokenizer<T> {
    public BGTokenizerCapture(
        IBGTokenizer<I> tokenizer,
        IBGTokenizerResultAccept<T> selectResult
    ) {
        this.Tokenizer = tokenizer;
        this.SelectResult = selectResult;
    }

    public IBGTokenizer<I> Tokenizer { get; }

    public IBGTokenizerResultAccept<T> SelectResult { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out var _, out var innerNext)) {
            var tokenMatch = value.SubString(0, innerNext.Start - value.Start);
            var tokenValue = this.SelectResult.Select(tokenMatch);
            token = new BGToken<T>(tokenMatch, tokenValue);
            next = innerNext;
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}
