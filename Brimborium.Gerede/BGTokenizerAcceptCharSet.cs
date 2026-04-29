namespace Brimborium.Gerede;

public class BGTokenizerAcceptCharSet<T> : IBGTokenizer<T> {
    public readonly char[] AcceptCharSet;
    public readonly System.Buffers.SearchValues<char> AcceptSearchValues;
    public readonly T AcceptValue;

    public BGTokenizerAcceptCharSet(
        IEnumerable<char> acceptCharSet,
        T acceptValue
    ) {
        this.AcceptCharSet = (acceptCharSet is char[] list)?list:acceptCharSet.ToArray();
        this.AcceptValue = acceptValue;
        ReadOnlySpan<char> chars = this.AcceptCharSet.AsSpan();
        this.AcceptSearchValues = System.Buffers.SearchValues.Create(chars);
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
                    token = new(value.Substring(0, 1), this.AcceptValue);
                    next = value.Substring(1);
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
