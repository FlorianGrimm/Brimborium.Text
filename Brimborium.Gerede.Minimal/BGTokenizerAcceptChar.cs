using System.Buffers;

namespace Brimborium.Gerede;

public class BGTokenizerAcceptChar : IBGTokenizer {
    public BGTokenizerAcceptChar(
        IEnumerable<char> value
        ) {
        var listValue = (value is char[] array) ? array : value.ToArray();
        this.Value =listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
    }

    public char[] Value { get; }

    private readonly SearchValues<char> _SearchValues;

    public bool TryGetToken(
        StringRange value,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (this._SearchValues.Contains(c)){
                var match = value.SubString(0, 1);
                next = value.SubString(1);
                return true;
            }
        }
        next = value;
        return false;
    }
}
public class BGTokenizerAcceptChar<T> : IBGTokenizer<T> {
    public BGTokenizerAcceptChar(
        IEnumerable<char> value,
        IBGTokenizerResultAccept<T> selectResult) {
        var listValue = (value is char[] array) ? array : value.ToArray();
        this.Value =listValue;
        this._SearchValues = System.Buffers.SearchValues.Create(listValue);
        this.SelectResult = selectResult;
    }

    public char[] Value { get; }

    private readonly  SearchValues<char> _SearchValues;

    public IBGTokenizerResultAccept<T> SelectResult { get; }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next) {
        if (value.TryGetFirst(out var c)) {
            if (this._SearchValues.Contains(c)){
                var match = value.SubString(0, 1);
                token = new BGToken<T>(match, this.SelectResult.Select(match));
                next = value.SubString(1);
                return true;
            }
        }
        token = default;
        next = value;
        return false;
    }
}