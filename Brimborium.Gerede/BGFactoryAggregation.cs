namespace Brimborium.Gerede;

public static class BGFactoryAggregation {
    private static BGFactoryAggregationConst<BGVoid>? _Void;

    public static BGFactoryAggregationConst<BGVoid> BGVoid()
        => (_Void ??= new BGFactoryAggregationConst<BGVoid>(new BGVoid()));

    public static BGFactoryAggregationConst<T> Const<T>(
        T value
    ) => new BGFactoryAggregationConst<T>(
        value
    );

    public static BGFactoryAggregationDelegation<TResult, TInner> Delegation<TResult, TInner>(
        Func<TResult> create,
        Func<TInner, TResult, TResult> aggregate
    ) => new BGFactoryAggregationDelegation<TResult, TInner>(
        create,
        aggregate
    );
}
