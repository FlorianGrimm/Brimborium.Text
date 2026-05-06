namespace Brimborium.Gerede;

public record struct BGTupple<T1, T2>(
    T1 Value1,
    T2 Value2);

public sealed class BGParserSequenceResultTupple<T1, T2>
    : IBGParserSequenceResult<T1, T2, BGTupple<T1, T2>> {
    public BGTupple<T1, T2> Select(BGResult<T1> first, BGResult<T2> next, StringRange match) {
        return new BGTupple<T1, T2>(first.Value, next.Value);
    }
}

public record struct BGTupple<T1, T2, T3>(
    T1 Value1,
    T2 Value2,
    T3 Value);

public sealed class BGParserSequenceResultTupple<T1, T2, T3>
    : IBGParserSequenceResult<T1, T2, T3, BGTupple<T1, T2, T3>> {
    public BGTupple<T1, T2, T3> Select(BGResult<T1> value1, BGResult<T2> value2, BGResult<T3> value3, StringRange match) {
        return new BGTupple<T1, T2, T3>(value1.Value, value2.Value, value3.Value);
    }
}

public record struct BGTupple<T1, T2, T3,T4>(
    T1 Value1,
    T2 Value2,
    T3 Value3,
    T4 Value4);

public sealed class BGParserSequenceResultTupple<T1, T2, T3,T4>
    : IBGParserSequenceResult<T1, T2, T3, T4, BGTupple<T1, T2, T3,T4>> {
    public BGTupple<T1, T2, T3, T4> Select(BGResult<T1> value1, BGResult<T2> value2, BGResult<T3> value3, BGResult<T4> value4, StringRange match) {
        return new BGTupple<T1, T2, T3, T4>(value1.Value, value2.Value, value3.Value, value4.Value);
    }
}