namespace Brimborium.Text;

[System.Diagnostics.DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct SubString {
    public static SubString Empty => new SubString(string.Empty);

    // the text is not null
    private readonly string _Text = String.Empty;
    
    // range start and stop are from start
    private readonly Range _Range;

    public SubString(
       string text
     ) {
        this._Text = text;
        this._Range = new Range(0, text.Length);
    }
    public SubString(
        string text,
        Range range) {
        if (range.Start.IsFromEnd || range.End.IsFromEnd) {
            var (offset, length) = range.GetOffsetAndLength(text.Length);
            range = new Range(offset, offset + length);
        }
        if (text.Length < range.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (text.Length < range.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (range.End.Value < range.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }

        this._Text = text;
        this._Range = range;
    }

    public SubString GetSubString(int start, int length) {
        if (start < 0) { throw new ArgumentOutOfRangeException(nameof(start)); }
        if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this._Text.Length);
        if (thisLength < length) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var nextRange = new Range(thisOffset + start, thisOffset + start + length);
        return new SubString(
            this._Text,
            nextRange
            );
    }

    public SubString GetSubString(Range range) {
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this._Text.Length);
        var (rangeOffset, rangeLength) = range.GetOffsetAndLength(thisLength);

        var nextRange = new Range(thisOffset + rangeOffset, thisOffset + rangeOffset + rangeLength);
        if (nextRange.Start.Value > nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisLength < nextRange.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisLength < nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }

        return new SubString(
            this._Text,
            nextRange
            );
    }

    public string Text => this.ToString();
    public Range Range => _Range;

    public int Start => this.Range.Start.Value;

    public int Length {
        get {
            var (_, length) = this.Range.GetOffsetAndLength(this._Text.Length);
            return length;
        }
    }

    public int End => this.Range.End.Value;

    public override string ToString() {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        if (length == 0) { return string.Empty; }
        if (offset == 0 && length == this._Text.Length) {
            return this._Text;
        } else {
            return this._Text.Substring(offset, length);
        }
    }

    public ReadOnlySpan<char> AsSpan()
        => this._Text.AsSpan()[this.Range];

    private string GetDebuggerDisplay() {
        if (this._Text is null) { return "null"; }
        if (this.Length < 32) {
            return $"{this._Text.Substring(this.Start)}[{this.Range}]";
        }
        return $"{this._Text.Substring(this.Start, 32)}...[{this.Range}]";
    }

    public bool IsNullOrEmpty() {
        return this._Text is null || this.Length == 0;
    }

    public bool IsNullOrWhiteSpace() {
        if (this._Text == null) return true;
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);

        for (int idx = 0; idx < length; idx++) {
            if (!char.IsWhiteSpace(this._Text[offset + idx])) { return false; }
        }

        return true;
    }

    public List<SubString> Split(char[] arraySeparator, int maxCount = -1) {
        List<SubString> result = new();
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        var end = offset + length;
        int startPart = offset;
        for (int idx = offset; idx < end; idx++) {
            bool found = false;
            char current = this._Text[idx];
            if (arraySeparator.Length == 1) {
                found = arraySeparator[0] == current;
            } else {
                foreach (char charSeparator in arraySeparator) {
                    found = (charSeparator == current);
                    if (found) { break; }
                }
            }
            if (found) {
                result.Add(new SubString(this._Text, startPart..idx));
                startPart = idx + 1;
                if (maxCount > 0 && maxCount == (result.Count + 1)) {
                    break;
                }
            }
        }
        if (startPart < this._Range.End.Value) {
            result.Add(new SubString(this._Text, startPart..this._Range.End));
        }
        return result;
    }
}
