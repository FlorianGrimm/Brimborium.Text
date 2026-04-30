namespace Brimborium.Gerede;

public interface IBGCombiner<TResult, T1, T2, T3> {
    TResult Combine(T1 value1, T2 value2, T3 value3);
}
public class BGParserSequence<TResult, T1, T2, T3>
    : IBGParser<TResult> {

    public readonly IBGParser<T1> Parser1;
    public readonly IBGParser<T2> Parser2;
    public readonly IBGParser<T3> Parser3;
    public readonly IBGCombiner<TResult, T1, T2, T3> Combiner;

    public BGParserSequence(
        IBGParser<T1> parser1,
        IBGParser<T2> parser2,
        IBGParser<T3> parser3,
        IBGCombiner<TResult, T1, T2, T3> combiner
    ) {
        this.Parser1 = parser1;
        this.Parser2 = parser2;
        this.Parser3 = parser3;
        this.Combiner = combiner;
    }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<TResult> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {

        if (!this.Parser1.Parse(input, out var match1, out var error1, out var next1)) {
            match = default;
            error = error1;
            next = input;
            return false;
        }

        if (!this.Parser2.Parse(next1, out var match2, out var error2, out var next2)) {
            match = default;
            error = error2;
            next = input;
            return false;
        }

        if (!this.Parser3.Parse(next2, out var match3, out var error3, out var next3)) {
            match = default;
            error = error3;
            next = input;
            return false;
        }

        var matchMatch = input.Input[0..next3.Input.End];
        var matchValue = this.Combiner.Combine(match1.Value, match2.Value, match3.Value);
        match = new BGResult<TResult>(matchMatch, matchValue);
        error = default;
        next = next3;

        return true;
    }
}

public class BGCombiner<TResult, T1, T2, T3>
    : IBGCombiner<TResult, T1, T2, T3> {
    private readonly Func<T1, T2, T3, TResult> _Combiner;

    public BGCombiner(
        Func<T1, T2, T3, TResult> combiner
        ) {
        this._Combiner = combiner;
    }
    public TResult Combine(T1 value1, T2 value2, T3 value3) {
        return _Combiner(value1, value2, value3);
    }
}

