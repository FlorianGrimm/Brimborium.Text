namespace Brimborium.Gerede;

public static class BGTokenizerResult {
    public static IBGTokenizerResultCreate<T> Const<T>(T value)
        => new BGTokenizerResultConst<T>(value);

    public static IBGTokenizerResultCreate<string> MatchAsString()
        => new BGTokenizerResultMatchAsString();
}

public sealed class BGTokenizerResultConst<T>(
        T value
    ) : IBGTokenizerResultCreate<T> {
    private readonly T _Value = value;

    public T Select(StringRange match) {
        return this._Value;
    }
}

public sealed class BGTokenizerResultMatchAsString : IBGTokenizerResultCreate<string> {
    public string Select(StringRange match) => match.ToString();
}

public sealed class BGTokenizerResultTransformMatchAsString<T> : IBGTokenizerResultTransform<T, string> {
    // public string Select(StringRange match) => match.ToString();

    public string Select(BGToken<T> value1, StringRange match) => match.ToString();
}
//
