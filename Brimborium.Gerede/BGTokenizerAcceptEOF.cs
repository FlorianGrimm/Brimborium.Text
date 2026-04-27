namespace Brimborium.Gerede;

public class BGTokenizerAcceptEOF<T>
    : IBGTokenizer<T> {
    public readonly T AcceptValue;

    public BGTokenizerAcceptEOF(
        T acceptValue
        ) {
        this.AcceptValue = acceptValue;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        var result = value.IsEmpty;
        token = new(value, this.AcceptValue);
        next = value;
        return result;
    }
}



