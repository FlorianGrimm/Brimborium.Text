namespace Brimborium.Gerede;

public static class BGTokenizer {
    public static IBGTokenizer AcceptEOF(
    ) => new BGTokenizerAcceptEOF();

    public static IBGTokenizer<T> AcceptEOF<T>(
        T acceptValue
    ) => new BGTokenizerAcceptEOF<T>(acceptValue);

    public static IBGTokenizer AcceptChar(
        IEnumerable<char> value
    ) => new BGTokenizerAcceptChar(value);

    public static IBGTokenizer<T> AcceptChar<T>(
        IEnumerable<char> value,
        IBGTokenizerResultAccept<T> selectResult
    ) => new BGTokenizerAcceptChar<T>(value, selectResult);

    public static IBGTokenizer Or(
        IEnumerable<IBGTokenizer> listTokenizer
    ) => new BGTokenizerOr(listTokenizer);

    public static IBGTokenizer<T> Or<T>(
        IEnumerable<IBGTokenizer<T>> listTokenizer
    ) => new BGTokenizerOr<T>(listTokenizer);

    extension(IBGTokenizer tokenizer) {
        IBGTokenizer Next(
            IBGTokenizer nextTokenizer
        ) => new BGTokenizerNext(tokenizer, nextTokenizer);

        public IBGTokenizer Repeat(
            int minRepeat,
            int maxRepeat
        ) => new BGTokenizerRepeat(tokenizer, minRepeat, maxRepeat);

        public IBGTokenizer<T> Capture<T>(
            IBGTokenizerResultAccept<T> selectResult
        ) => new BGTokenizerCapture<T>(tokenizer, selectResult);
    }

    extension<T>(IBGTokenizer<T> tokenizer) {
        public IBGTokenizer<R> Next<N, R>(
            IBGTokenizer<N> nextTokenizer,
            IBGTokenizerResultNext<T, N, R> selectResult
        ) => new BGTokenizerNext<T, N, R>(tokenizer, nextTokenizer, selectResult);

        public IBGTokenizer<R> Repeat<R>(
            int minRepeat,
            int maxRepeat,
            IBGTokenizerResultRepeat<T, R> selectResult
        ) => new BGTokenizerRepeat<T, R>(tokenizer, minRepeat, maxRepeat, selectResult);

        public IBGTokenizer<R> Capture<R>(
           IBGTokenizerResultAccept<R> selectResult
        ) => new BGTokenizerCapture<R, T>(tokenizer, selectResult);
    }
}

public interface IBGTokenizerResultAccept<T> {
    T Select(StringRange match);
}

public interface IBGTokenizerResultNext<T, N, R> {
    R Select(BGToken<T> first, BGToken<N> next, StringRange match);
}

public interface IBGTokenizerResultRepeat<T, R> {
    R Select(IReadOnlyList<BGToken<T>> items, StringRange match);
}

public readonly struct BGToken<T> {
    public readonly StringRange Match;
    public readonly T Value;

    public BGToken(
        StringRange match,
        T Value
        ) {
        this.Match = match;
        this.Value = Value;
    }
}

public interface IBGTokenizer {
    /// <summary>
    /// Try to read and categorize the token
    /// </summary>
    /// <param name="value">the input</param>
    /// <param name="next">the next input use if matches or not</param>
    /// <returns>true if matched</returns>
    bool TryGetToken(
        StringRange value,
        out StringRange next);
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
