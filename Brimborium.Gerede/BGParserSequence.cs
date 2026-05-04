namespace Brimborium.Gerede;

public interface IBGParserSequenceResult<T1, T2, R> {
    R Select(BGResult<T1> value1, BGResult<T2> value2, StringRange match);
}

public sealed class BGParserSequence<T1, T2, R> : IBGParser<R> {
    public BGParserSequence(
        IBGParser<T1> parser1,
        IBGParser<T2> parser2,
        IBGParserSequenceResult<T1, T2, R> selectResult) {
        this.Parser1 = parser1;
        this.Parser2 = parser2;
        this.SelectResult = selectResult;
    }

    public IBGParser<T1> Parser1 { get; }
    public IBGParser<T2> Parser2 { get; }
    public IBGParserSequenceResult<T1, T2, R> SelectResult { get; }
    //
    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<R> match,
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
        {
            var span = input.Input.Substring(0, next2.Input.Start - input.Input.Start);
            match = new BGResult<R>(span, this.SelectResult.Select(match1, match2, span));
            error = default;
            next = next2;
            return true;
        }
    }
}


public interface IBGParserSequenceResult<T1, T2, T3, R> {
    R Select(BGResult<T1> value1, BGResult<T2> value2, BGResult<T3> value3, StringRange match);
}

public sealed class BGParserSequence<T1, T2, T3, R> : IBGParser<R> {
    public BGParserSequence(
        IBGParser<T1> parser1,
        IBGParser<T2> parser2,
        IBGParser<T3> parser3,
        IBGParserSequenceResult<T1, T2, T3, R> selectResult) {
        this.Parser1 = parser1;
        this.Parser2 = parser2;
        this.Parser3 = parser3;
        this.SelectResult = selectResult;
    }

    public IBGParser<T1> Parser1 { get; }
    public IBGParser<T2> Parser2 { get; }
    public IBGParser<T3> Parser3 { get; }
    public IBGParserSequenceResult<T1, T2, T3, R> SelectResult { get; }
    //
    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<R> match,
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
        {
            var matchRange = input.Input.Substring(0, next3.Input.Start - input.Input.Start);
            match = new BGResult<R>(matchRange, this.SelectResult.Select(match1, match2, match3, matchRange));
            error = default;
            next = next2;
            return true;
        }
    }
}

