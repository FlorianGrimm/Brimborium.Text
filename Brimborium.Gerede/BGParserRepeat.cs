namespace Brimborium.Gerede;

public class BGParserRepeat<TResult, TInner>
    : IBGParser<TResult> {
    public readonly IBGParser<TInner> ParserInner;
    public readonly IBGFactory<TResult> Factory;
    public readonly IBGResultAggregation<TResult, TInner> Aggregation;
    public readonly int MinElements;
    public readonly int MaxElements;

    public BGParserRepeat(
        IBGParser<TInner> parserInner,
        int minElements,
        int maxElements,
        IBGFactoryAggregation<TResult, TInner> factoryAggregation
    ) : this(
        parserInner,
        minElements,
        maxElements,
        factoryAggregation, factoryAggregation
    ) {
    }

    public BGParserRepeat(
        IBGParser<TInner> parserInner,
        int minElements,
        int maxElements,
        IBGFactory<TResult> factory,
        IBGResultAggregation<TResult, TInner> aggregation
    ) {
        this.ParserInner = parserInner;
        this.MinElements = minElements;
        this.MaxElements = maxElements;
        this.Factory = factory;
        this.Aggregation = aggregation;
    }


    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<TResult> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next
    ) {
        TResult result = this.Factory.Create();
        int count = 0;
        
        BGParserInput current = input;
        while (!current.Input.IsEmpty) {
            if (this.ParserInner.Parse(current, out var resultMatch, out error, out next)) {
                count++;
                result = this.Aggregation.Aggregate(resultMatch.Value, result);
                current = next;
                if (this.MaxElements <= count) {
                    break;
                }
            } else {
                break;
            }
        }
        if (this.MinElements <= count) {
            match = new(input.Input, result);
            next = current;
            error = default;
            return true;
        } else {
            match = default;
            error = new(current.Input, "");
            next = input;
            return false;
        }
    }
}