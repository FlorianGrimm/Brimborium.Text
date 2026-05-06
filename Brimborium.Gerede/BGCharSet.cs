namespace Brimborium.Gerede;

public static class BGChar {
    public static BGCharList CList(string value) => new BGCharList(value);
    public static BGCharRange CRange(char from, char to) => new BGCharRange(from, to);
    public static BGCharSet COr(params IBGCharSet[] charSets) => new BGCharSet(charSets);
}

public interface IBGCharSet {
    void AddToTarget(List<char> target);
}

public sealed record BGCharList(
    params char[] Value
    ) : IBGCharSet {
    public BGCharList(string Value) : this(Value.ToCharArray()) { }

    public void AddToTarget(List<char> target) {
        target.EnsureCapacity(target.Count + Value.Length);
        target.AddRange(Value);
    }
    public static BGCharSet operator +(BGCharList left, IBGCharSet right) {
        return new BGCharSet(left, right);
    }
}

public sealed record BGCharRange(
    char From,
    char To) : IBGCharSet {
    public void AddToTarget(List<char> target) {
        if (From <= To) {
            target.EnsureCapacity(target.Count + (To - From) + 1);
            for (char current = From; current <= To; current++) {
                target.Add(current);
            }
        }
    }
    public static BGCharSet operator +(BGCharRange left, IBGCharSet right) {
        return new BGCharSet(left, right);
    }
}

public sealed record BGCharSet(
    params IBGCharSet[] CharSets
    ) : IBGCharSet {
    public void AddToTarget(List<char> target) {
        foreach (var charSet in CharSets) {
            charSet.AddToTarget(target);
        }
    }
    public static BGCharSet operator +(BGCharSet left, BGCharSet right) {
        return new BGCharSet([.. left.CharSets, .. right.CharSets]);
    }
    public static BGCharSet operator +(BGCharSet left, IBGCharSet right) {
        return new BGCharSet([.. left.CharSets, right]);
    }
}

public static class IBGCharSetExtension {
    extension(IBGCharSet charSet) {
        public IEnumerable<char> Build() {
            List<char> target = new();
            charSet.AddToTarget(target);
            return target;
        }
    }
}