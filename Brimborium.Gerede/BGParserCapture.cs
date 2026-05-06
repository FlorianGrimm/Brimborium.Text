namespace Brimborium.Gerede;

public class BGParserCapture<T, R> : IBGParser<R> {
    public BGParserCapture(
        IBGParser<T> innerParser,
        IBGTokenizerResultCaptureTransform<T, R> tokenizerResultCapture
        ) {
        this.InnerParser = innerParser;
        this.TokenizerResultCapture = tokenizerResultCapture;
    }

    public IBGParser<T> InnerParser { get; }
    public IBGTokenizerResultCaptureTransform<T, R> TokenizerResultCapture { get; }

    public bool Parse(BGParserInput input, [MaybeNullWhen(false)] out BGResult<R> match, [MaybeNullWhen(true)] out BGError error, out BGParserInput next) {
        if (this.InnerParser.Parse(input, out var innerMatch, out var innerError, out var innerNext)) {
            var tokenValue = this.TokenizerResultCapture.Select(innerMatch.Value, innerMatch.Match);
            match = new BGResult<R>(innerMatch.Match, tokenValue);
            error = default;
            next = innerNext;
            return true;
        } else {
            match = default;
            error = innerError;
            next = innerNext;
            return false;
        }
    }
}