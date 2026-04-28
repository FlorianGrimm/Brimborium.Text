namespace Brimborium.Gerede;

public interface IBGTokenizerListCombiner<TResult, TInner> {
    TResult Aggregate(List<TInner> listResult, StringRange match);
}

public class BGTokenizerListCombiner<TResult, TInner> 
    : IBGTokenizerListCombiner<TResult, TInner> {
    public readonly Func<List<TInner>, StringRange, TResult> SelectResult;

    public BGTokenizerListCombiner(
            Func<List<TInner>, StringRange, TResult> selectResult
        ) {
        this.SelectResult = selectResult;
    }

    public TResult Aggregate(List<TInner> listResult, StringRange match) {
        return this.SelectResult(listResult, match);
    }
}

public class BGTokenizerListCombine<TResult, TInner>
    : IBGTokenizer<TResult> {
    public readonly IBGTokenizer<TInner>[] ListTokenizer;
    public readonly IBGTokenizerListCombiner<TResult, TInner> SelectResult;

    public BGTokenizerListCombine(
            IEnumerable<IBGTokenizer<TInner>> listTokenizer,
            IBGTokenizerListCombiner<TResult, TInner> selectResult
        ) {
        this.ListTokenizer = (listTokenizer is IBGTokenizer<TInner>[] list)
            ? list
            : listTokenizer.ToArray();
        this.SelectResult = selectResult;
    }

    public bool TryGetToken(
        StringRange value,
        [MaybeNullWhen(false)] out BGToken<TResult> token,
        out StringRange next) {
        StringRange current = value;
        List<TInner> listResult = new();
        bool success = false;
        foreach (var tokenizer in this.ListTokenizer) {
            if (tokenizer.TryGetToken(value, out var thisToken, out var thisNext)) {
                listResult.Add(thisToken.Value);
                current = thisNext;
                success = true;
            } else {
                token = default;
                next = value;
                return false;
            }
        }
        if (success) {
            var length = current.End - value.Start;
            StringRange match = value.SubString(0, length);
            var result = this.SelectResult.Aggregate(listResult, match);
            token = new BGToken<TResult>(match, result);
            next = value.SubString(length);
            return true;
        } else {
            token = default;
            next = value;
            return false;
        }
    }
}