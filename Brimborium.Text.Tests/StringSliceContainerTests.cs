namespace Brimborium.Text;

public class StringSliceContainerTests {
    [Test]
    public async Task ValueStringSliceContainer() {
        var sut = new StringSliceContainer(new StringSlice("abc"));
        sut.Value = new StringSlice("def");
        var act = sut.Value;
        await Assert.That(act.ToString()).IsEqualTo("def");
    }
}