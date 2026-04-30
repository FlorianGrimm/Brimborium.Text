namespace Brimborium.Gerede; 

public sealed class BGTokenizerAcceptString : IBGTokenizer {
    public BGTokenizerAcceptString(string acceptText, StringComparison comparisonType) {
        this.AcceptText = acceptText;
        this.ComparisonType = comparisonType;
    }

    public string AcceptText { get; }
    public StringComparison ComparisonType { get; }

    public bool TryGetToken(StringRange value, out StringRange next) {
        if (value.StartsWith(this.AcceptText, this.ComparisonType)) {
            next = value.Substring(this.AcceptText.Length); 
            return true;
        } else {
            next = value;
            return false;
        }
    }
}

public sealed class BGTokenizerAcceptString<T> : IBGTokenizer<T> {
    public BGTokenizerAcceptString(
        string acceptText, 
        StringComparison comparisonType,
        IBGTokenizerResultAccept<T> selectResult
        ) {
        this.AcceptText = acceptText;
        this.ComparisonType = comparisonType;
        this.SelectResult = selectResult;
    }

    public string AcceptText { get; }
    public StringComparison ComparisonType { get; }
    public IBGTokenizerResultAccept<T> SelectResult { get; }

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<T> token, out StringRange next) {
        if (value.StartsWith(this.AcceptText, this.ComparisonType)) {
            var length = this.AcceptText.Length;
            var tokenMatch = value.Substring(0, length);
            var tokenValue = this.SelectResult.Select(tokenMatch);
            token = new BGToken<T>(tokenMatch, tokenValue);
            next = value.Substring(length);
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}