namespace Brimborium.Gerede;

public class BGTokenizerExceptString<T>(
        string exceptText,
        StringComparison comparisonType,
        IBGTokenizerResultCreate<T> selectResult
    ) : IBGTokenizer<T> {
    public string ExceptText { get; } = exceptText;
    public StringComparison ComparisonType { get; } = comparisonType;
    public IBGTokenizerResultCreate<T> SelectResult { get; } = selectResult;

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<T> token, out StringRange next) {
        var position = value.Text.IndexOf(this.ExceptText, value.Start, this.ComparisonType);
        var length = position - value.Start;
        if (position <= 0 || length <= 0) {
            token = default;
            next = value;
            return false;
        } else {
            var tokenMatch = value.Substring(length);
            var tokenValue = this.SelectResult.Select(tokenMatch);
            token = new BGToken<T>(tokenMatch, tokenValue);
            next = value.Substring(length);
            return true;
        }
    }

}