namespace Brimborium.Text;

public class StringSliceContainerTests {
    [Fact]
    public void ValueStringSliceContainer() {
        var sut = new StringSliceContainer(new StringSlice("abc"));
        sut.Value = new StringSlice("def");
        var act = sut.Value;
        Assert.Equal("def", act.ToString());
    }
}
