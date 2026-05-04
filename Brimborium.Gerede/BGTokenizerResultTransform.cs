namespace Brimborium.Gerede;

public static class BGTokenizerResultTransform {
    public static IBGTokenizerResultTransform<T1, R> Delegate<T1, R>(
        Func<BGToken<T1>, StringRange, R> selector
    ) => new BGTokenizerResultTransformDelegate<T1, R>(selector);

    public static IBGTokenizerResultTransform<T1, T2, R> Delegate<T1, T2, R>(
        Func<BGToken<T1>, BGToken<T2>, StringRange, R> selector
    ) => new BGTokenizerResultTransformDelegate<T1, T2, R>(selector);

    public static IBGTokenizerResultTransform<T1, T2, T3, R> Delegate<T1, T2, T3, R>(
        Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, StringRange, R> selector
    ) => new BGTokenizerResultTransformDelegate<T1, T2, T3, R>(selector);
}

public sealed class BGTokenizerResultTransformDelegate<T1, R>(
        Func<BGToken<T1>, StringRange, R> selector
    ) : IBGTokenizerResultTransform<T1, R> {
    private readonly Func<BGToken<T1>, StringRange, R> _Selector = selector;

    public R Select(BGToken<T1> value1, StringRange match) {
        return this._Selector(value1, match);
    }
}
public sealed class BGTokenizerResultTransformDelegate<T1, T2, R>(
        Func<BGToken<T1>, BGToken<T2>, StringRange, R> selector
    ) : IBGTokenizerResultTransform<T1, T2, R> {
    private readonly Func<BGToken<T1>, BGToken<T2>, StringRange, R> _Selector = selector;

    public R Select(BGToken<T1> value1, BGToken<T2> value2, StringRange match) {
        return this._Selector(value1, value2, match);
    }
}

public sealed class BGTokenizerResultTransformDelegate<T1, T2, T3, R>(
        Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, StringRange, R> selector
    ) : IBGTokenizerResultTransform<T1, T2, T3, R> {
    private readonly Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, StringRange, R> _Selector = selector;

    public R Select(BGToken<T1> value1, BGToken<T2> value2, BGToken<T3> value3, StringRange match) {
        return this._Selector(value1, value2, value3, match);
    }
}

public sealed class BGTokenizerResultTransformDelegate<T1, T2, T3, T4, R>(
        Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, BGToken<T4>, StringRange, R> selector
    ) : IBGTokenizerResultTransform<T1, T2, T3, T4, R> {
    private readonly Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, BGToken<T4>, StringRange, R> _Selector = selector;

    public R Select(BGToken<T1> value1, BGToken<T2> value2, BGToken<T3> value3, BGToken<T4> value4, StringRange match) {
        return this._Selector(value1, value2, value3, value4,match);
    }
}
