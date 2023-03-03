namespace Brimborium.Text;

public class StringSliceExtensionTests {
    [Fact()]
    public void AsStringSliceTest() {
        Assert.Equal("abc", "abc".AsStringSlice().ToString());
        Assert.Equal("bc", "abc".AsStringSlice(1).ToString());
        Assert.Equal("bc", "abcd".AsStringSlice(1,2).ToString());
        Assert.Equal("b", "abcd".AsStringSlice(1..2).ToString());
    }
}
