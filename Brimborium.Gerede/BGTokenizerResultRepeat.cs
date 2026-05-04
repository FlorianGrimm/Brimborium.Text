namespace Brimborium.Gerede;

public static class BGTokenizerResultRepeat {
    public static IBGTokenizerResultRepeat<T, BGVoid> ConstBGVoid<T>(
        ) {
        return new BGTokenizerResultRepeatConst<T, BGVoid>(new BGVoid());
    }

    public static IBGTokenizerResultRepeat<T, R> Const<T, R>(
        R value
        ) {
        return new BGTokenizerResultRepeatConst<T, R>(value);
    }

    public static IBGTokenizerResultRepeat<T, string> MatchAsString<T>() {
        return new BGTokenizerResultRepeatMatchAsString<T>();
    }
}

public sealed class BGTokenizerResultRepeatConst<T, R>(
        R value
    ) : IBGTokenizerResultRepeat<T, R> {
    private readonly R _Value = value;

    public R Select(IReadOnlyList<BGToken<T>> items, StringRange match) {
        return this._Value;
    }
}

public sealed class BGTokenizerResultRepeatMatchAsString<T> : IBGTokenizerResultRepeat<T, string> {
    public string Select(IReadOnlyList<BGToken<T>> items, StringRange match) {
        return match.ToString();
    }
}