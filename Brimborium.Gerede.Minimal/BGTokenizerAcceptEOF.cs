namespace Brimborium.Gerede;

public class BGTokenizerAcceptEOF : IBGTokenizer {
    public bool TryGetToken(
        StringRange value,
        out StringRange next) {
        var result = (value.IsEmpty);
        next = value;
        return result;
    }
}

public class BGTokenizerAcceptEOF<T> : IBGTokenizer<T> {
    public BGTokenizerAcceptEOF(
        T acceptValue
    ) {
        this.AcceptValue = acceptValue;
    }

    public T AcceptValue { get; set; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        var result = (value.IsEmpty);
        next = value;
        if (result) {
            token = new BGToken<T>(value, this.AcceptValue);
            return true;
        } else {
            token = default;
            return false;
        }
    }
}

