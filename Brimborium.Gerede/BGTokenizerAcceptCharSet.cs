namespace Brimborium.Gerede;

public class BGTokenizerAcceptCharSet<T> : IBGTokenizer<T> {
    public readonly char[] AcceptCharSet;
    public readonly System.Buffers.SearchValues<char> AcceptSearchValues;
    public readonly T AcceptValue;

    public BGTokenizerAcceptCharSet(
        char[] acceptCharSet,
        T acceptValue
    ) {
        this.AcceptCharSet = acceptCharSet;
        this.AcceptValue = acceptValue;
        this.AcceptSearchValues = System.Buffers.SearchValues.Create(acceptCharSet);
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next
        ) {
        if (!value.IsEmpty) {
            if (value.TryGetFirst(out var c)) {
                var result = this.AcceptCharSet.Contains(c);
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




