namespace Brimborium.Gerede;

public class BGTokenizerAcceptChar<T> : IBGTokenizer<T> {
    public readonly char AcceptChar;
    public readonly T AcceptValue;

    public BGTokenizerAcceptChar(
        char acceptChar,
        T acceptValue
    ) {
        this.AcceptChar = acceptChar;
        this.AcceptValue = acceptValue;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next
        ) {
        if (!value.IsEmpty) {
            if (value.TryGetFirst(out var c)) {
                var result = c == this.AcceptChar; 
                if (result) {
                    token = new(value.SubString(0, 1), this.AcceptValue);
                    next = value.SubString(1);
                    return result;
                }
            }
        }
        {
            token = default;
            next = value;
            return false;
        }
    }
}
