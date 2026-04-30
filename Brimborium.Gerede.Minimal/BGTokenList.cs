namespace Brimborium.Gerede;

public sealed class BGTokenList {
    private readonly List<object?> _Items = new();

    public IReadOnlyList<object?> Items => this._Items;

    public int Count => this._Items.Count;

    public void Add<T>(BGToken<T> token) => this._Items.Add(token);

    public void Add(object? token) => this._Items.Add(token);
}