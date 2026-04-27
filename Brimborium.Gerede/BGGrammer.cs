using Brimborium.Text;

using System.Diagnostics.CodeAnalysis;

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
        int column = 0;
        int row = 0;
        return new BGErrorEnhanced(
            error.Match,
            input.Filename,
            row,
            column,
            message
            );
    }
}


public class BGGrammer<T> {
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
            new (input.Value, new BGTokenList()), 
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
}