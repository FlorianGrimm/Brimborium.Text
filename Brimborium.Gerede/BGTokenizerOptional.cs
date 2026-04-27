namespace Brimborium.Gerede;

public class BGTokenizerOptional<T>
    : IBGTokenizer<T> {
    public readonly IBGTokenizer<T> Tokenizer;
    public readonly T OtherwiseValue;

    public BGTokenizerOptional(
        IBGTokenizer<T> tokenizer,
        T otherwiseValue) {
        this.Tokenizer = tokenizer;
        this.OtherwiseValue = otherwiseValue;
    }

    public bool TryGetToken(
        StringRange value, 
        [MaybeNullWhen(false)] out BGToken<T> token, 
        out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out token, out next)) {
            return true;
        } else {
            token = new BGToken<T>(value.SubString(0), this.OtherwiseValue);
            next = value;
            return true;
        }
    }
}