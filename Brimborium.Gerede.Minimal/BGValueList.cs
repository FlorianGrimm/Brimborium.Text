namespace Brimborium.Gerede;

public class BGValueList<T> {
    private readonly List<T> _Items;

    public BGValueList() {
        this._Items = new List<T>();
    }

    public BGValueList(IEnumerable<T> items) {
        this._Items = new List<T>(items);
    }

    public IReadOnlyList<T> Items => this._Items;

    public int Count => this._Items.Count;

    public T this[int index] => this._Items[index];

    public void Add(T value) => this._Items.Add(value);
}

