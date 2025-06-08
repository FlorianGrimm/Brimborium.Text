namespace Brimborium.Text;
/// <summary>
/// Value and Difference are the result of an adjust operation.
/// </summary>
public readonly record struct AdjustResult(
    /// <summary>
    /// The adjusted value.
    /// </summary>
    StringSlice Value,
    /// <summary>
    /// The difference between the original value and the adjusted value.
    /// </summary>
    StringSlice Difference
    );
