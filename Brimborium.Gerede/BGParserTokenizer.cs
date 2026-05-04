namespace Brimborium.Gerede;

public sealed class BGParserTokenizerSelect<T, R>(
        IBGTokenizer<T> tokenizer,
        IBGTokenizerResultTransform<T, R> selectResult
    ) : IBGParser<R> {
    public IBGTokenizer<T> Tokenizer { get; } = tokenizer;
    public IBGTokenizerResultTransform<T, R> SelectResult { get; } = selectResult;

    public bool Parse(
        BGParserInput input, 
        [MaybeNullWhen(false)] out BGResult<R> match, 
        [MaybeNullWhen(true)] out BGError error, 
        out BGParserInput next) {
        if (this.Tokenizer.TryGetToken(input.Input, out var token, out var nextRange)) {
            var tokenMatch = input.Input.Substring(0, nextRange.Start - input.Input.Start);
            var tokenResult = this.SelectResult.Select(token, tokenMatch);
            match = new BGResult<R>(tokenMatch, tokenResult);
            error = default;
            next = input.With(nextRange);
            return true;
        }
        match = default;
        error = new BGError(input.Input, "no match");
        next = input;
        return false;
    } 
}

public sealed class BGParserTokenizer<T>(
        IBGTokenizer<T> tokenizer
    ) : IBGParser<T> {
    public IBGTokenizer<T> Tokenizer { get; } = tokenizer;

    public bool Parse(BGParserInput input, [MaybeNullWhen(false)] out BGResult<T> match, [MaybeNullWhen(true)] out BGError error, out BGParserInput next) {
        if (this.Tokenizer.TryGetToken(input.Input, out var token, out var nextRange)) {
            match = new BGResult<T>(token.Match, token.Value);
            error = default;
            next = input.With(nextRange);
            return true;
        } else {
            match = default;
            error = new BGError(input.Input, "no match");
            next = input;
            return false;
        }
    }
}
