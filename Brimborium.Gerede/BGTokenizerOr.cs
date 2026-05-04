#pragma warning disable IDE0305 // Simplify collection initialization

namespace Brimborium.Gerede;

public sealed class BGTokenizerOr<T>(
        IEnumerable<IBGTokenizer<T>> listTokenizer
    ) : IBGTokenizer<T> {
    public IBGTokenizer<T>[] ListTokenizer { get; } = listTokenizer.ToArray();

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
