namespace Brimborium.Gerede;

public sealed class BGTokenizerRepeat<T, R>(
        IBGTokenizer<T> tokenizer,
        int minRepeat,
        int maxRepeat,
        IBGTokenizerResultRepeat<T, R> selectResult
    ) : IBGTokenizer<R> {
    public IBGTokenizer<T> Tokenizer { get; } = tokenizer;
    public int MinRepeat { get; } = minRepeat;
    public int MaxRepeat { get; } = maxRepeat;
    public IBGTokenizerResultRepeat<T, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<R> token, out StringRange next) {
        var items = new List<BGToken<T>>();
        var current = value;
        while (!value.IsEmpty
            && items.Count < this.MaxRepeat
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