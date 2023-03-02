namespace Brimborium.Text;

[System.Diagnostics.DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct SubString {
    public static SubString Empty => new SubString(string.Empty);

    // the text is not null
    private readonly string _Text = String.Empty;

    // range start and stop are from start
    private readonly Range _Range;

    public SubString() {
        this._Text = string.Empty;
        this._Range = new Range(0, 0);
    }

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

    public char this[int index] {
        get {
            var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
            if (index < 0) { throw new ArgumentOutOfRangeException(nameof(index)); }
            if (length <= index) { throw new ArgumentOutOfRangeException(nameof(index)); }
            return this._Text[offset + index];
        }
    }

    public SubString GetSubString(int start, int length) {
        if (start < 0) { throw new ArgumentOutOfRangeException(nameof(start)); }
        if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var (thisOffset, thisLength) = this._Range.GetOffsetAndLength(this._Text.Length);
        if (thisLength < length) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var nextRange = new Range(thisOffset + start, thisOffset + start + length);
        return new SubString(
            this._Text,
            nextRange
            );
    }

    public SubString GetSubString(Range range) {
        var (thisOffset, thisLength) = this._Range.GetOffsetAndLength(this._Text.Length);
        var (rangeOffset, rangeLength) = range.GetOffsetAndLength(thisLength);
        var thisEnd = thisOffset + thisLength;

        var nextRange = new Range(thisOffset + rangeOffset, thisOffset + rangeOffset + rangeLength);
        if (nextRange.Start.Value > nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisEnd < nextRange.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisEnd < nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }

        return new SubString(
            this._Text,
            nextRange
            );
    }

    public string Text => this.ToString();

    public int Length {
        get {
            var (_, length) = this._Range.GetOffsetAndLength(this._Text.Length);
            return length;
        }
    }

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
        => this._Text.AsSpan()[this._Range];

    public StringBuilder AsStringBuilder() {
        var result = StringBuilderPool.GetStringBuilder();
        result.Append(this.AsSpan());
        return result;
    }

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private string GetDebuggerDisplay() {
        if (this._Text is null) { return "null"; }
        if (this.Length < 32) {
            return this._Text[this._Range];
        } else {
            return this._Text[new Range(this._Range.Start, this._Range.Start.Value + 32)];
        }
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

    public int IndexOf(char search) {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            if (this._Text[idx] == search) {
                return idx - offset;
            }
        }
        return -1;
    }
    public int IndexOf(char search, Range range) {
        var (thisOffset, thisLength) = this._Range.GetOffsetAndLength(this._Text.Length);
        var (offset, length) = range.GetOffsetAndLength(thisLength);
        offset += thisOffset;
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            if (this._Text[idx] == search) {
                return idx - thisOffset;
            }
        }
        return -1;
    }

    public int IndexOfAny(char[] search) {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            foreach (var s in search) {
                if (this._Text[idx] == s) {
                    return idx - offset;
                }
            }
        }
        return -1;
    }

    public int IndexOfAny(char[] search, Range range) {
        var (thisOffset, thisLength) = this._Range.GetOffsetAndLength(this._Text.Length);
        var (offset, length) = range.GetOffsetAndLength(thisLength);
        offset += thisOffset;
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            foreach (var s in search) {
                if (this._Text[idx] == s) {
                    return idx - thisOffset;
                }
            }
        }
        return -1;
    }

    public SubString TrimStart(char[] chars) {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            bool found = false;
            if (chars.Length == 1) {
                found = (this._Text[idx] == chars[0]);
            } else {
                foreach (var c in chars) {
                    found = (this._Text[idx] == c);
                    if (found) { break; }
                }
            }
            if (found) {
                continue;
            } else {
                if (idx == offset) {
                    return this;
                } else {
                    return new SubString(this._Text, idx..end);
                }
            }
        }
        return SubString.Empty;
    }
    
    public SplitInto SplitInto(char[] arraySeparator, char[]? arrayStop = default) {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        if (length == 0) {
            return new SplitInto(SubString.Empty, SubString.Empty);
        }

        var end = offset + length;
        int posStop;
        if (arrayStop is not null && arrayStop.Length > 0) {
            posStop = this.IndexOfAny(arrayStop);
            if (posStop >= 0) {
                end = offset + posStop;
            }
        }

        int posSep = this.IndexOfAny(arraySeparator, 0..(end - offset));
        if (posSep < 0) {
            return new SplitInto(new SubString(this._Text, new Range(offset, end)), SubString.Empty);
        } else {
            var endSep = offset + posSep;
            return new SplitInto(
                new SubString(this._Text, new Range(offset, endSep)),
                (new SubString(this._Text, endSep..end)).TrimStart(arraySeparator)
                );
        }
    }

    /// <summary>
    /// Split the string into two parts, 
    /// the first part is the string until the decide function returns not 0.
    /// The second part is the rest of the string if <paramref name="decide"/> returns greater than 0.
    /// The second part is empty if <paramref name="decide"/> returns less than 0.
    /// </summary>
    /// <param name="decide">a callback to decide to 
    ///     0 continue,
    ///     greater 0 to return 2 parts the part Found (until before) and the Tail,
    ///     less 0 to return 2 parts the part Found (until before) and an empty Tail.</param>
    /// <returns>2 SubStrings the Found and the Tail.</returns>
    public SplitInto SplitIntoWhile(Func<char, int, int> decide) {
        var (offset, length) = this._Range.GetOffsetAndLength(this._Text.Length);
        if (length == 0) {
            return new SplitInto(SubString.Empty, SubString.Empty);
        }
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            var result = decide(this._Text[idx], idx);
            if (result == 0) {
                continue;
            } else if (result > 0) {
                return new SplitInto(new SubString(this._Text, new Range(offset, idx)), new SubString(this._Text, idx..end));
            } else if (result < 0) {
                return new SplitInto(new SubString(this._Text, new Range(offset, idx)), SubString.Empty);
            }
        }
        return new SplitInto(this, SubString.Empty);
    }
}

public readonly record struct SplitInto(SubString Found, SubString Tail);
