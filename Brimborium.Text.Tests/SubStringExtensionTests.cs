namespace Brimborium.Text;

public class SubStringExtensionTests {
    [Fact()]
    public void AsSubStringTest() {
        Assert.Equal("abc", "abc".AsSubString().ToString());
        Assert.Equal("bc", "abc".AsSubString(1).ToString());
        Assert.Equal("bc", "abcd".AsSubString(1,2).ToString());
        Assert.Equal("b", "abcd".AsSubString(1..2).ToString());
    }
}
