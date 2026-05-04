namespace Brimborium.Gerede;

public static partial class BGParserSequenceResult {
    public static IBGParserSequenceResult<T1, T2, R> Delegate<T1, T2, R>(
        Func<BGResult<T1>, BGResult<T2>, StringRange, R> selector
        ) => new BGParserSequenceDelegate<T1, T2, R>(selector);

    public static IBGParserSequenceResult<T1, T2, BGTupple<T1, T2>> Tupple<T1, T2>(
        ) => new BGParserSequenceResultTupple<T1, T2>();

    public static IBGParserSequenceResult<T1, T2, BGVoid> ReturnVoid<T1, T2>(
        ) => new BGParserSequenceDelegate<T1, T2, BGVoid>(
            static (_, _, _) => new BGVoid());

    public static IBGParserSequenceResult<T1, T2, T1> Returnvalue1<T1, T2>(
        ) => new BGParserSequenceDelegate<T1, T2, T1>(
            static (value1, _, _) => value1.Value);

    public static IBGParserSequenceResult<T1, T2, T2> Returnvalue2<T1, T2>(
        ) => new BGParserSequenceDelegate<T1, T2, T2>(
            (_, value2, _) => value2.Value);
}


public sealed class BGParserSequenceDelegate<T1, T2, R>(
        Func<BGResult<T1>, BGResult<T2>, StringRange, R> selector
    ) : IBGParserSequenceResult<T1, T2, R> {
    private readonly Func<BGResult<T1>, BGResult<T2>, StringRange, R> _Selector = selector;

    public R Select(BGResult<T1> value1, BGResult<T2> value2, StringRange match) {
        return this._Selector(value1, value2, match);
    }
}


public static partial class BGParserSequenceResult {
    public static IBGParserSequenceResult<T1, T2, T3, R> Delegate<T1, T2, T3, R>(
        Func<BGResult<T1>, BGResult<T2>, BGResult<T3>, StringRange, R> selector
        ) => new BGParserSequenceDelegate<T1, T2, T3, R>(selector);

    public static IBGParserSequenceResult<T1, T2, T3, BGTupple<T1, T2, T3>> Tupple<T1, T2, T3>(
        ) => new BGParserSequenceResultTupple<T1, T2, T3>();

    public static IBGParserSequenceResult<T1, T2, T3, BGVoid> ReturnVoid<T1, T2, T3>(
        ) => new BGParserSequenceDelegate<T1, T2, T3, BGVoid>(
            static (_, _, _, _) => new BGVoid());

    public static IBGParserSequenceResult<T1, T2, T3, T1> Returnvalue1<T1, T2, T3>(
        ) => new BGParserSequenceDelegate<T1, T2, T3, T1>(
            static (value1, _, _, _) => value1.Value);

    public static IBGParserSequenceResult<T1, T2, T3, T2> Returnvalue2<T1, T2, T3>(
        ) => new BGParserSequenceDelegate<T1, T2, T3, T2>(
            static (_, value2, _, _) => value2.Value);
    public static IBGParserSequenceResult<T1, T2, T3, T3> ReturnValue3<T1, T2, T3>(
        ) => new BGParserSequenceDelegate<T1, T2, T3, T3>(
            static (_, _, value3, _) => value3.Value);
}

public sealed class BGParserResultNextReturnsVoid<T1, T2, T3>
    : IBGParserSequenceResult<T1, T2, T3, BGVoid> {
    public BGVoid Select(BGResult<T1> value1, BGResult<T2> value2, BGResult<T3> value3, StringRange match) {
        return new BGVoid();
    }
}

public sealed class BGParserSequenceDelegate<T1, T2, T3, R>(
    Func<BGResult<T1>, BGResult<T2>, BGResult<T3>, StringRange, R> selector
        ) : IBGParserSequenceResult<T1, T2, T3, R> {
    private readonly Func<BGResult<T1>, BGResult<T2>, BGResult<T3>, StringRange, R> _Selector = selector;

    public R Select(BGResult<T1> value1, BGResult<T2> value2, BGResult<T3> value3, StringRange match) {
        return this._Selector(value1, value2, value3, match);
    }
}

