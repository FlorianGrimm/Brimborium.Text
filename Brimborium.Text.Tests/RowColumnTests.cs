using System;
using System.Collections.Generic;
using System.Text;

namespace Brimborium.Text; 
public class RowColumnTests {
    [Test]
    public async Task RowColumnTest() {
        await Assert.That(
            new StringRange("abc\r\ndef\r\n")
                .GetRowColumn()
            ).IsEquivalentTo(new RowColumn(1, 1));

        await Assert.That(
            new StringRange("abc\r\ndef\r\n").Substring(2)
                .GetRowColumn()
            ).IsEquivalentTo(new RowColumn(1,3));

        await Assert.That(
            new StringRange("abc\r\ndef\r\n").Substring(5)
                .GetRowColumn()
            ).IsEquivalentTo(new RowColumn(2, 1));

        await Assert.That(
            new StringRange("abc\r\ndef\r\n").Substring(6)
                .GetRowColumn()
            ).IsEquivalentTo(new RowColumn(2, 2));

        await Assert.That(
            new StringRange("abc\r\ndef\r\n").Substring(10)
                .GetRowColumn()
            ).IsEquivalentTo(new RowColumn(3, 1));
    }
}
