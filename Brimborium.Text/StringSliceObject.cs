namespace Brimborium.Text;

// TODO: better name StringSliceCache?

/// <summary>
/// A wrapper for <see cref="StringSlice"/> with a cached string.
/// </summary>
public class StringSliceObject : IEquatable<StringSliceObject> {
    private StringSlice _Value;
    private string? _ValueString;

    public StringSliceObject() {
        this._Value = StringSlice.Empty;
    }

    public StringSliceObject(StringSlice value) {
        this._Value = value;
    }

    public StringSlice Value {
        get {
            return this._Value;
        }
        set {
            this._Value = value;
            this._ValueString = default;
        }
    }

    public override bool Equals(object? obj) {
        if (obj is null) { return false; }
        if (ReferenceEquals(this, obj)) { return true; }
        if (obj is StringSliceObject stringSliceCache) { return this.Equals(stringSliceCache); }
        return base.Equals(obj);
    }

    public bool Equals(StringSliceObject? other) 
        => other is not null
        && this._Value.Equals(other._Value, StringComparison.Ordinal);

    public bool Equals(StringSliceObject? other, StringComparison stringComparison) 
        => other is not null
        && this._Value.Equals(other._Value, stringComparison);

    public override int GetHashCode() => this._Value.GetHashCode();
    
    /// <summary>
    /// The SliceString as an string (cached).
    /// </summary>
    /// <returns>The (cached) string.</returns>
    public override string ToString() => this._ValueString ??= this.Value.ToString();

    /// <summary>
    /// Cast a StringSlice to a StringSliceObject
    /// </summary>
    /// <param name="value">the source to convert</param>
    public static implicit operator StringSliceObject(StringSlice value) => new StringSliceObject(value);

    /// <summary>
    /// Cast a StringSliceObject to a StringSlice
    /// </summary>
    /// <param name="value">the source to convert</param>
    public static implicit operator StringSlice(StringSliceObject value) => value.Value;
}
