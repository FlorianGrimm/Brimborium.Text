namespace Brimborium.Gerede;

public static class BGCombine {
    public static IBGCombiner<TResult, T1, T2> Delegate<TResult, T1, T2>(
        Func<T1, T2, TResult> combiner
    ) => new BGCombiner<TResult, T1, T2>(
        combiner
    );

    public static IBGCombiner<TResult, T1, T2, T3> Delegate<TResult, T1, T2, T3>(
        Func<T1, T2, T3, TResult> combiner
    ) => new BGCombiner<TResult, T1, T2, T3>(
        combiner
    );

    public static IBGCombiner<TResult, T1, T2, T3, T4> Delegate<TResult, T1, T2, T3, T4>(
        Func<T1, T2, T3, T4, TResult> combiner
    ) => new BGCombiner<TResult, T1, T2, T3, T4>(
        combiner
    );

    public static IBGCombiner<TResult, T1, T2, T3, T4, T5> Delegate<TResult, T1, T2, T3, T4, T5>(
        Func<T1, T2, T3, T4, T5, TResult> combiner
    ) => new BGCombiner<TResult, T1, T2, T3, T4, T5>(
        combiner
    );

    public static IBGCombiner<TResult, T1, T2, T3, T4, T5, T6> Delegate<TResult, T1, T2, T3, T4, T5, T6>(
        Func<T1, T2, T3, T4, T5, T6, TResult> combiner
    ) => new BGCombiner<TResult, T1, T2, T3, T4, T5, T6>(
        combiner
    );
}