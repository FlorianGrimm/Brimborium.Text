
//namespace Brimborium.Gerede;

//public interface IBGChain<TResult> { }
//public interface IBGChain<T1,T2> { }

//public class BGChain<TCurrent> {
//    public readonly IBGParser<TCurrent> Parser;

//    public BGChain(
//        IBGParser<TCurrent> parser
//    ) {
//        this.Parser = parser;
//    }

//    public BGChain<TCurrent, TNext> Then<TNext>(
//        IBGParser<TNext> parser
//    ) {
//        return new BGChain<TCurrent, TNext>(
//            this,
//            parser
//            );
//    }

//    //public IBGParser<TResult> EndsWith<TResult>(
//    //    Func<T1, TResult> selectResult
//    //    ) {
//    //    return new BGChainParser<TResult, T1, T1>();
//    //}
//}

//public class BGChain<TPrev, TCurrent> {
//    public readonly IBGChain<TPrev> Chain;
//    public readonly IBGParser<TCurrent> Parser;

//    public BGChain(
//        IBGChain<TPrev> chain,
//        IBGParser<TCurrent> parser
//    ) {
//        this.Chain = chain;
//        this.Parser = parser;
//    }

//    public BGChain<TPrev, TCurrent, TNext> Then<TNext>(
//        IBGParser<TNext> parser
//    ) {
//        return new BGChain<TPrev, TCurrent, TNext>(
//            this,
//            parser);
//    }

//    public IBGParser<TResult> EndsWith<TResult>(
//        Func<TPrev, TCurrent, TResult> selectResult
//        ) {
//        return new BGChainParser<TResult, TCurrent, TPrev>(
//            this.Chain,
//            this.Parser,
//            selectResult);
//    }
//}

//public class BGChain<T1, T2, T3> 
//    : IBGChain<T3> {
//    public readonly IBGChain<T1, T2> Chain;
//    public readonly IBGParser<T3> Parser3;

//    public BGChain(IBGChain<T1, T2> chain, IBGParser<T3> parser) {
//        this.Chain = chain;
//        this.Parser = parser;
//    }

//    public IBGParser<TResult> EndsWith<TResult>(
//        ) {
//        return default!;
//    }
//}

///**/

//public class BGChainParser<TResult, TCurrent, TPrev>
//    : IBGParser<TResult> {
//    public readonly IBGParser<TPrev> ParserPrev;
//    public readonly IBGParser<TCurrent> ParserCurrent;
//    public readonly Func<TPrev, TCurrent, TResult> SelectResult;

//    public BGChainParser(
//        IBGParser<TPrev> parserPrev,
//        IBGParser<TCurrent> parserCurrent,
//        Func<TPrev, TCurrent, TResult> selectResult
//    ) {
//        this.ParserPrev = parserPrev;
//        this.ParserCurrent = parserCurrent;
//        this.SelectResult = selectResult;
//    }


//    public bool Parse(
//        BGParserInput input,
//        [MaybeNullWhen(false)] out BGResult<TResult> match,
//        [MaybeNullWhen(true)] out BGError error,
//        out BGParserInput next) {
//        if (this.ParserPrev.Parse(input, out var prevMatch, out var prevError, out var prevNext)) {
//            if (this.ParserCurrent.Parse(prevNext, out var currentMatch, out var currentError, out var currentNext)) {
//                match = new BGResult<TResult>();
//                error = default;
//                next = currentNext;
//                return true;
//            }
//        }
//        {
//            match = default!;
//            error = new BGError();
//            next = input;
//            return false;
//        }
//    }
//}
