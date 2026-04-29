namespace Brimborium.Gerede;

public class BGTokenizerAcceptString<T> : IBGTokenizer<T> {
    public readonly string AcceptText;
    public readonly T AcceptValue;

    public BGTokenizerAcceptString(
        string acceptText,
        T acceptValue
    ) {
        this.AcceptText = acceptText;
        this.AcceptValue = acceptValue;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next
        ) {
        if (!value.IsEmpty) {
            var result = value.StartsWith(this.AcceptText);
            if (result) {
                var length = this.AcceptText.Length;
                token = new(value.Substring(0, length), this.AcceptValue);
                next = value.Substring(length);
                return result;
            }
        }
        {
            token = default;
            next = value;
            return false;
        }
    }
}



