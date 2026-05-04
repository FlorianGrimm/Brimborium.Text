namespace Brimborium.Gerede;

public sealed class BGTokenizerAcceptEvery : IBGTokenizer<BGVoid> {
    private readonly int _AcceptLength;


    public BGTokenizerAcceptEvery(int acceptLength) {
        this._AcceptLength = acceptLength;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<BGVoid> token,
        out StringRange next) {
        if (value.IsEmpty) {
            token = default;
            next = value;
            return false;
        } else {
            var matchMatch = value.Substring(0, this._AcceptLength);
            token = new BGToken<BGVoid>(matchMatch, new BGVoid());
            next = value.Substring(this._AcceptLength);
            return true;
        }
    }
}