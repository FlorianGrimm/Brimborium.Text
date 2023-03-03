using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text;

namespace Brimborium.Text;

[System.Diagnostics.DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
public readonly struct StringSlice : IEquatable<StringSlice> {
    public static StringSlice Empty => new StringSlice(string.Empty);

    // the text is not null
    public readonly string Text = String.Empty;

    // range start and stop are from start
    public readonly Range Range;

    public StringSlice() {
        this.Text = string.Empty;
        this.Range = new Range(0, 0);
    }

    public StringSlice(
       string text
     ) {
        this.Text = text;
        this.Range = new Range(0, text.Length);
    }

    public StringSlice(
        string text,
        Range range) {
        if (range.Start.IsFromEnd || range.End.IsFromEnd) {
            var (offset, length) = range.GetOffsetAndLength(text.Length);
            range = new Range(offset, offset + length);
        }
        if (text.Length < range.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (text.Length < range.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (range.End.Value < range.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }

        this.Text = text;
        this.Range = range;
    }

    public char this[int index] {
        get {
            var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
            if (index < 0) { throw new ArgumentOutOfRangeException(nameof(index)); }
            if (length <= index) { throw new ArgumentOutOfRangeException(nameof(index)); }
            return this.Text[offset + index];
        }
    }
    public StringSlice Substring(int start) {
        if (start < 0) { throw new ArgumentOutOfRangeException(nameof(start)); }
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (thisLength < start) { throw new ArgumentOutOfRangeException(nameof(start)); }
        var nextRange = new Range(thisOffset + start, thisOffset + thisLength);
        return new StringSlice(
            this.Text,
            nextRange
            );
    }

    public StringSlice Substring(int start, int length) {
        if (start < 0) { throw new ArgumentOutOfRangeException(nameof(start)); }
        if (length < 0) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (thisLength < length) { throw new ArgumentOutOfRangeException(nameof(length)); }
        var nextRange = new Range(thisOffset + start, thisOffset + start + length);
        return new StringSlice(
            this.Text,
            nextRange
            );
    }

    public StringSlice Substring(Range range) {
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this.Text.Length);
        var (rangeOffset, rangeLength) = range.GetOffsetAndLength(thisLength);
        var thisEnd = thisOffset + thisLength;

        var nextRange = new Range(thisOffset + rangeOffset, thisOffset + rangeOffset + rangeLength);
        if (nextRange.Start.Value > nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisEnd < nextRange.Start.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }
        if (thisEnd < nextRange.End.Value) { throw new ArgumentOutOfRangeException(nameof(range)); }

        return new StringSlice(
            this.Text,
            nextRange
            );
    }

    public StringSliceState GetTextAndRange()
        => new StringSliceState(this.Text, this.Range);

    public void Deconstruct(out string text, out Range range) {
        text = this.Text;
        range = this.Range;
    }

    public int Length {
        get {
            var (_, length) = this.Range.GetOffsetAndLength(this.Text.Length);
            return length;
        }
    }

    public override string ToString() {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) { return string.Empty; }
        if (offset == 0 && length == this.Text.Length) {
            return this.Text;
        } else {
            return this.Text.Substring(offset, length);
        }
    }

    public ReadOnlySpan<char> AsSpan() {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        return this.Text.AsSpan(offset, length);
    }

    public ReadOnlyMemory<char> AsMemory()
        => this.Text.AsMemory(this.Range);

    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    private string GetDebuggerDisplay() {
        if (this.Text is null) { return "null"; }
        if (this.Length < 32) {
            return this.Text[this.Range];
        } else {
            return this.Text[new Range(this.Range.Start, this.Range.Start.Value + 32)];
        }
    }

    public bool IsNullOrEmpty() {
        return this.Text is null || this.Length == 0;
    }

    public bool IsNullOrWhiteSpace() {
        if (this.Text == null) return true;
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);

        for (int idx = 0; idx < length; idx++) {
            if (!char.IsWhiteSpace(this.Text[offset + idx])) { return false; }
        }

        return true;
    }

    public int IndexOf(char search) {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            if (this.Text[idx] == search) {
                return idx - offset;
            }
        }
        return -1;
    }

    public int IndexOf(char search, Range range) {
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this.Text.Length);
        var (offset, length) = range.GetOffsetAndLength(thisLength);
        offset += thisOffset;
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            if (this.Text[idx] == search) {
                return idx - thisOffset;
            }
        }
        return -1;
    }

    public int IndexOfAny(char[] search) {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            foreach (var s in search) {
                if (this.Text[idx] == s) {
                    return idx - offset;
                }
            }
        }
        return -1;
    }

    public int IndexOfAny(char[] search, Range range) {
        var (thisOffset, thisLength) = this.Range.GetOffsetAndLength(this.Text.Length);
        var (offset, length) = range.GetOffsetAndLength(thisLength);
        offset += thisOffset;
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            foreach (var s in search) {
                if (this.Text[idx] == s) {
                    return idx - thisOffset;
                }
            }
        }
        return -1;
    }

    public bool StartsWith(string search, StringComparison stringComparison) => this.AsSpan().StartsWith(search, stringComparison);
    
    public bool StartsWith(StringSlice search, StringComparison stringComparison) => this.AsSpan().StartsWith(search.AsSpan(), stringComparison);

    public bool StartsWith(ReadOnlySpan<char> search, StringComparison stringComparison) => this.AsSpan().StartsWith(search, stringComparison);

    public StringSlice TrimStart() {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) { return this; }
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            if (char.IsWhiteSpace(this.Text[idx])) {
                continue;
            } else {
                if (idx == offset) {
                    return this;
                } else {
                    return new StringSlice(this.Text, idx..end);
                }
            }
        }
        return new StringSlice(this.Text, end..end);
    }

    public StringSlice TrimStart(char[] chars) {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) { return this; }
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            bool found = false;
            if (chars.Length == 1) {
                found = (this.Text[idx] == chars[0]);
            } else {
                foreach (var c in chars) {
                    found = (this.Text[idx] == c);
                    if (found) { break; }
                }
            }
            if (found) {
                continue;
            } else {
                if (idx == offset) {
                    return this;
                } else {
                    return new StringSlice(this.Text, idx..end);
                }
            }
        }
        return new StringSlice(this.Text, end..end);
    }

    public StringSlice TrimWhile(Func<char, int, int> decide) {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) { return this; }
        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            var decision = decide(this.Text[idx], idx - offset);
            if (decision == 0) {
                continue;
            } else if (decision < 0) {
                return new StringSlice(this.Text, idx..idx);
            } else if (decision > 0) {
                if (idx == offset) {
                    return this;
                } else {
                    return new StringSlice(this.Text, idx..end);
                }
            }
        }
        return new StringSlice(this.Text, end..end);
    }
    public SplitInto SplitInto(char[] arraySeparator, char[]? arrayStop = default) {
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) {
            return new SplitInto(StringSlice.Empty, StringSlice.Empty);
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
            return new SplitInto(
                new StringSlice(this.Text, new Range(offset, end)),
                new StringSlice(this.Text, end..end));
        } else {
            var endSep = offset + posSep;
            return new SplitInto(
                new StringSlice(this.Text, new Range(offset, endSep)),
                (new StringSlice(this.Text, endSep..end)).TrimStart(arraySeparator)
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
        var (offset, length) = this.Range.GetOffsetAndLength(this.Text.Length);
        if (length == 0) { return new SplitInto(this, this); }

        var end = offset + length;
        for (int idx = offset; idx < end; idx++) {
            var result = decide(this.Text[idx], idx);
            if (result == 0) {
                continue;
            } else if (result > 0) {
                return new SplitInto(
                    new StringSlice(this.Text, new Range(offset, idx)),
                    new StringSlice(this.Text, idx..end));
            } else if (result < 0) {
                return new SplitInto(
                    new StringSlice(this.Text, new Range(offset, idx)),
                    new StringSlice(this.Text, idx..idx));
            }
        }
        return new SplitInto(this, new StringSlice(this.Text, end..end));
    }

    public override bool Equals([NotNullWhen(true)] object? obj) {
        if (obj is StringSlice other) { return this.Equals(other); }
        if (obj is string text) { return this.Equals(text.AsSpan(), StringComparison.Ordinal); }
        return false;
    }

    public bool Equals(StringSlice other) {
        var t = this.AsSpan();
        var o = other.AsSpan();
        if (t.Length != o.Length) { return false; }
        return t.StartsWith(o, StringComparison.Ordinal);
    }

    public bool Equals(string other, StringComparison stringComparison) {
        var t = this.AsSpan();
        var o = other.AsSpan();
        if (t.Length != o.Length) { return false; }
        return t.StartsWith(o, stringComparison);
    }

    public bool Equals(StringSlice other, StringComparison stringComparison) {
        var t = this.AsSpan();
        var o = other.AsSpan();
        if (t.Length != o.Length) { return false; }
        return t.StartsWith(o, stringComparison);
    }

    public bool Equals(ReadOnlySpan<char> other, StringComparison stringComparison) {
        var t = this.AsSpan();
        if (t.Length != other.Length) { return false; }
        return t.StartsWith(other, stringComparison);
    }

    public override int GetHashCode() => string.GetHashCode(this.AsSpan());

    public Enumerator GetEnumerator() => new Enumerator(this.AsSpan());

    public ref struct Enumerator {
        private readonly ReadOnlySpan<char> _Span;
        private int _Index;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal Enumerator(ReadOnlySpan<char> span) {
            _Span = span;
            _Index = -1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext() {
            var index = _Index + 1;
            if (index < _Span.Length) {
                _Index = index;
                return true;
            }

            return false;
        }

        public ref readonly char Current {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => ref this._Span[_Index];
        }
    }

    public static implicit operator StringSlice(string? value)
        => new StringSlice(value ?? string.Empty);
}

public readonly record struct SplitInto(StringSlice Found, StringSlice Tail);

public record struct StringSliceState(string Text, Range Range);