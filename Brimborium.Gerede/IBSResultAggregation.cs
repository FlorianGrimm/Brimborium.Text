namespace Brimborium.Gerede;

public interface IBGFactory<T> {
    T Create();
}
public interface IBGResultAggregation<TResult, TInner> {
    TResult Aggregate(TInner inner, TResult result);
}
public interface IBGFactoryAggregation<TResult, TInner>
    : IBGFactory<TResult>
    , IBGResultAggregation<TResult, TInner> {
}

public class BSFactoryNew<T> : IBGFactory<T> where T : new() {
    public T Create() {
        return new();
    }
}

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
