using System.ComponentModel.DataAnnotations;

namespace Brimborium.Gerede;

public class BGParserList<T1, S, R> : IBGParser<R> {
    public BGParserList(
        IBGParser<T1> parserContent,
        IBGParser<S> parserSplit,
        int minRepeat,
        int maxRepeat,
        IBGParserResultAggregate<T1, R> aggregate
        ) {
        this.ParserContent = parserContent;
        this.ParserSplit = parserSplit;
        this.MinRepeat = minRepeat;
        this.MaxRepeat = maxRepeat;
        this.Aggregate = aggregate;
    }

    public IBGParser<T1> ParserContent { get; }
    public IBGParser<S> ParserSplit { get; }
    public int MinRepeat { get; }
    public int MaxRepeat { get; }
    public IBGParserResultAggregate<T1, R> Aggregate { get; }

    public bool Parse(
        BGParserInput input, 
        [MaybeNullWhen(false)] out BGResult<R> match, 
        [MaybeNullWhen(true)] out BGError error, 
        out BGParserInput next) {
        int loop = 0;
        error = new BGError(input.Input, "List");
        var result = this.Aggregate.Create();
        var current = input;
        while (!input.Input.IsEmpty && (loop<this.MaxRepeat)) {
            if (!this.ParserContent.Parse(current, out var matchContent, out error, out var nextContent)){
                break;
            }
            loop++;
            result = this.Aggregate.Aggregate(result, matchContent.Value, matchContent.Match);
            if (!this.ParserSplit.Parse(nextContent, out var matchSplit, out var errorSplit, out var nextSplit)) {
                break;
            }
            current=nextSplit ;
        }
        if (!(this.MinRepeat <= loop && loop <= this.MaxRepeat)) {
            match = default;
            next = input;
            return false;
        } else {
            var tokenMatch = input.Input.Substring(0, current.Input.Start-input.Input.Start);
            match = new BGResult<R>(tokenMatch, result);
            error = default;
            next = current;
            return true;
        }
    }
}