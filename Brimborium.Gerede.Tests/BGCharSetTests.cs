namespace Brimborium.Gerede; 

public class BGCharSetTests {
    [Test]
    public async Task BGCharRangeTest() {
        BGCharRange sut = new('A', 'C');
        await Assert.That(string.Join("", sut.Build())).IsEqualTo("ABC");
    }

    [Test]
    public async Task BGCharSetTest() {
        BGCharRange r1 = new('A', 'C');
        BGCharRange r2 = new('a', 'c');
        BGCharSet sut = new(r1, r2);
        await Assert.That(string.Join("", sut.Build())).IsEqualTo("ABCabc");
    }
}
