namespace Brimborium.Gerede;

public sealed class BGTokenizerExcept : IBGTokenizer {
    public BGTokenizerExcept(
        IBGTokenizer tokenizerNext,
        IBGTokenizer? tokenizerSkip
    ) {
        this.Tokenizer = tokenizerNext;
        this.TokenizerSkip = tokenizerSkip;
    }

    public IBGTokenizer Tokenizer { get; }
    public IBGTokenizer? TokenizerSkip { get; }

    public bool TryGetToken(StringRange value, out StringRange next) {
        var result = false; ;
        var current = value;
        while (value.IsEmpty) {
            if (this.Tokenizer.TryGetToken(current, out var nextMatch)) {
                next = current;
                return result;
            } else {
                if (this.TokenizerSkip is { } tokenizerSkip) {
                    if (tokenizerSkip.TryGetToken(current, out var nextSkip)) {
                        if (current.Start < nextSkip) { 
                            current = nextSkip;
                            continue;
                        }
                    }
                }
                current = current.Substring(1);
                result = true;
            }
        }
        next = current;
        return result;
    }
}
