namespace Brimborium.Gerede;

public static partial class BGTokenizer {
    public static IBGTokenizer<BGVoid> AcceptEOF(
    ) => new BGTokenizerAcceptEOF<BGVoid>(new BGVoid());

    public static IBGTokenizer<T> AcceptEOF<T>(
        T acceptValue
    ) => new BGTokenizerAcceptEOF<T>(acceptValue);

    public static IBGTokenizer<BGVoid> ExceptEOF(
    ) => new BGTokenizerExceptEOF<BGVoid>(
        new BGTokenizerResultConst<BGVoid>(new BGVoid())
        );

    public static IBGTokenizer<T> ExceptEOF<T>(
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerExceptEOF<T>(selectResult);


    public static IBGTokenizer<BGVoid> AcceptChar(
        IEnumerable<char> acceptChar
    ) => new BGTokenizerAcceptChar<BGVoid>(
        acceptChar,
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<BGVoid> AcceptChar(
        IBGCharSet acceptCharSet
    ) => new BGTokenizerAcceptChar<BGVoid>(
        acceptCharSet.Build(),
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<T> AcceptChar<T>(
        IEnumerable<char> acceptChar,
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerAcceptChar<T>(acceptChar, selectResult);

    public static IBGTokenizer<T> AcceptChar<T>(
        IBGCharSet acceptCharSet,
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerAcceptChar<T>(acceptCharSet.Build(), selectResult);

    public static IBGTokenizer<BGVoid> ExceptChar(
        IEnumerable<char> exceptChar
    ) => new BGTokenizerExceptChar<BGVoid>(
        exceptChar,
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<BGVoid> ExceptChar(
        IBGCharSet exceptChar
    ) => new BGTokenizerExceptChar<BGVoid>(
        exceptChar.Build(),
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<T> ExceptChar<T>(
        IEnumerable<char> exceptChar,
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerExceptChar<T>(exceptChar, selectResult);

    public static IBGTokenizer<T> ExceptChar<T>(
        IBGCharSet exceptChar,
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerExceptChar<T>(exceptChar.Build(), selectResult);

    public static IBGTokenizer<BGVoid> AcceptString(
        string acceptText,
        StringComparison comparisonType = StringComparison.Ordinal
    ) => new BGTokenizerAcceptString<BGVoid>(
        acceptText,
        comparisonType,
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<BGVoid> ExceptString(
        string exceptText,
        StringComparison comparisonType = StringComparison.Ordinal
    ) => new BGTokenizerExceptString<BGVoid>(
        exceptText,
        comparisonType,
        new BGTokenizerResultConst<BGVoid>(new BGVoid()));

    public static IBGTokenizer<T> AcceptString<T>(
        string acceptText,
        StringComparison comparisonType,
        IBGTokenizerResultCreate<T> selectResult
    ) => new BGTokenizerAcceptString<T>(acceptText, comparisonType, selectResult);

    public static IBGTokenizer<T> Or<T>(
        params IBGTokenizer<T>[] listTokenizer
    ) => new BGTokenizerOr<T>(listTokenizer);

    public static IBGTokenizer<T> Or<T>(
        IEnumerable<IBGTokenizer<T>> listTokenizer
    ) => new BGTokenizerOr<T>(listTokenizer);

    extension<T>(IBGTokenizer<T> tokenizer) {
        public IBGTokenizer<R> TNext<T2, R>(
            IBGTokenizer<T2> nextTokenizer,
            IBGTokenizerResultTransform<T, T2, R> selectResult
        ) => new BGTokenizerNext<T, T2, R>(tokenizer, nextTokenizer, selectResult);

        public IBGTokenizer<R> TNext<T2, R>(
            IBGTokenizer<T2> nextTokenizer,
            Func<BGToken<T>, BGToken<T2>, StringRange, R> selector
        ) => new BGTokenizerNext<T, T2, R>(tokenizer, nextTokenizer, new BGTokenizerResultTransformDelegate<T, T2, R>(selector));

        public IBGTokenizer<BGVoid> TRepeat(
            int minRepeat,
            int maxRepeat
        ) => new BGTokenizerRepeat<T, BGVoid>(tokenizer, minRepeat, maxRepeat, BGTokenizerResultRepeat.ConstBGVoid<T>());

        public IBGTokenizer<R> TRepeat<R>(
            int minRepeat,
            int maxRepeat,
            IBGTokenizerResultRepeat<T, R> selectResult
        ) => new BGTokenizerRepeat<T, R>(tokenizer, minRepeat, maxRepeat, selectResult);

        public IBGTokenizer<R> TCapture<R>(
           IBGTokenizerResultTransform<T, R> selectResult
        ) => new BGTokenizerCapture<T, R>(tokenizer, selectResult);
    }
}

public interface IBGTokenizerResultCreate<R> {
    R Select(StringRange match);
}

public interface IBGTokenizerResultTransform<T1, R> {
    R Select(BGToken<T1> value1, StringRange match);
}

public interface IBGTokenizerResultTransform<T1, T2, R> {
    R Select(BGToken<T1> value1, BGToken<T2> value2, StringRange match);
}

public interface IBGTokenizerResultTransform<T1, T2, T3, R> {
    R Select(BGToken<T1> value1, BGToken<T2> value2, BGToken<T3> value3, StringRange match);
}

public interface IBGTokenizerResultTransform<T1, T2, T3, T4, R> {
    R Select(BGToken<T1> value1, BGToken<T2> value2, BGToken<T3> value3, BGToken<T4> value4, StringRange match);
}

public interface IBGTokenizerResultCaptureTransform<T, R> {
    R Select(T innerValue, StringRange match);
}


public interface IBGTokenizerResultRepeat<T, R> {
    R Select(IReadOnlyList<BGToken<T>> items, StringRange match);
}

public readonly struct BGToken<T>(
        StringRange match,
        T Value
    ) {
    public readonly StringRange Match = match;
    public readonly T Value = Value;
}

public interface IBGTokenizer<T> {
    /// <summary>
    /// Try to read and categorize the token
    /// </summary>
    /// <param name="value">the input</param>
    /// <param name="token">the found token - if returns true.</param>
    /// <param name="next">the next input use if matches or not</param>
    /// <returns>true if matched</returns>
    bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<T> token,
        out StringRange next);
}
