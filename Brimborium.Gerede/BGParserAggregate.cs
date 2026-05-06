using System.ComponentModel.DataAnnotations;

namespace Brimborium.Gerede;

public sealed class BGParserAggregate<T, R> : IBGParser<R> {
    public BGParserAggregate(IBGParser<T> parser, IBGParserResultAggregate<T, R> selectResult) {
        this.Parser = parser;
        this.SelectResult = selectResult;
    }

    public IBGParser<T> Parser { get; }

    public IBGParserResultAggregate<T, R> SelectResult { get; }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<R> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        var current = input;
        var result = false;
        var resultValue = this.SelectResult.Create();
        while (!input.Input.IsEmpty) {
            if (this.Parser.Parse(current, out var currentMatch, out var currentError, out var currentNext)) {
                result = true;
                current = current.With(currentNext.Input);
                resultValue = this.SelectResult.Aggregate(resultValue, currentMatch.Value, currentMatch.Match);
            } else {
                break;
            }
        }
        if (result) {
            next = current;
            var tokenMatch = input.Input.Substring(0, current.Input.Start - input.Input.Start);
            match = new BGResult<R>(tokenMatch, resultValue);
            error = default;
            return true;
        } else {
            next = input;
            match = default;
            error = new BGError(input.Input, "");
            return false;
        }
    }
}