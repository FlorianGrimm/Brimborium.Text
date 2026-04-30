namespace Brimborium.Gerede;

public sealed class BGParserLazy<T>
    : IBGParser<T> {

    public BGParserLazy(
        Lazy<IBGParser<T>> lazyParser
        ) {
        this.LazyParser = lazyParser;
    }

    public Lazy<IBGParser<T>> LazyParser { get; }

    public bool Parse(
        BGParserInput input, 
        [MaybeNullWhen(false)] out BGResult<T> match, 
        [MaybeNullWhen(true)] out BGError error, out BGParserInput next) {
        var parser = this.LazyParser.Value;
        return parser.Parse(input, out match, out error, out next);
    }
}
