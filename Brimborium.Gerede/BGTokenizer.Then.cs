namespace Brimborium.Gerede;

public static partial class BGTokenizerExtension {
    extension<T1>(IBGTokenizer<T1> tokenizer) {
        public BGTokenizerThenBuilder<T1, T2> Then<T2>(
            IBGTokenizer<T2> tokenizer2
        ) => new BGTokenizerThenBuilder<T1, T2>(tokenizer, tokenizer2);
    }

    extension<T1, T2>(BGTokenizerThenBuilder<T1, T2> builder) {
        public BGTokenizerThenBuilder<T1, T2, T3> Then<T3>(
            IBGTokenizer<T3> tokenizer3
        ) => new BGTokenizerThenBuilder<T1, T2, T3>(builder.Tokenizer1, builder.Tokenizer2, tokenizer3);

        public IBGTokenizer<R> Returns<R>(
            IBGTokenizerResultTransform<T1, T2, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                selectResult);
        }

        public IBGTokenizer<R> Returns<R>(
            Func<BGToken<T1>, BGToken<T2>, StringRange, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                new BGTokenizerResultTransformDelegate<T1, T2, R>(selectResult));
        }

        public IBGTokenizer<BGVoid> Returns() {
            return new BGTokenizerSequence<T1, T2, BGVoid>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                new BGTokenizerResultTransformDelegate<T1, T2, BGVoid>(
                    static (_, _, _) => new BGVoid()));
        }
    }

    extension<T1, T2, T3>(BGTokenizerThenBuilder<T1, T2, T3> builder) {
        public BGTokenizerThenBuilder<T1, T2, T3, T4> Then<T4>(
            IBGTokenizer<T4> tokenizer4
        ) => new BGTokenizerThenBuilder<T1, T2, T3, T4>(builder.Tokenizer1, builder.Tokenizer2, builder.Tokenizer3, tokenizer4);

        public IBGTokenizer<R> Returns<R>(
            IBGTokenizerResultTransform<T1, T2, T3, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, T3, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                selectResult);
        }

        public IBGTokenizer<R> Returns<R>(
            Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, StringRange, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, T3, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                new BGTokenizerResultTransformDelegate<T1, T2, T3, R>(selectResult));
        }

        public IBGTokenizer<BGVoid> Returns(
            ) {
            return new BGTokenizerSequence<T1, T2, T3, BGVoid>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                new BGTokenizerResultTransformDelegate<T1, T2, T3, BGVoid>(
                    static (_, _, _, _) => new BGVoid()));
        }
    }
    extension<T1, T2, T3, T4>(BGTokenizerThenBuilder<T1, T2, T3, T4> builder) {
        public IBGTokenizer<R> Returns<R>(
            IBGTokenizerResultTransform<T1, T2, T3, T4, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, T3, T4, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                builder.Tokenizer4,
                selectResult);
        }

        public IBGTokenizer<R> Returns<R>(
            Func<BGToken<T1>, BGToken<T2>, BGToken<T3>, BGToken<T4>, StringRange, R> selectResult
            ) {
            return new BGTokenizerSequence<T1, T2, T3, T4, R>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                builder.Tokenizer4,
                new BGTokenizerResultTransformDelegate<T1, T2, T3, T4, R>(selectResult));
        }

        public IBGTokenizer<BGVoid> Returns(
            ) {
            return new BGTokenizerSequence<T1, T2, T3, T4, BGVoid>(
                builder.Tokenizer1,
                builder.Tokenizer2,
                builder.Tokenizer3,
                builder.Tokenizer4,
                new BGTokenizerResultTransformDelegate<T1, T2, T3, T4, BGVoid>(
                    static (_, _, _, _, _) => new BGVoid()));
        }
    }
}

public record struct BGTokenizerThenBuilder<T1, T2>(
    IBGTokenizer<T1> Tokenizer1,
    IBGTokenizer<T2> Tokenizer2);

public record struct BGTokenizerThenBuilder<T1, T2, T3>(
    IBGTokenizer<T1> Tokenizer1,
    IBGTokenizer<T2> Tokenizer2,
    IBGTokenizer<T3> Tokenizer3);

public record struct BGTokenizerThenBuilder<T1, T2, T3, T4>(
    IBGTokenizer<T1> Tokenizer1,
    IBGTokenizer<T2> Tokenizer2,
    IBGTokenizer<T3> Tokenizer3,
    IBGTokenizer<T4> Tokenizer4);

public sealed class BGTokenizerSequence<T1, T2, R>(
        IBGTokenizer<T1> tokenizer1, 
        IBGTokenizer<T2> tokenizer2, 
        IBGTokenizerResultTransform<T1, T2, R> selectResult
    ) : IBGTokenizer<R> {
    public IBGTokenizer<T1> Tokenizer1 { get; } = tokenizer1;
    public IBGTokenizer<T2> Tokenizer2 { get; } = tokenizer2;
    public IBGTokenizerResultTransform<T1, T2, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<R> token,
        out StringRange next) {
        if (!this.Tokenizer1.TryGetToken(value, out var token1, out var next1)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer2.TryGetToken(next1, out var token2, out var next2)) {
            token = default;
            next = value;
            return false;
        }

        {
            var tokenMatch = value.Substring(0, (next2.Start - value.Start));
            var tokenValue = this.SelectResult.Select(token1, token2, tokenMatch);
            token = new BGToken<R>(tokenMatch, tokenValue);
            next = next2;
            return true;
        }
    }
}

public sealed class BGTokenizerSequence<T1, T2, T3, R>(
        IBGTokenizer<T1> tokenizer1,
        IBGTokenizer<T2> tokenizer2,
        IBGTokenizer<T3> tokenizer3,
        IBGTokenizerResultTransform<T1, T2, T3, R> selectResult
    ) : IBGTokenizer<R> {
    public IBGTokenizer<T1> Tokenizer1 { get; } = tokenizer1;
    public IBGTokenizer<T2> Tokenizer2 { get; } = tokenizer2;
    public IBGTokenizer<T3> Tokenizer3 { get; } = tokenizer3;
    public IBGTokenizerResultTransform<T1, T2, T3, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<R> token,
        out StringRange next) {
        if (!this.Tokenizer1.TryGetToken(value, out var token1, out var next1)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer2.TryGetToken(next1, out var token2, out var next2)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer3.TryGetToken(next2, out var token3, out var next3)) {
            token = default;
            next = value;
            return false;
        }

        {
            var tokenMatch = value.Substring(0, (next3.Start - value.Start));
            var tokenValue = this.SelectResult.Select(token1, token2, token3, tokenMatch);
            token = new BGToken<R>(tokenMatch, tokenValue);
            next = next3;
            return true;
        }
    }
}

public sealed class BGTokenizerSequence<T1, T2, T3, T4, R>(
    IBGTokenizer<T1> tokenizer1,
    IBGTokenizer<T2> tokenizer2,
    IBGTokenizer<T3> tokenizer3,
    IBGTokenizer<T4> tokenizer4,
    IBGTokenizerResultTransform<T1, T2, T3, T4, R> selectResult) : IBGTokenizer<R> {
    public IBGTokenizer<T1> Tokenizer1 { get; } = tokenizer1;
    public IBGTokenizer<T2> Tokenizer2 { get; } = tokenizer2;
    public IBGTokenizer<T3> Tokenizer3 { get; } = tokenizer3;
    public IBGTokenizer<T4> Tokenizer4 { get; } = tokenizer4;
    public IBGTokenizerResultTransform<T1, T2, T3, T4, R> SelectResult { get; } = selectResult;

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<R> token,
        out StringRange next) {
        if (!this.Tokenizer1.TryGetToken(value, out var token1, out var next1)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer2.TryGetToken(next1, out var token2, out var next2)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer3.TryGetToken(next2, out var token3, out var next3)) {
            token = default;
            next = value;
            return false;
        }

        if (!this.Tokenizer4.TryGetToken(next3, out var token4, out var next4)) {
            token = default;
            next = value;
            return false;
        }

        {
            var tokenMatch = value.Substring(0, (next4.Start - value.Start));
            var tokenValue = this.SelectResult.Select(token1, token2, token3, token4, tokenMatch);
            token = new BGToken<R>(tokenMatch, tokenValue);
            next = next4;
            return true;
        }
    }
}

//