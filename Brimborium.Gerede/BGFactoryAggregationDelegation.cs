namespace Brimborium.Gerede;

public class BGFactoryAggregationDelegation<TResult, TInner>
    : IBGFactoryAggregation<TResult, TInner> {
    private readonly Func<TResult> _Create;
    private readonly Func<TInner, TResult, TResult> _Aggregate;

    public BGFactoryAggregationDelegation(
        Func<TResult> create,
        Func<TInner, TResult, TResult> aggregate
        ) {
        this._Create = create;
        this._Aggregate = aggregate;
    }

    public TResult Create() {
        return this._Create();
    }

    public TResult Aggregate(TInner inner, TResult result) {
        return this._Aggregate(inner, result);
    }
}
