
namespace Brimborium.Gerede; 

public class BGParserOr<T> : IBGParser<T> {
    public readonly IBGParser<T>[] ListParser;

    public BGParserOr(IEnumerable<IBGParser<T>> listParser) {
        this.ListParser = (listParser is IBGParser<T>[] list)
            ? list
            : listParser.ToArray();
    }

    public bool Parse(
        BGParserInput input, 
        [MaybeNullWhen(false)] out BGResult<T> match, 
        [MaybeNullWhen(true)] out BGError error, 
        out BGParserInput next) {
        foreach(var parser in this.ListParser){
            if (parser.Parse(input, out match, out error, out next)) {
                return true;
            }
        }
        {
            match = default;
            error = new BGError();
            next = input;
            return false;
        }
    }
}