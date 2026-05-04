namespace Brimborium.Gerede;

public sealed class BGParserResultRepeatDelegate<T, R>(
        Func<IEnumerable<BGResult<T>>, StringRange, R> selector
    ) : IBGParserResultRepeat<T, R> {
    private readonly Func<IEnumerable<BGResult<T>>, StringRange, R> _Selector = selector;

    public R Select(IEnumerable<BGResult<T>> items, StringRange match) {
        return this._Selector(items, match);
    }
}