namespace Brimborium.Gerede;

public static class BGTokenizerResultCreate {
    public static IBGTokenizerResultCreate<T> Delegate<T>(
        Func<StringRange, T> selector
        ) => new BGTokenizerResultCreateDelegate<T>(selector);
}

public sealed class BGTokenizerResultCreateDelegate<T>(
        Func<StringRange, T> selector
    ) : IBGTokenizerResultCreate<T> {
    private readonly Func<StringRange, T> _Selector = selector;

    T IBGTokenizerResultCreate<T>.Select(StringRange match) {
        return this._Selector(match);
    }
}
