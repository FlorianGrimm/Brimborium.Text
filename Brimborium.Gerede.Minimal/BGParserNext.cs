namespace Brimborium.Gerede;

public class BGParserNext<T, N, R> : IBGParser<R> {
    public BGParserNext(
        IBGParser<T> parser,
        IBGParser<N> nextParser,
        IBGParserResultNext<T, N, R> selectResult) {
        this.Parser = parser;
        this.NextParser = nextParser;
        this.SelectResult = selectResult;
    }

    public IBGParser<T> Parser { get; }
    public IBGParser<N> NextParser { get; }
    public IBGParserResultNext<T, N, R> SelectResult { get; }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<R> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        if (this.Parser.Parse(input, out var firstMatch, out error, out var afterFirst)) {
            if (this.NextParser.Parse(afterFirst, out var nextMatch, out error, out var afterNext)) {
                var span = input.Input.Substring(0, afterNext.Input.Start - input.Input.Start);
                match = new BGResult<R>(span, this.SelectResult.Select(firstMatch, nextMatch, span));
                error = default;
                next = afterNext;
                return true;
            } else {
                match = default;
                error = new BGError(input.Input.Substring(afterFirst.Input.End - input.Input.Start), "");
                next = input;
                return false;
            }
        } else {

            match = default;

            next = input;
            return false;
        }
    }
}
