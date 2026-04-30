
namespace Brimborium.Gerede;

public class BGTokenizerOr<T> : IBGTokenizer<T> {
    public readonly IBGTokenizer<T>[] ListTokenizer;

    public BGTokenizerOr(IEnumerable<IBGTokenizer<T>> listTokenizer) {
        this.ListTokenizer = (listTokenizer is IBGTokenizer<T>[] list)
            ? list
            : listTokenizer.ToArray();
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        foreach (var tokenizer in this.ListTokenizer) {
            if (tokenizer.TryGetToken(value, out token, out next)) {
                return true;
            }
        }
        {
            token = default;
            next = value;
            return false;
        }
    }
}