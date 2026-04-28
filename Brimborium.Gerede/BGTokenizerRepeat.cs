namespace Brimborium.Gerede;

public class BGTokenizerRepeat<TResult, TInner>
    : IBGTokenizer<TResult> {
    public readonly IBGTokenizer<TInner> Tokenizer;
    public readonly int MinElements;
    public readonly int MaxElements;
    public readonly IBGFactory<TResult> Factory;
    public readonly IBGResultAggregation<TResult, TInner> Aggregation;

    public BGTokenizerRepeat(
        IBGTokenizer<TInner> tokenizer,
        int minElements,
        int maxElements,
        IBGFactoryAggregation<TResult, TInner> factoryAggregation
    ) {
        this.Tokenizer = tokenizer;
        this.MinElements = minElements;
        this.MaxElements = maxElements;
        this.Factory = factoryAggregation;
        this.Aggregation = factoryAggregation;
    }
    public BGTokenizerRepeat(
        IBGTokenizer<TInner> tokenizer,
        int minElements,
        int maxElements,
        IBGFactory<TResult> factory,
        IBGResultAggregation<TResult, TInner> aggregation
    ) {
        this.Tokenizer = tokenizer;
        this.MinElements = minElements;
        this.MaxElements = maxElements;
        this.Factory = factory;
        this.Aggregation = aggregation;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<TResult> token,
        out StringRange next
    ) {
        StringRange current = value;
        TResult result = this.Factory.Create();
        int loop = 0;
        while (loop < this.MaxElements) {
            if (this.Tokenizer.TryGetToken(current, out var subToken, out var subNext)) {
                loop++;
                result = this.Aggregation.Aggregate(subToken.Value, result);
                current = subNext;
            } else {
                break;
            }
        }
        if (this.MinElements <= loop) {
            token = new(value.SubString(0, current.Start - value.Start), result);
            next = current;
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}
