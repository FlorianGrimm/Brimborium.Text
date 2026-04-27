namespace Brimborium.Text;

/// <summary>
/// Represents a lightweight, immutable view into a portion of a string defined by an
/// absolute start and end position within the underlying <see cref="Text"/>.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="StringRange"/> is similar in spirit to <see cref="StringSlice"/> but exposes the
/// boundaries as plain <see cref="int"/> fields (<see cref="Start"/> and <see cref="End"/>)
/// instead of a <see cref="System.Range"/>. All boundary values are absolute indices into
/// <see cref="Text"/> and are never relative to the slice itself.
/// </para>
/// <para>
/// Because <see cref="StringRange"/> is a <see langword="readonly struct"/>, instances are
/// immutable; operations that derive a new range (such as <see cref="SubString(int)"/> or the
/// <see cref="this[System.Range]"/> indexer) return new <see cref="StringRange"/> values
/// without copying the underlying string data.
/// </para>
/// </remarks>
public readonly struct StringRange {
    /// <summary>
    /// The underlying string referenced by this range. Always non-<see langword="null"/>;
    /// an empty range is represented by an empty <see cref="Text"/> and zero
    /// <see cref="Start"/>/<see cref="End"/> values.
    /// </summary>
    public readonly string Text;

    /// <summary>
    /// The inclusive zero-based start index of the range within <see cref="Text"/>.
    /// </summary>
    public readonly int Start;

    /// <summary>
    /// The exclusive zero-based end index of the range within <see cref="Text"/>.
    /// </summary>
    public readonly int End;

    /// <summary>
    /// Initializes a new instance of <see cref="StringRange"/> referencing
    /// <see cref="string.Empty"/> with <see cref="Start"/> and <see cref="End"/> set to zero.
    /// </summary>
    public StringRange() {
        this.Text = string.Empty;
        this.Start = 0;
        this.End = 0;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StringRange"/> that spans the entire
    /// supplied <paramref name="text"/>.
    /// </summary>
    /// <param name="text">The underlying string to reference. Must not be <see langword="null"/>.</param>
    public StringRange(string text) {
        this.Text = text;
        this.Start = 0;
        this.End = text.Length;
    }

    /// <summary>
    /// Initializes a new instance of <see cref="StringRange"/> with explicit absolute boundaries.
    /// </summary>
    /// <param name="text">The underlying string to reference.</param>
    /// <param name="start">The inclusive zero-based start index within <paramref name="text"/>.</param>
    /// <param name="end">The exclusive zero-based end index within <paramref name="text"/>.</param>
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="start"/> or <paramref name="end"/> exceed the length of
    /// <paramref name="text"/>, or when <paramref name="end"/> is smaller than
    /// <paramref name="start"/>.
    /// </exception>
    public StringRange(
        string text,
        int start,
        int end
        ) {
        if (text.Length < start) { throw new ArgumentException("start is gr than the string"); }
        if (text.Length < end) { throw new ArgumentException("end is bigger than the string"); }
        if (end < start) { throw new ArgumentException("end before start"); }
        this.Text = text;
        this.Start = start;
        this.End = end;
    }

    /// <summary>
    /// Gets a value indicating whether this range covers no characters
    /// (i.e. <see cref="Start"/> equals <see cref="End"/>).
    /// </summary>
    public readonly bool IsEmpty => this.Start == this.End;

    /// <summary>
    /// Gets a <see cref="System.Range"/> equivalent to this <see cref="StringRange"/>'s
    /// <see cref="Start"/> and <see cref="End"/> values.
    /// </summary>
    public readonly Range Range => new Range(this.Start, this.End);

    /// <summary>
    /// Returns a <see cref="ReadOnlySpan{T}"/> over the characters of <see cref="Text"/>
    /// covered by this range.
    /// </summary>
    /// <returns>A span over the slice of <see cref="Text"/> defined by <see cref="Start"/> and <see cref="End"/>.</returns>
    public readonly ReadOnlySpan<char> AsSpan() {
        return this.Text.AsSpan(start: this.Start, length: this.End - this.Start);
    }

    /// <summary>
    /// Returns a new <see cref="StringRange"/> that begins <paramref name="start"/> characters
    /// after the current <see cref="Start"/> and ends at the current <see cref="End"/>.
    /// </summary>
    /// <param name="start">The number of characters to advance the start position by, relative to this range.</param>
    /// <returns>A new <see cref="StringRange"/> starting at <c>this.Start + start</c>.</returns>
    public readonly StringRange SubString(int start) {
        var nextStart = this.Start + start;
        var validStart = (nextStart < this.End) ? nextStart : this.End;
        return new StringRange(this.Text, validStart, this.End);
    }

    /// <summary>
    /// Returns a new <see cref="StringRange"/> that begins <paramref name="start"/> characters
    /// after the current <see cref="Start"/> and spans up to <paramref name="length"/> characters,
    /// clamped to the current <see cref="End"/>.
    /// </summary>
    /// <param name="start">The number of characters to advance the start position by, relative to this range.</param>
    /// <param name="length">The maximum number of characters to include in the returned range.</param>
    /// <returns>A new <see cref="StringRange"/> covering at most <paramref name="length"/> characters.</returns>
    public readonly StringRange SubString(int start, int length) {
        var nextStart = this.Start + start;
        var validStart = (nextStart < this.End) ? nextStart : this.End;
        var nextEnd = validStart + length;
        var validEnd = (nextEnd < this.End) ? nextEnd : this.End;
        return new StringRange(this.Text, validStart, validEnd);
    }

    /// <summary>
    /// Gets the number of characters covered by this range
    /// (i.e. <see cref="End"/> minus <see cref="Start"/>).
    /// </summary>
    public readonly int Length => this.End - this.Start;

    /// <summary>
    /// Gets a sub-range of this <see cref="StringRange"/> using a <see cref="System.Range"/>
    /// expressed relative to this range's <see cref="Length"/>.
    /// </summary>
    /// <param name="range">The relative range to extract.</param>
    /// <returns>A new <see cref="StringRange"/> covering the requested sub-range.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="range"/> resolves to a negative offset or to a length larger
    /// than this range.
    /// </exception>
    public readonly StringRange this[Range range] {
        get {
            var length = this.Length;
            var (rangeOffset, rangeLength) = range.GetOffsetAndLength(length);
            if (rangeOffset < 0) { throw new ArgumentOutOfRangeException(nameof(range)); }
            if (length < rangeLength) { throw new ArgumentOutOfRangeException(nameof(range)); }
            var nextStart = this.Start + rangeOffset;
            return new StringRange(this.Text, nextStart, nextStart + rangeLength);
        }
    }

    /// <summary>
    /// Attempts to retrieve the first character of this range.
    /// </summary>
    /// <param name="result">
    /// When this method returns, contains the first character of the range when it is not empty;
    /// otherwise the default <see cref="char"/> value.
    /// </param>
    /// <returns><see langword="true"/> if a character was returned; otherwise <see langword="false"/>.</returns>
    public readonly bool TryGetFirst(out char result) {
        if (this.IsEmpty) {
            result = default;
            return false;
        } else {
            result = Text[Start];
            return true;
        }
    }

    /// <summary>
    /// Determines whether the characters covered by this range start with the supplied
    /// <paramref name="search"/> string.
    /// </summary>
    /// <param name="search">The string to compare with the beginning of this range.</param>
    /// <param name="comparisonType">The <see cref="StringComparison"/> to use; defaults to <see cref="StringComparison.Ordinal"/>.</param>
    /// <returns><see langword="true"/> when the range starts with <paramref name="search"/>; otherwise <see langword="false"/>.</returns>
    public bool StartsWith(string search, StringComparison comparisonType = StringComparison.Ordinal)
        => this.AsSpan().StartsWith(search, comparisonType);

    public override string ToString()
        => this.Text.Substring(this.Start, this.Length);
}
