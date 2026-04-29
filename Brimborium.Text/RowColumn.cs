namespace Brimborium.Text;

public record RowColumn(
    int Row,
    int Column);

public static class RowColumnExtension {
    private static System.Buffers.SearchValues<char> _NewLine
        = System.Buffers.SearchValues.Create((ReadOnlySpan<char>)"\r\n");

    extension(StringRange that) {
        public RowColumn GetRowColumn() {
            int row = 1;
            var text = that.Text.AsSpan();
            var until = ((that.Start + 1) < text.Length) ? (that.Start + 1) : text.Length;
            var start = 0;
            while (start <= until) {
                var indexRelative = text[start..until].IndexOfAny(_NewLine);
                if (indexRelative < 0) {
                    if (until == start) {
                        return new RowColumn(row, 1);
                    } else { 
                        return new RowColumn(row, (until - start));
                    }
                } else {
                    var indexAbsolute = start + indexRelative;
                    if (text[indexAbsolute] == '\n') {
                        start = indexAbsolute + 1;
                        row++;
                    } else if (text[indexAbsolute] == '\r') {
                        if (((indexAbsolute + 1) <= until)
                            && (text[indexAbsolute + 1] == '\n')) {
                            start = indexAbsolute + 2;
                        } else {
                            start = indexAbsolute + 1;
                        }
                        row++;
                    } else {
                        start++;
                    }
                }
            }
            return new RowColumn(row, (until - start)+1);
        }
    }
}
