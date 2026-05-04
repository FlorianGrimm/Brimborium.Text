#pragma warning disable IDE0305 // Simplify collection initialization

using System.Buffers;

namespace Brimborium.Gerede;

public sealed class BGTokenizerAcceptChar<T> : IBGTokenizer<T> {
    public BGTokenizerAcceptChar(
        IEnumerable<char> acceptChar,
        IBGTokenizerResultCreate<T> selectResult) {
        var listValue = (acceptChar is char[] array) ? array : acceptChar.ToArray();
        this.AcceptChar = listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
        this.SelectResult = selectResult;
    }

    public char[] AcceptChar { get; }

    private readonly SearchValues<char> _SearchValues;

    public IBGTokenizerResultCreate<T> SelectResult { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (this._SearchValues.Contains(c)) {
                var match = value.Substring(0, 1);
                token = new BGToken<T>(match, this.SelectResult.Select(match));
                next = value.Substring(1);
                return true;
            }
        }
        token = default;
        next = value;
        return false;
    }
}
