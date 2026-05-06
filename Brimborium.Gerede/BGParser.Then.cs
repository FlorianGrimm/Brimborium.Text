namespace Brimborium.Gerede;

public static partial class BGParser {
    extension<T1>(IBGParser<T1> parser) {
        public BGParserThenBuilder<T1, N> Then<N>(
            IBGParser<N> parser2
            ) => new BGParserThenBuilder<T1, N>(
                parser ?? throw new ArgumentNullException(nameof(parser)), 
                parser2 ?? throw new ArgumentNullException(nameof(parser2))
            );
    }
}

public record struct BGParserThenBuilder<T1, T2>(
    IBGParser<T1> Parser1,
    IBGParser<T2> Parser2);

public record struct BGParserThenBuilder<T1, T2, T3>(
    IBGParser<T1> Parser1,
    IBGParser<T2> Parser2,
    IBGParser<T3> Parser3);

public record struct BGParserThenBuilder<T1, T2, T3, T4>(
    IBGParser<T1> Parser1,
    IBGParser<T2> Parser2,
    IBGParser<T3> Parser3,
    IBGParser<T4> Parser4);

public static class BGParserThenBuilderExtension {
    extension<T1, T2>(BGParserThenBuilder<T1, T2> builder) {
        public IBGParser<R> Returns<R>(
            IBGParserSequenceResult<T1, T2, R> selectResult
            ) {
            return new BGParserSequence<T1, T2, R>(
                builder.Parser1,
                builder.Parser2,
                selectResult);
        }

        public IBGParser<R> Returns<R>(
            Func<BGResult<T1>, BGResult<T2>, StringRange, R> selector
            ) {
            return new BGParserSequence<T1, T2, R>(
                builder.Parser1,
                builder.Parser2,
                new BGParserSequenceDelegate<T1, T2, R>(selector));
        }

        public IBGParser<T1> Returns1() {
            return new BGParserSequence<T1, T2, T1>(
                builder.Parser1,
                builder.Parser2,
                new BGParserSequenceDelegate<T1, T2, T1>((value1, value2, _) => value1.Value));
        }

        public IBGParser<T2> Returns2() {
            return new BGParserSequence<T1, T2, T2>(
                builder.Parser1,
                builder.Parser2,
                new BGParserSequenceDelegate<T1, T2, T2>((value1, value2, _) => value2.Value));
        }
        public IBGParser<BGTupple<T1, T2>> ReturnsTupple() {
            return new BGParserSequence<T1, T2, BGTupple<T1, T2>>(
                builder.Parser1,
                builder.Parser2,
                new BGParserSequenceResultTupple<T1, T2>());
        }
        public BGParserThenBuilder<T1, T2, T3> Then<T3>(
            IBGParser<T3> parser3
            ) {
            return new BGParserThenBuilder<T1, T2, T3>(
                builder.Parser1,
                builder.Parser2,
                parser3 ?? throw new ArgumentNullException(nameof(parser3))
                );
        }
    }

    extension<T1, T2, T3>(BGParserThenBuilder<T1, T2, T3> builder) {
        public IBGParser<R> Returns<R>(
            IBGParserSequenceResult<T1, T2, T3, R> selectResult
            ) {
            return new BGParserSequence<T1, T2, T3, R>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                selectResult);
        }

        public IBGParser<R> Returns<R>(
            Func<BGResult<T1>, BGResult<T2>, BGResult<T3>, StringRange, R> selector
            ) {
            return new BGParserSequence<T1, T2, T3, R>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                new BGParserSequenceDelegate<T1, T2, T3, R>(selector));
        }

        public IBGParser<T1> Returns1() {
            return new BGParserSequence<T1, T2, T3, T1>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                new BGParserSequenceDelegate<T1, T2, T3, T1>((value1, _, _, _) => value1.Value));
        }

        public IBGParser<T2> Returns2() {
            return new BGParserSequence<T1, T2, T3, T2>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                new BGParserSequenceDelegate<T1, T2, T3, T2>((_, value2, _, _) => value2.Value));
        }

        public IBGParser<T3> Returns3() {
            return new BGParserSequence<T1, T2, T3, T3>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                new BGParserSequenceDelegate<T1, T2, T3, T3>((_, _, value3, _) => value3.Value));
        }

        public IBGParser<BGTupple<T1, T2, T3>> ReturnsTupple() {
            return new BGParserSequence<T1, T2, T3, BGTupple<T1, T2, T3>>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                new BGParserSequenceResultTupple<T1, T2, T3>());
        }

        public BGParserThenBuilder<T1, T2, T3, T4> Then<T4>(
            IBGParser<T4> parser4
            ) {
            return new BGParserThenBuilder<T1, T2, T3, T4>(
                    builder.Parser1,
                    builder.Parser2,
                    builder.Parser3,
                    parser4 ?? throw new ArgumentNullException(nameof(parser4))
                );
        }
    }

    extension<T1, T2, T3, T4>(BGParserThenBuilder<T1, T2, T3, T4> builder) {
        public IBGParser<R> Returns<R>(
            IBGParserSequenceResult<T1, T2, T3, T4, R> selectResult
            ) {
            return new BGParserSequence<T1, T2, T3, T4, R>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                selectResult);
        }

        public IBGParser<R> Returns<R>(
            Func<BGResult<T1>, BGResult<T2>, BGResult<T3>, BGResult<T4>, StringRange, R> selector
            ) {
            return new BGParserSequence<T1, T2, T3, T4, R>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceDelegate<T1, T2, T3, T4, R>(selector));
        }

        public IBGParser<T1> Returns1() {
            return new BGParserSequence<T1, T2, T3, T4, T1>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceDelegate<T1, T2, T3, T4, T1>((value1, _, _, _, _) => value1.Value));
        }

        public IBGParser<T2> Returns2() {
            return new BGParserSequence<T1, T2, T3, T4, T2>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceDelegate<T1, T2, T3, T4, T2>((_, value2, _, _, _) => value2.Value));
        }

        public IBGParser<T3> Returns3() {
            return new BGParserSequence<T1, T2, T3, T4, T3>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceDelegate<T1, T2, T3, T4, T3>((_, _, value3, _, _) => value3.Value));
        }

        public IBGParser<T4> Returns4() {
            return new BGParserSequence<T1, T2, T3, T4, T4>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceDelegate<T1, T2, T3, T4, T4>((_, _, _, value4, _) => value4.Value));
        }

        public IBGParser<BGTupple<T1, T2, T3, T4>> ReturnsTupple() {
            return new BGParserSequence<T1, T2, T3, T4, BGTupple<T1, T2, T3, T4>>(
                builder.Parser1,
                builder.Parser2,
                builder.Parser3,
                builder.Parser4,
                new BGParserSequenceResultTupple<T1, T2, T3, T4>());
        }

        /*
        public BGParserThenBuilder<T1, T2, T3, T4, T4> Then<T5>(
            IBGParser<T5> parser5
            ) {
            return new BGParserThenBuilder<T1, T2, T3, T4, T5>(
                    builder.Parser1,
                    builder.Parser2,
                    builder.Parser3,
                    builder.Parser4,
                    parser5 ?? throw new ArgumentNullException(nameof(parser5))
                );
        }
        */
    }
}
