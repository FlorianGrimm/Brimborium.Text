namespace Brimborium.Gerede;

public static class BGTokenizerResultCapture {
    public static IBGTokenizerResultCaptureTransform<T, R> Delegate<T, R>(
        Func<T, StringRange, R> selectResult
        ) {
        return new BGTokenizerResultCaptureDelegate<T, R>(selectResult);
    }

    public static IBGTokenizerResultCaptureTransform<T, string> MatchAsString<T>(
        ) {
        return new BGTokenizerResultCaptureMatchAsString<T>();
    }
}

public class BGTokenizerResultCaptureDelegate<T, R>(
    Func<T, StringRange, R> selectResult
    ) : IBGTokenizerResultCaptureTransform<T, R> {
    public Func<T, StringRange, R> SelectResult { get; } = selectResult;

    public R Select(T innerValue, StringRange match) {
        return this.SelectResult(innerValue, match);
    }
}

public sealed class BGTokenizerResultCaptureMatchAsString<T>
    : IBGTokenizerResultCaptureTransform<T, string> {

    public string Select(T innerValue, StringRange match) {
        return match.ToString();
    }
}