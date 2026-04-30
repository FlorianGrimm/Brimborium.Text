namespace Brimborium.Gerede;

public sealed class BGParserOr<T> : IBGParser<T> {
    public BGParserOr(IEnumerable<IBGParser<T>> listParser) {
        this.ListParser = (listParser is IBGParser<T>[] array)
            ? array
            : listParser.ToArray();
    }

    public IBGParser<T>[] ListParser { get; }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        foreach (var parser in this.ListParser) {
            if (parser.Parse(input, out match, out error, out next)) {
                return true;
            }
        }
        match = default;
        error = new BGError(input.Input, "no alternative matched");
        next = input;
        return false;
    }
}
