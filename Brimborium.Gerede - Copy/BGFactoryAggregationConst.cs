namespace Brimborium.Gerede;

public class BGFactoryAggregationConst<T>
    : IBGFactoryAggregation<T,T> {
    public readonly T Value;

    public BGFactoryAggregationConst(
        T result
        ) {
        this.Value = result;
    }

    public T Create() => this.Value;

    public T Aggregate(T inner, T result) => this.Value;
}
