namespace Brimborium.Gerede;

public sealed class BGTokenizerAcceptEOF<T>(
        T acceptValue
    ) : IBGTokenizer<T> {
    public T AcceptValue { get; set; } = acceptValue;

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

