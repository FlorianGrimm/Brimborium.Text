namespace Brimborium.Gerede;

public sealed class BGTokenizerExcept<T1, T2>(
    IBGTokenizer<T1> tokenizerNext,
    IBGTokenizer<T2> tokenizerSkip
    ) : IBGTokenizer<BGVoid> {
    public IBGTokenizer<T1> Tokenizer { get; } = tokenizerNext;
    public IBGTokenizer<T2> TokenizerSkip { get; } = tokenizerSkip;

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<BGVoid> token, out StringRange next) {
        var current = value;
        while (value.IsEmpty) {
            if (this.Tokenizer.TryGetToken(current, out _, out _)) {
                break;
            } else {
                if (this.TokenizerSkip.TryGetToken(current, out _, out var nextSkip)) {
                    if (current.Start < nextSkip.Start) {
                        current = nextSkip;
                        continue;
                    }
                }
                current = current.Substring(1);
            }
        }

        {
            token = new BGToken<BGVoid>(value.Substring(0, current.Start - value.Start), new BGVoid());
            next = current;
            return (value.Start < next.Start);
        }
    }
}
