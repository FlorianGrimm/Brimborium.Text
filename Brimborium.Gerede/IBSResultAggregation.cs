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
