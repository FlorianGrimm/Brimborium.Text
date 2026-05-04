namespace Brimborium.Gerede;

public sealed class BGParserResultAggregateDelegate<T, R>(
    Func<R> create,
    Func<R, T, StringRange, R> aggregate
    ) : IBGParserResultAggregate<T, R> {

    private readonly Func<R> _Create = create;
    private readonly Func<R, T, StringRange, R> _Aggregate = aggregate;

    public R Create() {
        return this._Create();
    }

    public R Aggregate(R accumulator, T currentValue, StringRange match) {
        return this._Aggregate(accumulator, currentValue, match);
    }
}
