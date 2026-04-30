namespace Brimborium.Gerede;

public class BGTokenizerPredicate<T> : IBGTokenizer<T> {
    public readonly Func<char, bool> Predicate;
    public readonly T AcceptValue;

    public BGTokenizerPredicate(
        Func<char, bool> predicate,
        T acceptValue
    ) {
        this.Predicate = predicate;
        this.AcceptValue = acceptValue;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next
        ) {
        if (!value.IsEmpty) {
            if (value.TryGetFirst(out var c)) {
                var result = this.Predicate(c);
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




