namespace Brimborium.Gerede;

public sealed class BGParserRepeat<T, R> : IBGParser<R> {
    public BGParserRepeat(
        IBGParser<T> parser,
        int minRepeat,
        int maxRepeat,
        IBGParserResultRepeat<T, R> selectResult) {
        this.Parser = parser;
        this.MinRepeat = minRepeat;
        this.MaxRepeat = maxRepeat;
        this.SelectResult = selectResult;
    }

    public IBGParser<T> Parser { get; }
    public int MinRepeat { get; }
    public int MaxRepeat { get; }
    public IBGParserResultRepeat<T, R> SelectResult { get; }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<R> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        var items = new List<BGResult<T>>();
        var current = input;
        while (items.Count < this.MaxRepeat
            && this.Parser.Parse(current, out var item, out _, out var afterItem)) {
            items.Add(item);
            current = afterItem;
        }
        if (this.MinRepeat <= items.Count) {
            var span = input.Input.Substring(0, current.Input.Start - input.Input.Start);
            match = new BGResult<R>(span, this.SelectResult.Select(items, span));
            error = default;
            next = current;
            return true;
        }
        match = default;
        error = new BGError(input.Input, "minimum repeat not satisfied");
        next = input;
        return false;
    }
}
