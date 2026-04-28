namespace Brimborium.Gerede;

public class BGParserTokenizer<T> : IBGParser<T> {
    public BGParserTokenizer(IBGTokenizer<T> tokenizer) {
        this.Tokenizer = tokenizer;
    }

    public IBGTokenizer<T> Tokenizer { get; }

    public bool Parse(BGParserInput input, [MaybeNullWhen(false)] out BGResult<T> match, [MaybeNullWhen(true)] out BGError error, out BGParserInput next) {
        if (this.Tokenizer.TryGetToken(input.Input, out var token, out var nextRange)) {
            match = new BGResult<T>(token.Match, token.Value);
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