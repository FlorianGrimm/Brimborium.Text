namespace Brimborium.Gerede;

public class BGTokenizerOr : IBGTokenizer {
    public BGTokenizerOr(IEnumerable<IBGTokenizer> listTokenizer) {
        this.ListTokenizer = listTokenizer.ToArray();
    }

    public IBGTokenizer[] ListTokenizer { get; }

    public bool TryGetToken(
        StringRange value,
        out StringRange next) {
        foreach (var tokenizer in this.ListTokenizer) {
            if (tokenizer.TryGetToken(value,  out next)) {
                return true;
            }
        }
        next = value;
        return false;
    }
}

public class BGTokenizerOr<T> : IBGTokenizer<T> {
    public BGTokenizerOr(IEnumerable<IBGTokenizer<T>> listTokenizer) {
        this.ListTokenizer = listTokenizer.ToArray();
    }

    public IBGTokenizer<T>[] ListTokenizer { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        foreach (var tokenizer in this.ListTokenizer) {
            if (tokenizer.TryGetToken(value, out token, out next)) {
                return true;
            }
        }
        token = default;
        next = value;
        return false;
    }
}