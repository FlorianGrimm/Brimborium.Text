namespace Brimborium.Gerede;

public class BGParserToken<T>
    : IBGParser<T> {
    public readonly IBGTokenizer<T> Tokenizer;

    public BGParserToken(
        IBGTokenizer<T> tokenizer
        ) {
        this.Tokenizer = tokenizer;
    }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next
    ) {
        if (this.Tokenizer.TryGetToken(
            input.Input,
            out var token,
            out var nextToken)) {
            match = new(token.Match, token.Value);
            error = default;
            next = input.With(nextToken);
            return true;
        } else {
            match = default;
            error = new();
            next = input;
            return false;
        }
    }
}
