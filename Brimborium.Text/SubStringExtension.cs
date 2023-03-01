namespace Brimborium.Text;

public static class SubStringExtension {
    public static SubString AsSubString(this string value) {
        return new SubString(value);
    }
    public static SubString AsSubString(this string value, int pos) {
        return new SubString(value, new Range(pos, value.Length));
    }

    public static SubString AsSubString(this string value, int pos, int length) {
        return new SubString(value, new Range(pos, pos + length));
    }
    public static SubString AsSubString(this string value, Range range) {
        return new SubString(value, range);
    }
}