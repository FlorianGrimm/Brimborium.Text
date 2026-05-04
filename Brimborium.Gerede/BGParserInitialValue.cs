namespace Brimborium.Gerede;

public class BGParserInitialValue<T>
    : IBGParser<T> {
    private readonly T _Value;
    public BGParserInitialValue(
        T value
        ) {
        this._Value = value;
    }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        var tokenMatch = input.Input.Substring(0, 0);
        var tokenValue = this._Value;
        match = new BGResult<T>(tokenMatch, tokenValue);
        error = default(BGError);
        next = input;
        return true;
    }
}