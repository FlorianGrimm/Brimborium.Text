namespace Brimborium.Text;

/// <summary>
/// Represents the result of a search operation within a <see cref="StringSlice"/>.
/// Contains information about the foundStart text and its position within the original string.
/// </summary>
public readonly struct StringSliceSearchResult {
    private readonly string _Text;
    private readonly int _BeforeStart;
    private readonly int _FoundStart;
    private readonly int _FoundEnd;
    private readonly int _AfterEnd;

    /// <summary>
    /// Initializes a new instance of the <see cref="StringSliceSearchResult"/> struct.
    /// </summary>
    /// <param name="text">The original string that was searched.</param>
    /// <param name="beforeStart">The start of the original slice that was searched.</param>
    /// <param name="foundStart">The index where the search text was found.</param>
    /// <param name="foundEnd">The index of the foundStart text.</param>
    /// <param name="afterEnd">The end of the original slice that was searched.</param>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="text"/> is null.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the offsets or lengths are invalid.</exception>
    /// <remarks>
    /// Special case: When all foundStart and foundEnd parameters are -1, it represents a "not foundStart" result.
    /// </remarks>
    public StringSliceSearchResult(
        string text,
        int beforeStart,
        int foundStart,
        int foundEnd,
        int afterEnd
        ) {
        if ((-1 == beforeStart) && (-1 == foundStart) && (-1 == foundEnd) && (-1 == afterEnd)) {
            // OK Fault values
        } else {
            if (text is null) {
                throw new ArgumentNullException(nameof(text));
            }
            var textLength = text.Length;
            if (!(0 <= beforeStart)) { throw new ArgumentOutOfRangeException(nameof(beforeStart)); }
            if (!(beforeStart <= foundStart)) { throw new ArgumentOutOfRangeException(nameof(foundStart)); }
            if (!(foundStart <= foundEnd)) { throw new ArgumentOutOfRangeException(nameof(foundEnd)); }
            if (!(foundEnd <= afterEnd)) { throw new ArgumentOutOfRangeException(nameof(afterEnd)); }
            if (!(afterEnd <= textLength)) { throw new ArgumentOutOfRangeException(nameof(afterEnd)); }
        }
        this._Text = text;
        this._BeforeStart = beforeStart;
        this._FoundStart = foundStart;
        this._FoundEnd = foundEnd;
        this._AfterEnd = afterEnd;
    }

    /// <summary>
    /// Gets the original string that was searched.
    /// </summary> 
    public string Text => this._Text;

    /// <summary>
    /// Gets the start of the orginal text or the start.
    /// </summary> 
    public int BeforeStart => this._BeforeStart;

    /// <summary>
    /// Gets the start where the search text was found.
    /// </summary> 
    public int FoundStart => this._FoundStart;

    /// <summary>
    /// Gets the end of the found text.
    /// </summary> 
    public int FoundEnd => this._FoundEnd;

    /// <summary>
    /// Gets the end of the original text or the text after.
    /// </summary>
    public int AfterEnd => this._AfterEnd;

    /// <summary>
    /// Gets a <see cref="StringSlice"/> representing the text beforeStart the foundStart text.
    /// </summary>
    public StringSlice Before => new StringSlice(this._Text, new Range(this._BeforeStart, this._FoundStart));

    /// <summary>
    /// Gets a <see cref="StringSlice"/> representing the text beforeStart and including the foundStart text.
    /// </summary>
    public StringSlice BeforeAndFound => new StringSlice(this._Text, new Range(this._BeforeStart, this._FoundEnd));

    /// <summary>
    /// Gets a <see cref="StringSlice"/> representing only the foundStart text.
    /// </summary>
    public StringSlice Found => new StringSlice(this._Text, new Range(this._FoundStart, this._FoundEnd));

    /// <summary>
    /// Gets a <see cref="StringSlice"/> representing the foundStart text and all text after it.
    /// </summary>
    public StringSlice FoundAndAfter => new StringSlice(this._Text, new Range(this._FoundStart, this._AfterEnd));

    /// <summary>
    /// Gets a <see cref="StringSlice"/> representing only the text after the foundStart text.
    /// </summary>
    public StringSlice After => new StringSlice(this._Text, new Range(this._FoundEnd, this._AfterEnd));
}
