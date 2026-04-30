using System.Buffers;

namespace Brimborium.Gerede;

public sealed class BGTokenizerAcceptChar : IBGTokenizer {
    public BGTokenizerAcceptChar(
        IEnumerable<char> acceptChar
        ) {
        var listValue = (acceptChar is char[] array) ? array : acceptChar.ToArray();
        this.AcceptChar = listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
    }

    public char[] AcceptChar { get; }

    private readonly SearchValues<char> _SearchValues;

    public bool TryGetToken(
        StringRange value,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (this._SearchValues.Contains(c)) {
                var match = value.Substring(0, 1);
                next = value.Substring(1);
                return true;
            }
        }
        next = value;
        return false;
    }
}

public sealed class BGTokenizerAcceptChar<T> : IBGTokenizer<T> {
    public BGTokenizerAcceptChar(
        IEnumerable<char> acceptChar,
        IBGTokenizerResultAccept<T> selectResult) {
        var listValue = (acceptChar is char[] array) ? array : acceptChar.ToArray();
        this.AcceptChar = listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
        this.SelectResult = selectResult;
    }

    public char[] AcceptChar { get; }

    private readonly SearchValues<char> _SearchValues;

    public IBGTokenizerResultAccept<T> SelectResult { get; }

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
