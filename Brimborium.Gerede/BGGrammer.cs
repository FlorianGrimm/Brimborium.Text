namespace Brimborium.Gerede;

public sealed class BGInput {
    public readonly string Filename;
    public readonly StringRange Value;

    public BGInput(
        string filename,
        string value
        ) {
        this.Value = new StringRange(value);
        this.Filename = filename;
    }
}

public record struct BGError(
    StringRange Match,
    string Message
);

public sealed record class BGErrorEnhanced(
    StringRange Match,
    string Filename,
    int Row,
    int Column,
    string Message
    );

public static class BSErrorEnhancedExtension {
    public static BGErrorEnhanced Enhance(
        BGError error,
        BGInput input
        ) {
        string message = error.Message;
        var (row, column) = error.Match.GetRowColumn();
        return new BGErrorEnhanced(
            error.Match,
            input.Filename,
            row,
            column,
            message
            );
    }
}


public sealed class BGGrammer<T> : IBGParser<T> {
    public readonly IBGParser<T> Parser;

    public BGGrammer(
        IBGParser<T> parser
        ) {
        this.Parser = parser;
    }

    public bool Parse(
        BGInput input,
        [MaybeNullWhen(false)] out T match,
        [MaybeNullWhen(true)] out BGErrorEnhanced error) {
        if (this.Parser.Parse(
            new(input.Value, new BGTokenList()),
            out var resultMatch,
            out var resultError,
            out var next)) {
            // TODO: handle next
            match = resultMatch.Value;
            error = default;
            return true;
        } else {
            error = BSErrorEnhancedExtension.Enhance(resultError, input);
            match = default;
            return false;
        }
    }

    public bool Parse(
        BGParserInput input,
        [MaybeNullWhen(false)] out BGResult<T> match,
        [MaybeNullWhen(true)] out BGError error,
        out BGParserInput next) {
        return this.Parser.Parse(input, out match, out error, out next);
    }
}