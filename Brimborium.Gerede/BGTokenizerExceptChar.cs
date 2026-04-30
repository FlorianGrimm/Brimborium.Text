using System.Buffers;

namespace Brimborium.Gerede;

public sealed class BGTokenizerExceptChar : IBGTokenizer {
    public BGTokenizerExceptChar(
        IEnumerable<char> exceptChar
        ) {
        var listValue = (exceptChar is char[] array) ? array : exceptChar.ToArray();
        this.ExceptChar = listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
    }

    public char[] ExceptChar { get; }

    private readonly SearchValues<char> _SearchValues;

    public bool TryGetToken(
        StringRange value,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (!this._SearchValues.Contains(c)) {
                var match = value.Substring(0, 1);
                next = value.Substring(1);
                return true;
            }
        }
        next = value;
        return false;
    }
}

public sealed class BGTokenizerExceptChar<T> : IBGTokenizer<T> {
    public BGTokenizerExceptChar(
        IEnumerable<char> exceptChar,
        IBGTokenizerResultAccept<T> selectResult) {
        var listValue = (exceptChar is char[] array) ? array : exceptChar.ToArray();
        this.ExceptChar = listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
        this.SelectResult = selectResult;
    }

    public char[] ExceptChar { get; }

    private readonly SearchValues<char> _SearchValues;

    public IBGTokenizerResultAccept<T> SelectResult { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (!this._SearchValues.Contains(c)) {
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
