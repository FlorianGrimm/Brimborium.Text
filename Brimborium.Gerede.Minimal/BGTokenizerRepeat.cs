namespace Brimborium.Gerede;

public class BGTokenizerRepeat : IBGTokenizer {
    public BGTokenizerRepeat(
        IBGTokenizer tokenizer,
        int minRepeat,
        int maxRepeat) {
        this.Tokenizer = tokenizer;
        this.MinRepeat = minRepeat;
        this.MaxRepeat = maxRepeat;
    }

    public IBGTokenizer Tokenizer { get; }
    public int MinRepeat { get; }
    public int MaxRepeat { get; }

    public bool TryGetToken(StringRange value, out StringRange next) {
        var current = value;
        var loop = 0;
        while (loop < this.MaxRepeat
            && this.Tokenizer.TryGetToken(current,  out var afterItem)) {
            current = afterItem;
            loop++;
        }
        if (this.MinRepeat <= loop) {
            next = current;
            return true;
        } else {
            next = value;
            return false;
        }
    }
}

public class BGTokenizerRepeat<T, R> : IBGTokenizer<R> {
    public BGTokenizerRepeat(
        IBGTokenizer<T> tokenizer,
        int minRepeat,
        int maxRepeat,
        IBGTokenizerResultRepeat<T, R> selectResult) {
        this.Tokenizer = tokenizer;
        this.MinRepeat = minRepeat;
        this.MaxRepeat = maxRepeat;
        this.SelectResult = selectResult;
    }

    public IBGTokenizer<T> Tokenizer { get; }
    public int MinRepeat { get; }
    public int MaxRepeat { get; }
    public IBGTokenizerResultRepeat<T, R> SelectResult { get; }

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<R> token, out StringRange next) {
        var items = new List<BGToken<T>>();
        var current = value;
        while (items.Count < this.MaxRepeat
            && this.Tokenizer.TryGetToken(current, out var item, out var afterItem)) {
            items.Add(item);
            current = afterItem;
        }
        if (this.MinRepeat <= items.Count) {
            var match = value.Substring(0, current.Start - value.Start);
            token = new BGToken<R>(match, this.SelectResult.Select(items, match));
            next = current;
            return true;
        }
        token = default;
        next = value;
        return false;
    }
}