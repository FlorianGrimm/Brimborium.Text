namespace Brimborium.Gerede;

public sealed class BGParserRefer<T>
    : IBGParser<T> {
    private readonly Func<IBGParser<T>> _GetParser;
    private IBGParser<T>? _GetParserCache;

    public BGParserRefer(
        Func<IBGParser<T>> getParser
        ) {
        this._GetParser = getParser;
    }


    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error, out BGParserInput next) {
        var parser = (this._GetParserCache??= this._GetParser());
        return parser.Parse(input, out match, out error, out next);
    }
}
