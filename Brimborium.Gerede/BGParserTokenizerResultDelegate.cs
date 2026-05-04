namespace Brimborium.Gerede;

public sealed class BGParserTokenizerResultDelegate<T>(
        Func<StringRange, T> transform
    ) : IBGParserTokenizerResult<T> {
    private readonly Func<StringRange, T> _Transform = transform;

    public T Select(StringRange match) {
        return this._Transform(match);
    }
}