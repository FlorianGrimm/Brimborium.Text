using System.Text.RegularExpressions;

namespace Brimborium.Gerede;

public class BGTokenizerNext : IBGTokenizer {
    public BGTokenizerNext(
        IBGTokenizer tokenizer,
        IBGTokenizer nextTokenizer
    ) {
        this.Tokenizer = tokenizer;
        this.NextTokenizer = nextTokenizer;
    }

    public IBGTokenizer Tokenizer { get; }
    public IBGTokenizer NextTokenizer { get; }

    public bool TryGetToken(StringRange value, out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out var afterFirst)) {
            if (this.NextTokenizer.TryGetToken(afterFirst, out var afterNext)) {
                next = afterNext;
                return true;
            } else {
                next = value;
                return false;
            }
        } else {
            next = value;
            return false;
        }
    }
}

public class BGTokenizerNext<T, N, R> : IBGTokenizer<R> {
    public BGTokenizerNext(
        IBGTokenizer<T> tokenizer,
        IBGTokenizer<N> nextTokenizer,
        IBGTokenizerResultNext<T, N, R> selectResult) {
        this.Tokenizer = tokenizer;
        this.NextTokenizer = nextTokenizer;
        this.SelectResult = selectResult;
    }

    public IBGTokenizer<T> Tokenizer { get; }
    public IBGTokenizer<N> NextTokenizer { get; }
    public IBGTokenizerResultNext<T, N, R> SelectResult { get; }

    public bool TryGetToken(StringRange value, [MaybeNullWhen(false)] out BGToken<R> token, out StringRange next) {
        if (this.Tokenizer.TryGetToken(value, out var firstToken, out var afterFirst)) {
            if (this.NextTokenizer.TryGetToken(afterFirst, out var nextToken, out var afterNext)) {
                var match = value.SubString(0, afterNext.Start - value.Start);
                token = new BGToken<R>(match, this.SelectResult.Select(firstToken, nextToken, match));
                next = afterNext;
                return true;
            } else {
                token = default;
                next = value;
                return false;
            }
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}